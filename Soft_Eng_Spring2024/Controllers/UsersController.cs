using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Soft_Eng_Spring2024.Data;
using Soft_Eng_Spring2024.Models;

namespace Soft_Eng_Spring2024.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize(Policy ="MustBeAdmin")]
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/User_Approval
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> User_Approval()
        {

            var users =await _context.Users.ToListAsync();

            return View(users);
        }

        // POST: Users/User_Approval
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> User_Approval(int Id, int Role)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(Id);
                if (user == null)
                {
                    return NotFound();
                }
                user.Role = Role;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(User_Approval));
            }
            return View();
        }


        [Authorize(Policy="AdminOrProfessor")]
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET:Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // GET:Users/Login
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index","Home");
        }

        public IActionResult Index1()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Firstname,Lastname,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Firstname,Lastname,Email,Password,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(User_Approval));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Firstname,Lastname,Email,Password")] User user)
        {
            Hash_Password(user);
            if (ModelState.IsValid)
            { 
                _context.Add(user);
                await _context.SaveChangesAsync();
                securityContext(user);
                return RedirectToAction("Index","Home");
            }
            return View(user);
        }

        private void Hash_Password(User user)
        {
            user.Salt = Create_Salt();
            byte[] pass=Encoding.ASCII.GetBytes(user.Password);
            var hash = Rfc2898DeriveBytes.Pbkdf2(pass, Convert.FromBase64String(user.Salt),10, hashAlgorithm,64);
            user.Password=Convert.ToBase64String(hash);
                
        }

        private string Create_Salt()
        {
            var salt=RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(salt);
        }

        private bool Verify_Password(string givenPass,User user)
        {
            var derivedHash= Rfc2898DeriveBytes.Pbkdf2(givenPass, Convert.FromBase64String(user.Salt), 10, hashAlgorithm, 64);
            return CryptographicOperations.FixedTimeEquals(derivedHash, Convert.FromBase64String(user.Password));
            
        }


        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? Email,string? Password)
        {
            if (Email == null || Password == null) {
                return NotFound();
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == Email);
            if (user == null)
            {
                return NotFound();
            }
            if (Verify_Password(Password, user))
            {
                securityContext(user);
                return RedirectToAction("Index","Home");
            }

            return NotFound();
        }
        private void securityContext(User user)
        {
            //creating security context
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Role",user.Role.ToString()),
                    new Claim("uId",user.Id.ToString())
                };
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = claimsPrincipal;
            HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
        }
    }
}
