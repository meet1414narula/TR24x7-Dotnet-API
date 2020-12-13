using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class GoodsResponseEntity
    {
        public int GoodsId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int Freight { get; set; }
        public string CreationDate { get; set; }
        public string ValidTill { get; set; }
        public string VehicleLength { get; set; }
        public string Status { get; set; }
        public string VehicleType { get; set; }
        public string MaterialType { get; set; }
        public string ImgVehicleType { get; set; }
        public int TotalBidsCount { get; set; }
        public int CurrentBidPrice { get; set; }
        public long UserId { get; set; }
    }

    public class EnquiryResponseEntity
    {
        public int EnquiryId { get; set; }
        public string Comments { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int Freight { get; set; }
        public string CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime ValidTill { get; set; }
        public string VehicleLength { get; set; }
        public string Status { get; set; }
        public string VehicleType { get; set; }
        public string MaterialType { get; set; }
        public string ImgVehicleType { get; set; }
        public int TotalBidsCount { get; set; }
        public int CurrentBidPrice { get; set; }
        public long UserId { get; set; }

        public string LoadingCharges { get; set; }

        public string UnloadingCharges { get; set; }

        public string PackagingCharges { get; set; }
    }

    public class QuotationResponseEntity:EnquiryResponseEntity
    {
        public string TotalCharges { get; set; }

        public string Comments { get; set; }
        //public string LoadingCharges { get; set; }

        //public string UnloadingCharges { get; set; }

        //public string PackagingCharges { get; set; }
    }

    public class BookingResponseEntity : EnquiryResponseEntity
    {
        public string TotalCharges { get; set; }

        public string Advance { get; set; }

        public string Comments { get; set; }
    }

    public class BookedVehicleResponseEntity : VehicleRequestEntity
    {
        
    }


}
