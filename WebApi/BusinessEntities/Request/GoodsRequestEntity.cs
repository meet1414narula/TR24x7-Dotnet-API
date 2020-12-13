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
        public int UserId { get; set; }
        public int AssignedToUserId { get; set; }
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

    public class BookingRequestEntity:EnquiryRequestEntity
    {
        public int EnquiryId { get; set; }
        public int Advance { get; set; }
    }


    public class VehicleRequestEntity
    {
        public int? UserId { get; set; }
        public int BookingId { get; set; }
        public int EnquiryId { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DateOfBooking { get; set; }
        public DateTime DateOfMoving { get; set; }
        public DateTime DateOfReaching { get; set; }

        public string GRNumber { get; set; }

        public string VehicleLength { get; set; }
        public string VehicleNumber { get; set; }
        public string Roadlines { get; set; }
        public string TransporterName { get; set; }
        public string TransporterNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverNumber { get; set; }
        public string DriverCity { get; set; }
        public string OwnerName { get; set; }
        public string OwnerNumber { get; set; }
        public string VehiclePayment { get; set; }
        public string LabourPayment { get; set; }
        public string VehicleType { get; set; }
        public string OtherPayment { get; set; }

        public string BookingAmount { get; set; }

        public int Rating { get; set; }
        public bool IsActive { get; set; }

        public bool ALLIndia { get; set; }
        public string Comments { get; set; }

    }
}
