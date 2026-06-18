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

namespace BlogApp.Controllers;
[Authorize]
public class CategoriesController : Controller
{
    private readonly BlogAppContext _context;

    public CategoriesController(BlogAppContext context)
    {
        _context = context;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        
        var categories = await _context.Categories.ToListAsync();

        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {


        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")]Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        
        return View(category);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name")] Category category)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null)
            return NotFound();

        // If validation failed, return the view populated with an entity that includes the key
        if (!ModelState.IsValid)
        {
            existing.Name = category.Name; // preserve user input for re-display
            return View(existing);
        }

        // Apply the allowed change to the tracked entity
        existing.Name = category.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Categories.AnyAsync(e => e.CategoryID == id))
                return NotFound();
            else
                throw;
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var Existing = await _context.Categories.FindAsync(id);
        if (Existing == null)
        {
            return NotFound();

        }
       



        return View(Existing);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id , bool notUsed)
    {
        var Existing = await _context.Categories.FindAsync(id);
        if (Existing == null)
        {
            return NotFound();
        }
        else
        {

            _context.Categories.Remove(Existing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}