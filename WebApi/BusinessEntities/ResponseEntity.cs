using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public class ResponseEntity
    {
        public bool IsSuccess { get; set; }
        public List<string> Error { get; set; }
        public string SuccessMessage { get; set; }
        public object Response { get; set; }
    }
}
