using InvManagmentAO.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvManagmentAO.Controllers
{
    public class AuthController : Controller
    {

        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context=context;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser(string name,  string email, string password, string password2)
        {
            if(await _context.Users.AnyAsync(u => u.Name == name))
            {
                ViewBag.Message = "Numele este deja inregistrat";
                return View("Register");
            }

            if(await _context.Users.AnyAsync(u => u.Email == email))
            {
                ViewBag.Message = "Emailul este deja inregistrat";
                return View("Register");
            }

            if (password.Length < 6)
            {
                ViewBag.Message = "Parola trebuie sa aiba minim 6 caractere";
                return View("Register");
            }

            if(password != password2)
            {
                ViewBag.Message = "Parolele nu sunt identice";
                return View("Register");
            }

            var newUser= new Models.User { 
                Name=name,
                Email=email,
                Password=password
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return View("Login");
        }



        [HttpPost]
        public async Task<IActionResult> LoginUser(string name, string password)
        {
            if(name=="" || password == "")
            {
                ViewBag.Message = "Campurile sunt goale";
                return View("Login");
            }

            Models.User user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name && u.Password == password);
            if(user != null)
            {
                return View("Register");
            }

            HttpContext.Session.SetString("name", name);
            return View("Invetory", "Managment");

        }





    }

}

