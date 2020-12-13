using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnquiryService _enquiryService;

        public HomeController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;
        }
        public ActionResult Index(Query query)
        {
            if(query != null && !string.IsNullOrEmpty(query.Mobile))
            _enquiryService.CreateEnquiry(new BusinessEntities.EnquiryRequestEntity {From="CheckSite",To= "CheckSite",MaxWeight=1,VehicleLength="Any", Comments=query.Message,MobileNumber=query.Mobile,VehicleType=1,MaterialType=1,UserId=1,ValidTill=DateTime.Now.AddDays(5),Status="OPEN" });
            return View();
        }

        public class Query
        {
            public string Mobile { get; set; }
            public string Message { get; set; }

        }
    }
}
