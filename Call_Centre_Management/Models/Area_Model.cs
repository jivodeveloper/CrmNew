using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Call_Centre_Management.Models
{
    public class Area_Model
    {
        public int id { get; set; }
        public int AreaId { get; set; }
        public int ZoneId { get; set; }
        public int StateId { get; set; }
        public string AreaName { get; set; }
        public string ZoneName { get; set; }
        public string StateName{ get; set; }
        public int pincode { get; set; }
    }
    public class Area
    {
        public int id { get; set; }
        public string Area_Name { get; set; }
        public string Zone_id { get; set;} 
        public string State_id { get; set; }
        public int pincode { get; set; }
        public bool active { get; set; }


    }
}