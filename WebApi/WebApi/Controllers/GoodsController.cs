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
    [RoutePrefix("api/Goods")]
    public class GoodsController : ApiController
    {
        #region Private variable.

        private readonly IGoodsService _goodsServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize goods service instance
        /// </summary>
        public GoodsController(IGoodsService goodsServices)
        {
            _goodsServices = goodsServices;
        }

        #endregion

        // GET api/goods/GetAllGoods
        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllGoods")]
        public HttpResponseMessage GetAllGoods()
        {
            var goodsEntities = _goodsServices.GetAllGoods();
            if (goodsEntities !=null)
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
        [ActionName("GetGoods")]
        public HttpResponseMessage GetGoods([FromUri] int goodsId)
        {
            var goodsEntity = _goodsServices.GetGoods(goodsId);
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
        [ActionName("AddGoods")]
        public HttpResponseMessage AddGoods([FromBody] GoodsRequestEntity goodsEntity)
        {
            if(goodsEntity == null)
            {
               return  Request.CreateResponse(HttpStatusCode.OK);
            }

            goodsEntity.UserId = GetUserId();

            var goodsId= _goodsServices.CreateGoods(goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, goodsId);
            return response;
        }


        // POST api/AddGoods
        [HttpPost]
        [HttpOptions]
        public HttpResponseMessage UpdateGoods([FromUri]int goodsId, GoodsRequestEntity goodsEntity)
        {
            if (goodsEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            goodsEntity.UserId = GetUserId();
            var success= _goodsServices.UpdateGoods(goodsId,goodsEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
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
