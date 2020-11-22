#region Using Namespaces...

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Validation;
using DataModel.GenericRepository;

#endregion

namespace DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork: IDisposable, IUnitOfWork
    {
        #region Private member variables...

        private readonly DBEntities _context = null;

        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<RoadLine> _roadLineRepository;
        private GenericRepository<Contact_RoadLine_Mapping> _contactRoadLineMappingRepository;
        private GenericRepository<OTP> _otpRepository;
        private GenericRepository<Good> _goodsRepository;
        private GenericRepository<Enquiry> _enquiryRepository;
        private GenericRepository<EnquiryStatu> _enquiryStatusRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<UserAccess> _userAccessRepository;
        private GenericRepository<UserType> _userTypeRepository;
        private GenericRepository<Token> _tokenRepository;
        private GenericRepository<Error> _errorRepository;
        private GenericRepository<City> _cityRepository;
        private GenericRepository<MaterialType> _materialTypeRepository;
        private GenericRepository<VehicleType> _vehicleTypeRepository;
        private GenericRepository<ConfigKeyValue> _configKeyValueRepository;
        private GenericRepository<Bid> _bidRepository;

        private GenericRepository<Quotation> _quotationRepository;
        private GenericRepository<Booking> _bookingRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new DBEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for otp repository.
        /// </summary>
        public GenericRepository<Contact> ContactRepository
        {
            get
            {
                if (this._contactRepository == null)
                    this._contactRepository = new GenericRepository<Contact>(_context);
                return _contactRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<RoadLine> RoadLineRepository
        {
            get
            {
                if (this._roadLineRepository == null)
                    this._roadLineRepository = new GenericRepository<RoadLine>(_context);
                return _roadLineRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<Contact_RoadLine_Mapping> ContactRoadLineMappingRepository
        {
            get
            {
                if (this._contactRoadLineMappingRepository == null)
                    this._contactRoadLineMappingRepository = new GenericRepository<Contact_RoadLine_Mapping>(_context);
                return _contactRoadLineMappingRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for otp repository.
        /// </summary>
        public GenericRepository<OTP> OTPRepository
        {
            get
            {
                if (this._otpRepository == null)
                    this._otpRepository = new GenericRepository<OTP>(_context);
                return _otpRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<Good> GoodsRepository
        {
            get
            {
                if (this._goodsRepository == null)
                    this._goodsRepository = new GenericRepository<Good>(_context);
                return _goodsRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<Enquiry> EnquiryRepository
        {
            get
            {
                if (this._enquiryRepository == null)
                    this._enquiryRepository = new GenericRepository<Enquiry>(_context);
                return _enquiryRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<UserType> UserTypeRepository
        {
            get
            {
                if (this._userTypeRepository  == null)
                    this._userTypeRepository = new GenericRepository<UserType>(_context);
                return _userTypeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Token> TokenRepository
        {
            get
            {
                if (this._tokenRepository == null)
                    this._tokenRepository = new GenericRepository<Token>(_context);
                return _tokenRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for error repository.
        /// </summary>
        public GenericRepository<Error> ErrorRepository
        {
            get
            {
                if (this._errorRepository == null)
                    this._errorRepository = new GenericRepository<Error>(_context);
                return _errorRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<City> CityRepository
        {
            get
            {
                if (this._cityRepository == null)
                    this._cityRepository = new GenericRepository<City>(_context);
                return _cityRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<MaterialType> MaterialTypeRepository
        {
            get
            {
                if (this._materialTypeRepository == null)
                    this._materialTypeRepository = new GenericRepository<MaterialType>(_context);
                return _materialTypeRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<VehicleType> VehicleTypeRepository
        {
            get
            {
                if (this._vehicleTypeRepository == null)
                    this._vehicleTypeRepository = new GenericRepository<VehicleType>(_context);
                return _vehicleTypeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<EnquiryStatu> EnquiryStatusRepository
        {
            get
            {
                if (this._enquiryStatusRepository == null)
                    this._enquiryStatusRepository = new GenericRepository<EnquiryStatu>(_context);
                return _enquiryStatusRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<ConfigKeyValue> ConfigKeyValueRepository
        {
            get
            {
                if (this._configKeyValueRepository == null)
                    this._configKeyValueRepository = new GenericRepository<ConfigKeyValue>(_context);
                return _configKeyValueRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Bid repository.
        /// </summary>
        public GenericRepository<Bid> BidRepository
        {
            get
            {
                if (this._bidRepository == null)
                    this._bidRepository = new GenericRepository<Bid>(_context);
                return _bidRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<Quotation> QuotationRepository
        {
            get
            {
                if (this._quotationRepository == null)
                    this._quotationRepository = new GenericRepository<Quotation>(_context);
                return _quotationRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<Booking> BookingRepository
        {
            get
            {
                if (this._bookingRepository == null)
                    this._bookingRepository = new GenericRepository<Booking>(_context);
                return _bookingRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<UserAccess> UserAccessRepository
        {
            get
            {
                if (this._userAccessRepository == null)
                    this._userAccessRepository = new GenericRepository<UserAccess>(_context);
                return _userAccessRepository;
            }
        }

        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false; 
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion
    }
}