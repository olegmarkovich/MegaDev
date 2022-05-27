using DemoMegaDev.Data;
using DemoMegaDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DemoMegaDev.Controllers
{
    public class BlogsController : Controller
    {
        private readonly MegaDevContext _context;

        private readonly IWebHostEnvironment Enviroment;

        public BlogsController(MegaDevContext context, IWebHostEnvironment environment)
        {
            _context = context;
            Enviroment = environment;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var megaDevContext = _context.Blogs.Include(b => b.Image).Where(b => b.MainBlogId == null && b.ScheduleDate <= DateTime.Now);
            return View(await megaDevContext.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            ViewData["MainBlogId"] = new SelectList(_context.Blogs, "Id", "Article");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Headline,Excerpt,Article,ScheduleDate,LanguageId,FormImage")] Blog blog)
        { 

            if (ModelState.IsValid)
            {
                Image image = null;

                if(blog.FormImage != null)
                {
                    image = new Image
                    {
                        File = Path.Combine("Images", "Uploads", blog.FormImage.FileName)
                        , LastChangeDate = DateTime.Now
                    };

                    using(Stream stream = new FileStream(Path.Combine(Enviroment.WebRootPath, "Images", "Uploads", blog.FormImage.FileName), FileMode.Create))
                    {
                        await blog.FormImage.CopyToAsync(stream);
                    }
                }

                blog.CreationDate = DateTime.Now;
                blog.LastUpdate = DateTime.Now;

                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();

                if(image != null)
                {
                    image.BlogId = blog.Id;
                    _context.Images.Add(image);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["MainBlogId"] = new SelectList(_context.Blogs, "Id", "Article", blog.MainBlogId);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["MainBlogId"] = new SelectList(_context.Blogs, "Id", "Article", blog.MainBlogId);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Headline,Excerpt,Article,ScheduleDate,LanguageId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blog.LastUpdate = DateTime.Now;
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            ViewData["MainBlogId"] = new SelectList(_context.Blogs, "Id", "Article", blog.MainBlogId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.MainBlog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'MegaDevContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
