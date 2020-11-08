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
    
    public partial class Quotation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quotation()
        {
            this.Bookings = new HashSet<Booking>();
        }
    
        public long QuotationPID { get; set; }
        public Nullable<int> Freight { get; set; }
        public string LoadingCharges { get; set; }
        public string UnloadingCharges { get; set; }
        public string PackagingCharges { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> EnquiryFID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Enquiry Enquiry { get; set; }
    }
}
