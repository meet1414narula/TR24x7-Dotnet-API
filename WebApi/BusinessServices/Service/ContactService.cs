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
                    Mobile = contactEntity.MobileNumber,
                    Rating = contactEntity.Rating,
                    DisplayOrder = contactEntity.DisplayOrder,
                    Rank = contactEntity.Rank,
                    CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    UserTypeFID = contactEntity.ContactTypeId,
                    IsActive = true,
                    VehicleLength = contactEntity.VehicleLength,
                    City = contactEntity.City
                };

                if (!string.IsNullOrEmpty(contactEntity.OtherNumbers))
                {
                    contact.OtherNumbers = contactEntity.OtherNumbers;
                }

                if (!string.IsNullOrEmpty(contactEntity.Comments))
                {
                    contact.Comments = contactEntity.Comments;
                }

                if (!string.IsNullOrEmpty(contactEntity.OtherRoadlines))
                {
                    contact.OtherRoadlines = contactEntity.OtherRoadlines;
                }

                if (!string.IsNullOrEmpty(contactEntity.MainRoadlines))
                {
                    contact.MainRoadlines = contactEntity.MainRoadlines;
                }

                _unitOfWork.ContactRepository.Insert(contact);


                // _unitOfWork.Save();

                if (!string.IsNullOrEmpty(contactEntity.OtherRoadlines))
                {
                    foreach (var item in contactEntity.OtherRoadlines.Split(','))
                    {
                        var roadline = new RoadLine
                        {
                            Name = item,
                            DisplayOrder = contactEntity.DisplayOrder,
                            Rank = contactEntity.Rank,
                            IsActive = true
                        };

                        var roadLine = _unitOfWork.RoadLineRepository.Get(x => x.Name.Equals(item, StringComparison.InvariantCultureIgnoreCase));
                        if(roadLine==null)
                        _unitOfWork.RoadLineRepository.Insert(roadline);

                        var contactRoadline = new Contact_RoadLine_Mapping
                        {
                            ContactFID = contact.ContactPID,
                            RoadLineFID = roadLine !=null ? roadLine.RoadLinePID: roadline.RoadLinePID
                        };
                        _unitOfWork.ContactRoadLineMappingRepository.Insert(contactRoadline);

                        _unitOfWork.Save();
                    }
                }

                foreach (var item in contactEntity.Roadlines)
                {
                    var contactRoadline = new Contact_RoadLine_Mapping
                    {
                        ContactFID = contact.ContactPID
                    };
                    if(item !=0)
                    {
                        contactRoadline.RoadLineFID = item;
                    }
                    _unitOfWork.ContactRoadLineMappingRepository.Insert(contactRoadline);
                    _unitOfWork.Save();
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
                       var contact = _unitOfWork.ContactRepository.GetByID(contactId);
                        if(contact != null)
                        _unitOfWork.ContactRepository.Delete(contact);
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

                foreach (var item in goods)
                {
                    ContactResponseEntity contactResponseEntity = new ContactResponseEntity
                    {
                        ContactId = Convert.ToInt32(item.ContactPID),
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Rating = Convert.ToInt32(item.Rating),
                        VehicleLength =item.VehicleLength,
                        ContactType = item.UserType.Type,
                        City =item.City,
                        Comments = item.Comments,
                        OtherNumbers = item.OtherNumbers,
                        MainRoadlines = item.MainRoadlines,
                        OtherRoadlines =item.OtherRoadlines
                    };

                    contactResponseEntity.RoadLines = new List<RoadLineResponseEntity>();

                    foreach (var rd in item.Contact_RoadLine_Mapping)
                    {
                        if (rd.RoadLine != null)
                        {
                            contactResponseEntity.Roadlines = contactResponseEntity.Roadlines + rd.RoadLine.Name + ",";
                            RoadLineResponseEntity roadLineResponseEntity = new RoadLineResponseEntity
                            {
                                Id = rd.RoadLine.RoadLinePID,
                                Name = rd.RoadLine.Name
                            };
                            contactResponseEntity.RoadLines.Add(roadLineResponseEntity);
                        }
                    }

                    listGoodsEntity.Add(contactResponseEntity);
                }
                
            }
            return listGoodsEntity;
        }

        public List<ContactResponseEntity> GetAllContacts()
        {
            var contacts = _unitOfWork.ContactRepository.GetAll().ToList();
            if (contacts.Any())
            {
                return MapGoods(contacts);
            }
            return null;
        }

        public ContactResponseEntity GetContact(int contactId)
        {
            var contact = _unitOfWork.ContactRepository.GetByID(contactId);
            if (contact != null)
            {
                ContactResponseEntity contactResponseEntity = new ContactResponseEntity
                {
                    ContactId = Convert.ToInt32(contact.ContactPID),
                    Name = contact.Name,
                    Mobile = contact.Mobile,
                    Rating = Convert.ToInt32(contact.Rating),
                    ContactType =Convert.ToString(contact.UserType.UserTypePID),
                    City = contact.City,
                    Comments = contact.Comments,
                    OtherNumbers = contact.OtherNumbers,
                    OtherRoadlines = contact.OtherRoadlines
                };

                contactResponseEntity.RoadLines = new List<RoadLineResponseEntity>();

                foreach (var rd in contact.Contact_RoadLine_Mapping)
                {
                    if (rd.RoadLine != null)
                    {
                        contactResponseEntity.Roadlines = contactResponseEntity.Roadlines + rd.RoadLine.Name + ",";
                        RoadLineResponseEntity roadLineResponseEntity = new RoadLineResponseEntity
                        {
                            Id = rd.RoadLine.RoadLinePID,
                            Name = rd.RoadLine.Name
                        };
                        contactResponseEntity.RoadLines.Add(roadLineResponseEntity);
                    }
                }
                return contactResponseEntity;
            }
            return null;
        }
    }
}
