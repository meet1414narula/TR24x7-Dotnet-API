using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class BidResponseEntity
    {
        public long? GoodsId { get; set; }
        public long? VehicleId { get; set; }
        public int Price { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public int Rating { get; set; }
        public int TotalYearOfExperiance { get; set; }
    }


    //public class BidEntity
    //{
    //    public int GoodsId { get; set; }
    //    public int Price { get; set; }
    //    public long UserId { get; set; }
    //    public string Username { get; set; }
    //}
}
