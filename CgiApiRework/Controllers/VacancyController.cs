using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CgiApiRework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CgiApiRework.Controllers
{
    public class VacancyController : ControllerBase
    {
        [HttpGet]
        //[Authorize]
        public ArrayList DefaultAction()
        {
            return Vacancy.GetListVacancy();
        }

        [Route("api/vacancy/{id}")]
        [HttpGet]
        public ArrayList GetVacancyList(int id)
        {
            return Vacancy.GetListVacancy(id);
        }

        [Route("api/vacancy/responses")]
        [HttpGet]
        public ArrayList GetListRespondVacancyUser()
        {
            return Vacancy.GetListRespondVacancyUser();
        }

        // Krijgt een lijst van gereageerde werknemers met het aangegeven VacancyID en StatusID
        [Route("api/vacancy/GetRespondVacancyUser")]
        [HttpGet]
        public ArrayList GetRespondVacancyUserList(int userID, int vacancyID, int statusID)
        {
            Employer employer = new Employer();
            employer = Employer.GetEmployer(userID);
            return Vacancy.GetListRespondVacancyUser(employer.UserID, vacancyID, statusID);
        }

        [Route("api/vacancy/{id}/responses")]
        [HttpGet]
        public ArrayList GetRespondVacancyUserList(int id)
        {
            return Vacancy.GetListRespondVacancyUser(id);
        }

        //Voegt een vacature in de database met de class Vacancy
        [Route("api/vacancy/add")]
        [HttpPost]
        public HttpResponseMessage AddVacancy([FromBody]Vacancy vacancy)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            if (vacancy != null)
            {
                message.Content = new StringContent(Vacancy.AddVacancy(vacancy));
            }
            else
            {
                message.Content = new StringContent("object is null, please check parameters");
                message.ReasonPhrase = "check the json format or the right parameters";
            }

            return message;
        }

        [Route("api/vacancy/addResponse")]
        [HttpPost]
        public HttpResponseMessage AddRespondVacancyUser([FromBody]RespondVacancyUser user)
        {
            if (Vacancy.AddRespondVacancyUser(user))
            {
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }
        }


        [Route("api/vacancy/response/update")]
        [HttpPut]
        public void Put([FromBody]RespondVacancyUser user)
        {
            Vacancy.UpdateRespondVacancyUser(user);
        }

        [Route("api/vacancy/updatelist")]
        [HttpPut]
        public void Put([FromBody]List<RespondVacancyUser> userList)
        {
            Vacancy.UpdateRespondVacancyUser(userList);
        }
    }
}
