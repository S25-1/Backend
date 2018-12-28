using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgiApiRework.Models
{
    public class RespondVacancyUser
    {
        public string UserID { get; set; }
        public int VacancyID { get; set; }
        public int StatusID { get; set; }

        public RespondVacancyUser(string userID, int vacancyID)
        {
            UserID = userID;
            VacancyID = vacancyID;
            StatusID = 1;
        }

        [JsonConstructor]
        public RespondVacancyUser(string userID, int vacancyID, int statusID)
        {
            UserID = userID;
            VacancyID = vacancyID;
            StatusID = statusID;
        }
    }
}