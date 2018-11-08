using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Belt.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
 
        public HomeController(UserContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Register(ValidateUser user){
            if(ModelState.IsValid){
                PasswordHasher<ValidateUser> Hasher = new PasswordHasher<ValidateUser>();
                user.password = Hasher.HashPassword(user, user.password);
                User thisUser = new User{
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password
                };
                _context.Add(thisUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", thisUser.UserId);
                return RedirectToAction("Success", thisUser.UserId);
            }
            else{
                return View("Index");
            }
        }

        public IActionResult Login(string email, string password){
            var thisUser = _context.Users.Where(p => p.email == email).FirstOrDefault();
            if(thisUser != null && password != null){
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(thisUser, thisUser.password, password)){
                    HttpContext.Session.SetInt32("UserId", thisUser.UserId);
                    return RedirectToAction("Success");
                }
            }
            ViewBag.error = "Email and Password do not match";
            return View("Index");
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/home")]
        public IActionResult Success(){
            int curId = (int)HttpContext.Session.GetInt32("UserId");
            if(curId != null){
                ViewBag.name = _context.Users.SingleOrDefault(x => x.UserId == curId);
                ViewBag.userId = curId;
                User thisUser = _context.Users.Where(x => x.UserId == curId).SingleOrDefault();
                var allAct = _context.Todos.Include(x => x.Guests).ThenInclude(x => x.user).ToList();
                return View(allAct);
            }
            else{
                return RedirectToAction("/");
            }
        }
        [Route("/new")]
        public IActionResult NewActivity(){
            return View();
        }

        [Route("/join/{id}")]
        public IActionResult Join(int id){
            int curId = (int)HttpContext.Session.GetInt32("UserId");
            Todo thisAct = _context.Todos.Include(x => x.Guests).ThenInclude(x => x.user).SingleOrDefault(x => x.ActivityId == id);
            Guest newGuest = new Guest(){
                UserId = curId,
                ActivityId = id
            };
            _context.Guests.Add(newGuest);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }

        [Route("/unjoin/{id}")]
        public IActionResult UnJoin(int id){
            int curId = (int)HttpContext.Session.GetInt32("UserId");
            Guest thisGuest = _context.Guests.SingleOrDefault(x => x.ActivityId == id && x.UserId == curId);
            if(thisGuest != null){
                _context.Guests.Remove(thisGuest);
                _context.SaveChanges();
            }
            return RedirectToAction("Success");
        }

        [Route("/delete/{id}")]
        public IActionResult Delete(int id){
            Todo thisAct = _context.Todos.Include(x => x.Guests).SingleOrDefault(x => x.ActivityId == id);
            foreach(var guest in thisAct.Guests){
                _context.Remove(guest);
            }
            _context.Remove(thisAct);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }

        public IActionResult Post(Todo todo){
            int curId = (int)HttpContext.Session.GetInt32("UserId");
            User thisUser = _context.Users.Include(x => x.Todos).SingleOrDefault(x => x.UserId == curId);
            Todo newAct = new Todo(){
                title = todo.title,
                time = todo.time,
                duration = todo.duration,
                duration_type = todo.duration_type,
                description = todo.description,
                UserId = curId
            };
            if(todo.date > DateTime.Now){
                newAct.date = todo.date;
                _context.Todos.Add(newAct);
                _context.SaveChanges();
            }
            else{
                ViewBag.dateError = "Date must be later than today";
                return View("NewActivity");
            }
            return Redirect("/activity/"+newAct.ActivityId);
        }

        [Route("/activity/{id}")]
        public IActionResult Details(int id){
            int curId = (int)HttpContext.Session.GetInt32("UserId");
            Todo thisAct = _context.Todos.Include(x => x.Guests).ThenInclude(x => x.user).SingleOrDefault(x => x.ActivityId == id);
            Todo activity = _context.Todos.Include(x => x.user).SingleOrDefault(x => x.ActivityId == id);
            ViewBag.x = activity.user.first_name;
            ViewBag.userId = curId;
            return View(thisAct);
        }
        [Route("/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
