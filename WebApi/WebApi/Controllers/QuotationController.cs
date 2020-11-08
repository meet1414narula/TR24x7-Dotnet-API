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
    [RoutePrefix("api/Quotation")]
    public class QuotationController : ApiController
    {
        #region Private variable.

        private readonly IQuotationService _goodsServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize goods service instance
        /// </summary>
        public QuotationController(IQuotationService goodsServices)
        {
            _goodsServices = goodsServices;
        }

        #endregion

        // GET api/goods/GetAllGoods
        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllQuotations")]
        public HttpResponseMessage GetAllQuotations()
        {
            var enquiryEntities = _goodsServices.GetAllEnquiries();
            if (enquiryEntities != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, enquiryEntities);
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
        [ActionName("GetGoodsByUser")]
        public HttpResponseMessage GetGoodsByUser([FromUri]UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            userEntity.UserId = GetUserId();
            var goodsEntities = _goodsServices.GetGoodsByUser(userEntity);
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
            var goodsEntity = _goodsServices.GetGoods(enquiryId);
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
        [ActionName("AddQuotation")]
        public HttpResponseMessage AddQuotation([FromUri]int enquiryId, QuotationRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            goodsEntity.UserId = GetUserId();
            goodsEntity.EnquiryId = enquiryId;
            var success = _goodsServices.CreateQuotation(goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }


        // POST api/AddGoods
        [HttpPost]
        [HttpOptions]
        public HttpResponseMessage UpdateEnquiry([FromUri]int enquiryId, EnquiryRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            goodsEntity.UserId = GetUserId();
            var success = _goodsServices.UpdateGoods(enquiryId, goodsEntity);
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
        [ActionName("DeleteEnquiry")]
        public HttpResponseMessage DeleteEnquiry([FromUri]int enquiryId)
        {
            if (enquiryId == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            // goodsEntity.UserId = GetUserId();
            var success = _goodsServices.DeleteGoods(enquiryId);
            var enquiryEntities = _goodsServices.GetAllEnquiries();
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
