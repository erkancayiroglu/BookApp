using BookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Controllers
{
    public class BookController:Controller
    {
        public readonly DataContext _context;
        public BookController(DataContext context) //Bu işemlere Injection deniyor.
        {
            _context = context;
        }
      
        public IActionResult Create(int? Id)
        {
            if (Id == null)
            {
                ViewBag.Users = new SelectList(_context.Users, "UserId", "UserName");

                return View();
            }

            // Kullanıcı bilgisini çekiyoruz
            var user = _context.Users.Find(Id);
            if (user == null)
            {
                return NotFound();
            }

            
            var book = new BookViewModel//Model olarak bunu alıyor.
            {
                UserId = user.UserId
            };

            
            ViewBag.Users = new SelectList(_context.Users, "UserId", "UserName");

            return View(book);
        }


        [HttpPost]
        public async Task<IActionResult> Create(BookViewModel model)//Kitap bilgileri geleceği için Bu şekilde 
        {//asenkron sıra alınır yemek gelince verilir senkronda kişinin yemeğei gelmeden başka sipariş alınmaz
    
            if (ModelState.IsValid)
            {
                _context.Books.Add(new Book() { BookId = model.BookId, BookName = model.BookName, AuthorName = model.AuthorName,
                    PublisherName = model.PublisherName, PublisherYear = model.PublisherYear ,UserId=model.UserId });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "UserId", "UserName");
            return View(model);


        }

        public async Task<IActionResult> Index(string searchString, string author)
        {
      

            var books = _context.Books.Include(b => b.Users).AsQueryable(); // IQueryable olarak başlatıyoruz

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchString = searchString;
                books = books.Where(b => b.BookName.ToLower().Contains(searchString.ToLower())); // Veritabanı üzerinde filtreleme
            }

            if (!String.IsNullOrEmpty(author)&& author!="0")
            {
                books = books.Where(b => b.BookId == int.Parse(author));//b.bookid ile tüm satırları kontrol edip filtreleme işlemi
            }


            ViewBag.Books=new SelectList(_context.Books, "BookId","AuthorName");

            var bookList = await books.ToListAsync(); // Veritabanına sorguyu yapıp listeyi alıyoruz.

            return View(bookList);
        }
        


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }


            var book = await _context

                .Books
                .Include(u => u.Users) //Buna bile gerek yok
                                      .Select(u=>new BookViewModel //Normal book da kullanabiliriz
                                      { 
                                         BookId=u.BookId, BookName=u.BookName,AuthorName=u.AuthorName,PublisherName=u.PublisherName,PublisherYear=u.PublisherYear,UserId=u.UserId 
                                       })                                                                                //Bu şekilde  istediğimiz bilgileri 

                  .FirstOrDefaultAsync(u => u.BookId == id);


            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "UserId", "UserName");
            return View(book);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BookViewModel model)//Burası Kurs di
        {
            if (id != model.BookId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Books.Update(new Book() { BookId = model.BookId, BookName = model.BookName, AuthorName = model.AuthorName, PublisherName = model.PublisherName, PublisherYear = model.PublisherYear, UserId = model.UserId });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {                       // o nesnesi veri tabını tarıyor moddelin id sine bakıyor.
                    if (!_context.Books.Any(o => o.BookId == model.BookId))
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
            ViewBag.Books = new SelectList(await _context.Books.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(model);
        }

       public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);


            if(book==null)
            {
                return NotFound();
            }

            return View(book);

        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int BookId)//asp-for ile de @Model.BookId gönderip iki parametre olaiblir
        {
            if (BookId == null)
            {
                return NotFound();

            }
            var book = await _context.Books.FindAsync(BookId);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }

            var book= await _context
                .Books
                .Include(b=>b.Users)
                .FirstOrDefaultAsync(u => u.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);


        }







    }
}
