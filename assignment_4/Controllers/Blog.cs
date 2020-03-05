using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment_4.Data;
using assignment_4.Models;
using Microsoft.AspNetCore.Authorization;
 using  Microsoft.AspNetCore.Identity;
namespace assignment_4.Controllers
{
    public class Blog : Controller
    {
        private readonly ApplicationDbContext _context;

        public Blog(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(c => c.Owner).OrderByDescending(p => p.Time);    //Sorterer slik at nyeste post kommer først
             
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Blog/Add
        [Authorize]
        public IActionResult Add()
        {
            
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id",User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View();
        }

        // POST: Blog/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Title,Summary,Content,Time,OwnerId")] Post post)
        {
            if (ModelState.IsValid)
            {


                post.Time = DateTime.Now;            //Nåværende tid. Dato med klokkeslett
                
                post.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);        //Den nye blog posten blir registrert på den brukeren om poster den
                
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            return View(post);
        }

        [Authorize]
        // GET: Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier)){            //Sjekker om blog post IDen er den samme som ID på innlogget bruker
            
                ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", User.FindFirstValue(ClaimTypes.NameIdentifier));
           
                return View(post);
        }
            return RedirectToAction(nameof(Index));        //Redirecter til Index hvis man ikke er logga inn eller er den rette bruker til å redigere blog posten
    }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Summary,Content,Time,OwnerId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.Time = DateTime.Now;  
                    
                    post.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);        //Den nye blog posten blir registrert på den brukeren om poster den
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            return View(post);
        }

        // GET: Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
