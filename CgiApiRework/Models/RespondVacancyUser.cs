using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgiApiRework.Models
{
    public class RespondVacancyUser
    {

        public int VacancyID { get; set; }
        public string VacancyName { get; set; }
        public string VacancyDescription { get; set; }
        public string VacancyJobName { get; set; }
        public DateTime VacancyDateBegin { get; set; }
        public DateTime VacancyDateEnd { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public int UserStatusID { get; set; }
        public string UserStatusName { get; set; }

        [JsonConstructor]
        public RespondVacancyUser(int vacancyID, string vacancyName, string vacancyDescription, string vacancyJobName, 
             string userID, string userName, int userStatusID, string userStatusName, string userPhoneNumber, string userEmail, DateTime vacancyDateBegin, DateTime vacancyDateEnd)
        {
            VacancyID = vacancyID;
            VacancyName = vacancyName;
            VacancyDescription = vacancyDescription;
            VacancyJobName = vacancyJobName;
            VacancyDateBegin = vacancyDateBegin;
            VacancyDateEnd = vacancyDateEnd;
            UserID = userID;
            UserName = userName;
            UserPhoneNumber = userPhoneNumber;
            UserEmail = userEmail;
            UserStatusID = userStatusID;
            UserStatusName = userStatusName;
        }

        public RespondVacancyUser(int vacancyID, string userID, int userStatusID)
        {
            VacancyID = vacancyID;
            UserID = userID;
            UserStatusID = userStatusID;
        }
    }

}