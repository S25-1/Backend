using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CgiApiRework.SelectFitUsers
{
    public class User
    {
        public int UserId { get; set; }

        public User(int userId)
        {
            UserId = userId;
        }
    }
}
