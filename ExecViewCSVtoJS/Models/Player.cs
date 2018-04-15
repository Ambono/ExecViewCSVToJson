using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExecViewCSVtoJS.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Position { get; set; }
        public string Number { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string University { get; set; }
        public Double PPG { get; set; }
    }
}