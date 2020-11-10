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
    [RoutePrefix("api/Contact")]
    public class ContactController : ApiController
    {
        #region Private variable.

        private readonly IContactService _goodsServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize goods service instance
        /// </summary>
        public ContactController(IContactService goodsServices)
        {
            _goodsServices = goodsServices;
        }

        #endregion

        // GET api/goods/GetAllGoods
        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllContacts")]
        public HttpResponseMessage GetAllContacts(List<int> roadLines)
        {
            var enquiryEntities = _goodsServices.GetAllContacts(roadLines);
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

        // POST api/AddGoods
        [HttpPost]
        [HttpOptions]
        [ActionName("AddContact")]
        public HttpResponseMessage AddContact([FromBody] ContactRequestEntity goodsEntity)
        {
            if(goodsEntity == null)
            {
               return  Request.CreateResponse(HttpStatusCode.OK);
            }
            var goodsId= _goodsServices.CreateContact(goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, goodsId);
            return response;
        }


        // POST api/AddGoods
        [HttpPost]
        [HttpOptions]
        public HttpResponseMessage UpdateContact([FromUri]int enquiryId, ContactRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            var success= _goodsServices.UpdateContact(enquiryId,goodsEntity);
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
        [ActionName("DeleteContact")]
        public HttpResponseMessage DeleteContact([FromUri]int enquiryId)
        {
            if (enquiryId == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
           // goodsEntity.UserId = GetUserId();
            var success = _goodsServices.DeleteContact(enquiryId);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        private int GetUserId()
        {
            if (Request.Headers.Contains("UserId"))
            {
                return Convert.ToInt32(Request.Headers.GetValues("UserId").First());
            }
            return 0;
        }
    }
}
