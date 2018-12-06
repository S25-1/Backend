using System;
using System.Threading.Tasks;
using CgiApiRework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace cgiAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [Route("api/employee/add")]
        [HttpPost]
        public async Task<object> AddUser([FromBody]User user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Email,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            if (result.Succeeded)
            {
                // Add user to Identity Database
                await _signInManager.SignInAsync(identityUser, false);

                // Hacky way too quickly add employee role
                //IdentityRole role = new IdentityRole();
                //role.Name = "employee";
                //await _roleManager.CreateAsync(role);

                await _userManager.AddToRoleAsync(identityUser, "employee");
                var id = await _userManager.GetUserIdAsync(identityUser);

                await _signInManager.SignOutAsync();

                user.UserID = id;

                CgiApiRework.Models.User.AddUser(user);
            }
            else
            {
                throw new Exception("FAILED TO ADD EMPLOYEE");
            }

            return result.Succeeded;
        }
    }
}
