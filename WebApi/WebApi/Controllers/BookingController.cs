using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Booking")]
    public class BookingController : ApiController
    {
        #region Private variable.

        private readonly IBookingService _goodsServices;
        private int userId;
        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize goods service instance
        /// </summary>
        public BookingController(IBookingService goodsServices)
        {
            _goodsServices = goodsServices;
        }

        #endregion

        // GET api/goods/GetAllGoods
        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllBookings")]
        public HttpResponseMessage GetAllBookings()
        {
            userId = GetUserId();
            var enquiryEntities =  _goodsServices.GetAllBookings(userId);
            if (enquiryEntities !=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, enquiryEntities);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
           // throw new Exception("Goods not found");
        }

        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllBookedVehicles")]
        public HttpResponseMessage GetAllBookedVehicles()
        {
            userId = GetUserId();
            var enquiryEntities = _goodsServices.GetAllBookedVehicles(userId);
            if (enquiryEntities != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, enquiryEntities);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // GET api/goods/GetAllGoods
        [HttpGet]
        [HttpOptions]
        [ActionName("GetGoodsByUser")]
        public HttpResponseMessage GetGoodsByUser([FromUri]UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            userEntity.UserId = GetUserId();
            var goodsEntities = _goodsServices.GetBookingsByUser(userEntity);
            if (goodsEntities != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, goodsEntities);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            // throw new Exception("Goods not found");
        }

        // GET api/goods/GetAllGoods
        [HttpGet]
        [HttpOptions]
        [ActionName("GetEnquiry")]
        public HttpResponseMessage GetEnquiry([FromUri] int enquiryId)
        {
            var goodsEntity = _goodsServices.GetBooking(enquiryId);
            if (goodsEntity != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, goodsEntity); 
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            // throw new Exception("Goods not found");
        }

        // POST api/AddGoods
        [HttpPost]
        [HttpOptions]
        [ActionName("AddBooking")]
        public HttpResponseMessage AddQuotation([FromUri]int enquiryId, BookingRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            goodsEntity.UserId = GetUserId();
            goodsEntity.EnquiryId = enquiryId;
            var success = _goodsServices.CreateBooking(goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        [HttpPost]
        [HttpOptions]
        [ActionName("AddBookedVehicle")]
        public HttpResponseMessage AddBookedVehicle([FromUri]int bookingId, VehicleRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            goodsEntity.EnquiryId = bookingId;
            var success = _goodsServices.AddVehicle(goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }


        // POST api/AddGoods
        [HttpPost]
        [HttpOptions]
        public HttpResponseMessage UpdateBooking([FromUri]int enquiryId, BookingRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            goodsEntity.UserId = GetUserId();
            var success= _goodsServices.UpdateBooking(enquiryId,goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [HttpOptions]
        [ActionName("DeleteBooking")]
        public HttpResponseMessage DeleteBooking([FromUri]int enquiryId)
        {
            if (enquiryId == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
           // goodsEntity.UserId = GetUserId();
            var success = _goodsServices.DeleteBooking(enquiryId);
            var enquiryEntities = _goodsServices.GetAllBookings(GetUserId());
            if (enquiryEntities != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, enquiryEntities);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
           // var response = Request.CreateResponse(HttpStatusCode.OK, success);
            //return response;
        }

        private int GetUserId()
        {
            if (Request.Headers.Contains("UserId"))
            {
                return Convert.ToInt32(Request.Headers.GetValues("UserId").First());
            }
            return 0;
        }
        //private ResponseEntity GetResponse(object response)
        //{
        //    var responseEntity = new ResponseEntity { IsSuccess=true,SuccessMessage= };
        //    return responseEntity;
        //}
    }
}
