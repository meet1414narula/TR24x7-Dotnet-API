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
    public class BidController : ApiController
    {
        #region Private variable.

        private readonly IBidService _bidServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize bid service instance
        /// </summary>
        public BidController(IBidService bidServices)
        {
            _bidServices = bidServices;
        }

        #endregion

        // GET api/bid/GetAllBid
        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllGoodsBid")]
        public HttpResponseMessage GetAllGoodsBid(int goodsId)
        {
            var bidEntities = _bidServices.GetAllGoodsBid(goodsId);
            if (bidEntities !=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bidEntities);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // GET api/bid/GetAllBid
        [HttpGet]
        [HttpOptions]
        [ActionName("GetAllVehicleBid")]
        public HttpResponseMessage GetAllVehicleBid(int vehicleId)
        {
            var bidEntities = _bidServices.GetAllVehicleBid(vehicleId);
            if (bidEntities != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bidEntities);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
       
        // POST api/AddBid
        [HttpPost]
        [HttpOptions]
        public HttpResponseMessage AddBid(BidRequestEntity bidEntity)
        {
            if(bidEntity == null)
            {
               return  Request.CreateResponse(HttpStatusCode.OK);
            }
            var bidId= _bidServices.AddBid(bidEntity);
            var response = Request.CreateResponse(HttpStatusCode.OK,bidId);
            return response;
        }
      
    }
}
