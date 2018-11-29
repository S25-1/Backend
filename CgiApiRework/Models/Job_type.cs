using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgiApiRework.Models
{
    public class Job_Type
    {
        public int Job_typeID { get; set; }
        public string Job_name { get; set; }

        [JsonConstructor]
        public Job_Type(int job_typeID, string job_name)
        {
            Job_typeID = job_typeID;
            Job_name = job_name;
        }
        public Job_Type(int job_typeID)
        {
            Job_typeID = job_typeID;
            Job_name = "null";
        }
    }
}