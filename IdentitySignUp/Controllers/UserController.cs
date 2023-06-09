﻿using IdentitySignUp.Entities;
using IdentitySignUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentitySignUp.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserLog userLog)
        {
            var user = new User()
            {
                UserName = userLog.UserName,
                Email = userLog.Email,
            };

            var result = await _userManager.CreateAsync(user, userLog.Password);


            if (userLog.IsAdmin == true)
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("admin"));
                await _userManager.AddToRoleAsync(user, "admin");
            }


            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction("Profile");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = HttpContext.User;

            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = user.FindFirstValue(ClaimTypes.Name);
            var email = user.FindFirstValue(ClaimTypes.Email);

            return Ok(id + "        "  + name + email);
           // return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSign userSign)
        {
            //signinuser klass ochib uni userdan inhert qilib koraman, va userga tenglashtirib koraman
            //User user = userSign;
            
            var result = await _signInManager.PasswordSignInAsync(userName: userSign.UserName,
                password: userSign.PasswordHash,
                isPersistent: true, false);

            if (!result.Succeeded)
            {
                return BadRequest(result.IsNotAllowed);
            }

            return RedirectToAction("Profile");
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddQuestion()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddQuestion(Question question)
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckNumber()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckNumber(int checkNum, int endNum)
        {
            int count = 0;
            for(int i = 0; i <= endNum; i++)
            {
                int a = i;

                while (a > 0)
                {
                    if (a % 10 == checkNum)
                    {
                        count++;
                    }
                    a /= 10;
                }
            }
            ViewBag.Count = count;

            return View();
        }
    }
}
