using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplyAds.Misc
{
    public class PayStackClass
    {
        public string @event { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}