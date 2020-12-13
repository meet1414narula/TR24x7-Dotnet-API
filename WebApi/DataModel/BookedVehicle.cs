//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookedVehicle
    {
        public int BookedVehiclePID { get; set; }
        public Nullable<int> Freight { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<System.DateTime> DateOfBooking { get; set; }
        public Nullable<System.DateTime> DateOfMoving { get; set; }
        public Nullable<System.DateTime> DateOfReaching { get; set; }
        public string VehicleLength { get; set; }
        public string GRNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string TransporterName { get; set; }
        public string TransporterNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverNumber { get; set; }
        public string OwnerName { get; set; }
        public string OwnerNumber { get; set; }
        public string Roadlines { get; set; }
        public string VehiclePayment { get; set; }
        public string LabourPayment { get; set; }
        public string OtherPayment { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> ALLIndia { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string VehicleType { get; set; }
        public Nullable<int> VehicleTypeFID { get; set; }
        public Nullable<int> UserFID { get; set; }
        public Nullable<int> EnquiryId { get; set; }
        public Nullable<int> BookingFID { get; set; }
    }
}
