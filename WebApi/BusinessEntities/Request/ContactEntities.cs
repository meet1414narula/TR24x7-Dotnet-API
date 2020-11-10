using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class ContactRequestEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int Rating { get; set; }
        public int DisplayOrder { get; set; }
        public int Rank { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }

        public List<int> RoadLines { get; set; }

        public string OtherRoadLines { get; set; }
    }
}
