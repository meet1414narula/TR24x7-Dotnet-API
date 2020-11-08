using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class BidRequestEntity
    {
        public int GoodsId { get; set; }
        public int VehicleId { get; set; }
        public long UserId { get; set; }
        public int Price { get; set; }
    }
}
