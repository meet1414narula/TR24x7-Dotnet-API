using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class ContactRequestEntity
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int DisplayOrder { get; set; }
        public int Rank { get; set; }
        public int ContactTypeId { get; set; }
        public bool IsActive { get; set; }

        public List<int> Roadlines { get; set; }

        public string OtherRoadlines { get; set; }
        public string MainRoadlines { get; set; }

        public string Comments { get; set; }

        public string OtherNumbers { get; set; }

        public string VehicleLength { get; set; }
    }
}
