using BookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Controllers
{
    public class UserController :Controller
    {
         public readonly DataContext _context;//yapıcı metot içine 
        public UserController(DataContext context) //Bu işemlere Injection deniyor.
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }   
        [HttpPost]

        public async Task<IActionResult> Create(User model)//User bilgileri geleceği için Bu şekilde 
        {//asenkron sıra alınır yemekgelince verilir senkronda kişinin yemeğei gelmeden başka sipariş alınmaz
            if (ModelState.IsValid)
            {
                _context.Users.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);

        }

        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.ToListAsync();

            return View(user);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {

                return NotFound();

            }
            return View(user);

        }
        [HttpPost]
        
        public async Task<IActionResult> Edit(int? id, User model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {                       // o nesnesi veri tabını tarıyor moddelin id sine bakıyor.
                    if (!_context.Users.Any(u => u.UserId == model.UserId))
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
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
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

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int UserId)
        {
            if (UserId <= 0)
            {
                return NotFound();
            }

            var user = await _context.Users
           .Include(u => u.Books) // Kullanıcının kitaplarını da çek
           .FirstOrDefaultAsync(u => u.UserId == UserId);
            if (user == null)
            {
                return NotFound();
            }

            
            //var userBooks = _context.Books.Where(b => b.UserId == UserId).ToList();

            
            _context.Books.RemoveRange(user.Books);

            // Kullanıcıyı kaldır
            _context.Users.Remove(user);

            // Veritabanına değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(i => i.Books)
                .FirstOrDefaultAsync(i => i.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }






    }




}


    

