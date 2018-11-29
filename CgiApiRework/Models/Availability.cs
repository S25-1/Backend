using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgiApiRework.Models
{
    public class Availability
    { 
        public int AvailabilityID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Availability(int availabilityID, DateTime startDate, DateTime endDate)
        {
            AvailabilityID = availabilityID;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}