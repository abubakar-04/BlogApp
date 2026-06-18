using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Controllers;
[Authorize]
public class PostsController: Controller
{
    private readonly BlogAppContext _context;
    private readonly UserManager<IdentityUser> _usermanager;

    public PostsController(BlogAppContext context, UserManager<IdentityUser> usermanager)
    {
        _context = context;
        _usermanager = usermanager;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, int? categoryId)
    {
        IQueryable<Post> posts = _context.Posts;

        if (!string.IsNullOrEmpty(search))
        {
            posts = posts.Where(p => p.Title.Contains(search));
        }
        if (categoryId.HasValue)
        {
            posts = posts.Where(p => p.CategoryID == categoryId.Value);
        }
        var PostList = await posts.Include(p => p.Category).Include(p => p.User).ToListAsync();

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "Name", categoryId);

        return View(PostList);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts.Include(p => p.Category).Include(p => p.Comments).Include(p => p.User).FirstOrDefaultAsync(p => p.PostID == id);

        return View(post);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "Name");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title", "Body", "CategoryID")] Post post)
    {
        
        if (ModelState.IsValid)
        {
            post.CreatedAt = DateTime.Now;
            post.UserID = _usermanager.GetUserId(User);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = post.PostID });
        }
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "Name");

        return View(post);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (id == null)
        {
            return NotFound();
        }
        if (post == null)
        {
            return NotFound();
        }
        var currentUserId = _usermanager.GetUserId(User);
        if (post.UserID != currentUserId)
        {
            return Forbid();
        }
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(),"CategoryID","Name",post.CategoryID);
        return View(post);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Title", "Body", "CategoryID")] Post post)
    {
        var Existing = await _context.Posts.FindAsync(id);
        if (Existing == null)
        {
            return NotFound();
        }
        var currentUserId = _usermanager.GetUserId(User);
        if (Existing.UserID != currentUserId)
        {
            return Forbid();
        }
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "Name");
            return View(post);

        }
        Existing.Title = post.Title;
        Existing.Body = post.Body;
        Existing.CategoryID = post.CategoryID;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Posts.AnyAsync(e => e.PostID == id))
                return NotFound();
            else
                throw;
        }
        return RedirectToAction("Details", new { id = Existing.PostID });
    }
    [HttpGet]
    
    public async Task<IActionResult> Delete(int? id )
    {
        if (id == null)
        {
            return NotFound();
        }
        var post = await _context.Posts.Include(p => p.Category).Include(p => p.Comments).Include(p => p.User).FirstOrDefaultAsync(p => p.PostID == id.Value);
        if (post == null)
        {
            return NotFound();
        }

        var currentUserId = _usermanager.GetUserId(User);
        if (post.UserID != currentUserId)
        {
            return Forbid();
        }
        if (post == null) return NotFound();

        return View(post);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id, bool notUsed)
    {
        var currentUserId = _usermanager.GetUserId(User);
        if (id == null)
        {
            return NotFound();
        }
        var Existing = await _context.Posts.Include(p => p.Comments).Include(p => p.User).FirstOrDefaultAsync(p => p.PostID == id);
        if (Existing == null)
        {
            return NotFound();
        }
        if (Existing.UserID != currentUserId)
        {
            return Forbid();
        }
        else
        {
            _context.Posts.Remove(Existing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}