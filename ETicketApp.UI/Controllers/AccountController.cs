using ETicketApp.Core.Models;
using ETicketApp.Data;
using ETicketApp.UI.Static;
using ETicketApp.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ETicketApp.UI.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            return View(users);
        }
        public async Task<IActionResult> Admins()
        {
            var loginedAdmin = await _userManager.GetUserAsync(User);
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            admins.Remove(loginedAdmin);
            return View(admins);
        }

        [AllowAnonymous]
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }

        [AllowAnonymous]
        public IActionResult UserRegister() => View(new RegisterVM());

        //[AllowAnonymous]
        public IActionResult AdminRegister() => View(new RegisterVM());

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                bool userRoleExists = await _roleManager.RoleExistsAsync("User");
                if (!userRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return View("RegisterCompleted");
            }

            else
            {
                TempData["Error"] = "The password must be at least 6 and at max 100 characters long. It must contain uppercase, lowercase and special character.";
                return View();
            }



        }

        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> AdminRegister(RegisterVM registerVM)
        {

            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {

                bool adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!adminRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                return View("RegisterCompleted");
            }
            else
            {
                TempData["Error"] = "The password must be at least 6 and at max 100 characters long. It must contain uppercase, lowercase and special character.";
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        public async Task<IActionResult> Delete(string Id)
        {
            await _userManager.DeleteAsync(_userManager.FindByIdAsync(Id).Result);
            return RedirectToAction("Index", "Movies");
        }
    }
}
