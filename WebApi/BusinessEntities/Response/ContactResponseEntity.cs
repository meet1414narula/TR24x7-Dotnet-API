﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class ContactResponseEntity
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int Rating { get; set; }

        public string ContactType { get; set; }

        public string VehicleLength { get; set; }

        public string OtherNumbers { get; set; }

        public string City { get; set; }

        public string Comments { get; set; }

        public string OtherRoadlines { get; set; }
        public string MainRoadlines { get; set; }

        public List<RoadLineResponseEntity> RoadLines { get; set; }

        public string Roadlines { get; set; }
    }

    public class RoadLineResponseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayOrder { get; set; }
        public string Rank { get; set; }
    }

}
