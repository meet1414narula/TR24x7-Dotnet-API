using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class GoodsRequestEntity
    {
      //  public int GoodsId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public string MobileNumber { get; set; }
        public int OTP { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int Freight { get; set; }

        public string LoadingCharges { get; set; }

        public string UnloadingCharges { get; set; }

        public string PackagingCharges { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime ValidTill { get; set; }
        public string VehicleLength { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public int VehicleType { get; set; }
        public int MaterialType { get; set; }
        public long UserId { get; set; }
    }

    public class EnquiryRequestEntity
    {
        public string Comments { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public string MobileNumber { get; set; }
        public int OTP { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int Freight { get; set; }

        public string LoadingCharges { get; set; }

        public string UnloadingCharges { get; set; }

        public string PackagingCharges { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime ValidTill { get; set; }
        public string VehicleLength { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public int VehicleType { get; set; }
        public int MaterialType { get; set; }
        public long UserId { get; set; }
    }

    public class QuoteRequestEntity
    {
       
        public string From { get; set; }
        public string To { get; set; }
        public string VehicleLength { get; set; }
        public int MaterialType { get; set; }

        public int MaxWeight { get; set; }

    }
    public class QuotationRequestEntity
    {
        public int EnquiryId { get; set; }
        public int Freight { get; set; }

        public string LoadingCharges { get; set; }

        public string UnloadingCharges { get; set; }

        public string PackagingCharges { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime ValidTill { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
      
        public long UserId { get; set; }
    }
}
