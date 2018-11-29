using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CgiApiRework.Models;
using Newtonsoft.Json;
using CgiApiRework.Models;
using Microsoft.AspNetCore.Mvc;

namespace cgiAPI.Controllers
{
    public class UserController : ControllerBase
    {
        [Route("api/employee/add")]
        [HttpPost]
        public void AddUser([FromBody]User user)
        {
            CgiApiRework.Models.User.AddUser(user);
        }
    }
}
