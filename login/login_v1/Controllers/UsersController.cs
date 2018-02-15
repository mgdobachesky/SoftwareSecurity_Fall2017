using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using login_v1.Models;
using System.Text;

namespace login_v1.Controllers
{
	public class UsersController : Controller
	{
		private readonly login_v1Context _context;

		public UsersController(login_v1Context context)
		{
			_context = context;
		}

		// GET: User/Login
		public IActionResult Login()
		{
			//ViewData["Message"] = "Login with your username and password.";

			return View();
		}

		// POST: Users/Login
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login([Bind("username,password")] User user)
		{
            if (!String.IsNullOrEmpty(user.username))
            {
                var userInDB = from m in _context.User
                               where m.username == user.username
                               select m;
                userInDB = userInDB.Take(1);
                foreach (var item in userInDB)
                {
                    user.salt = item.salt;
                }
            }
                if (UserLoginExists(user.username, HashPassword(user.password, user.salt)))
                {
                    ViewData["Message"] = "Login Successful!";
                }
                else
                {
                    ViewData["Message"] = "Login Failed!";
                }
			return View();
		}

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,username,password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.salt = GetSalt();
                user.password = HashPassword(user.password, user.salt);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
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

            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,username,password,salt")] User user)
        {
            if (id != user.ID)
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
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
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

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.ID == id);
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
            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

		private bool UserLoginExists(string username, string password)
		{
			return _context.User.Any(e => e.username == username && e.password == password);
		}

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));

                // Get the hashed string.
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                
                // Retrun hased password.
                return hash;
            }
        }

        private string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
