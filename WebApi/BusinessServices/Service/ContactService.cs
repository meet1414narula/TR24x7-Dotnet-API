using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using Common.Constants;

namespace BusinessServices
{
    /// <summary>
    /// Offers services for goods specific CRUD operations
    /// </summary>
    public class ContactService:IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOTPService _otpService;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ContactService(IUnitOfWork unitOfWork,IOTPService otpService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        /// <summary>
        /// Fetches all the goodss.
        /// </summary>
        /// <returns></returns>
        public List<BusinessEntities.ContactResponseEntity> GetAllContacts(List<int> roadlines)
        {
            var contacts = _unitOfWork.ContactRepository.GetAll().ToList();
            if (contacts.Any())
            {
                return MapGoods(contacts);
            }
            return null;
        }

        /// <summary>
        /// Creates a goods
        /// </summary>
        /// <param name="contactEntity"></param>
        /// <returns></returns>
        public long CreateContact(BusinessEntities.ContactRequestEntity contactEntity)
        {
            using (var scope = new TransactionScope())
            {
                var contact = new Contact
                {
                    Name = contactEntity.Name,
                    Mobile= contactEntity.Mobile,
                    Rating = contactEntity.Rating,
                    DisplayOrder = contactEntity.DisplayOrder,
                    Rank = contactEntity.Rank,
                    CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    UserTypeFID = contactEntity.UserTypeId,
                    IsActive = true
                };

                _unitOfWork.ContactRepository.Insert(contact);
                // _unitOfWork.Save();

                if (!string.IsNullOrEmpty(contactEntity.OtherRoadLines))
                {
                    foreach (var item in contactEntity.OtherRoadLines.Split(','))
                    {
                        var roadline = new RoadLine
                        {
                            Name = item,
                            DisplayOrder = contactEntity.DisplayOrder,
                            Rank = contactEntity.Rank,
                            IsActive = true
                        };
                        _unitOfWork.RoadLineRepository.Insert(roadline);

                        var contactRoadline = new Contact_RoadLine_Mapping
                        {
                            ContactFID = contact.ContactPID,
                            ContactRoadLineMappingPID = roadline.RoadLinePID
                        };
                        _unitOfWork.ContactRoadLineMappingRepository.Insert(contactRoadline);

                        //_unitOfWork.Save();
                    }
                }

                foreach (var item in contactEntity.RoadLines)
                {
                    var contactRoadline = new Contact_RoadLine_Mapping
                    {
                        ContactFID = contact.ContactPID,
                        ContactRoadLineMappingPID = item
                    };
                    _unitOfWork.ContactRoadLineMappingRepository.Insert(contactRoadline);
                }

                _unitOfWork.Save();
                scope.Complete();
                return contact.ContactPID;
            }
        }

        /// <summary>
        /// Updates a goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="contactEntity"></param>
        /// <returns></returns>
        public bool UpdateContact(int contactId, BusinessEntities.ContactRequestEntity contactEntity)
        {
            var success = false;
            if (contactEntity != null)
            {
                DeleteContact(contactId);
                CreateContact(contactEntity);
            }
            return success;
        }

        /// <summary>
        /// Deletes a particular goods
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public bool DeleteContact(int contactId)
        {
            var success = false;
            if (contactId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var goods = _unitOfWork.ContactRoadLineMappingRepository.GetMany(x=>x.ContactFID==contactId);
                    if (goods != null)
                    {
                        foreach (var item in goods)
                        {
                            _unitOfWork.ContactRoadLineMappingRepository.Delete(item);
                        }
                        _unitOfWork.ContactRepository.GetByID(contactId);
                        _unitOfWork.ContactRepository.Delete(goods);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        private List<ContactResponseEntity> MapGoods(List<Contact> goods)
        {
            List<ContactResponseEntity> listGoodsEntity  = new List<ContactResponseEntity>();
          
            if (goods != null && goods.Count() > 0)
            {
                goods = goods.OrderByDescending(x => x.CreationDate).ToList();

                listGoodsEntity.AddRange(goods.Select(x =>
                {
                    return new ContactResponseEntity
                    {
                        ContactId = Convert.ToInt32(x.ContactPID),
                        Name = x.Name,
                        Mobile = x.Mobile,
                        Rating = Convert.ToInt32(x.Rating),
                    };
                }
                ));
            }
            return listGoodsEntity;
        }

        public List<ContactResponseEntity> GetAllContacts()
        {
            throw new NotImplementedException();
        }
    }
}
