using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplyAds.Models
{
    public class SearchedAdViewModel
    {
        public string Name { get; set; }
        public string RefNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Duration { get; set; }
        public string Amount { get; set; }
        public bool Urgent { get; set; }
        public bool Treated { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Cars { get; set; }
    }
}