using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserTypeRepository _userTypeRepository;

        public AccountController(IUserProfileRepository userProfileRepository,
                                    IUserTypeRepository userTypeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _userTypeRepository = userTypeRepository;
        }

        // GET: Accounts
        public ActionResult Index()
        {
            List<UserProfile> users = _userProfileRepository.GetAllUsers();
            return View(users);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.UserType.Name),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        //GET: Account/Create
        public ActionResult Create()
        {
            UserProfile user = new UserProfile();
            return View();

        }

        //POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfile user)
        {
            try
            {
                _userProfileRepository.CreateUser(user);
                return RedirectToAction("Index", "Home" );
            }
            catch (Exception ex)
            {
                return View(user);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
     
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            UserProfile user = _userProfileRepository.GetById(id);
            List<UserType> types = _userTypeRepository.GetAllTypes();
         
            int admins = _userProfileRepository.getAdminCount();

           
        
            EditUserProfileViewModel vm = new EditUserProfileViewModel()
            {
                UserProfile = user,
                UserTypes = types,
                AdminCount = admins

            };


            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditUserProfileViewModel vm)
        {
           
           int getUser = GetCurrentUserId();
           int adminCount = _userProfileRepository.getAdminCount();
            
          if (adminCount <= 1 && getUser == id)
            {
                vm.UserProfile.UserTypeId = 1;
                try
                {
                    _userProfileRepository.UpdateUserProfile(id, vm.UserProfile);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(vm);
                }
            }
            else
            {
                try
                {
                    _userProfileRepository.UpdateUserProfile(id, vm.UserProfile);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(vm);
                }
            }
         }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        //GET: Account/Delete/id
        public ActionResult Deactivate(int id)
        {
            UserProfile user = _userProfileRepository.GetById(id);
            return View(user);
        }

        //POST: Account/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.DeactivateUser(userProfile.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(userProfile);
            }
        }

    }
}


