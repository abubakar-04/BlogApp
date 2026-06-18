using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers;
[Authorize]
public class CommentsController : Controller
{
    private readonly BlogAppContext _context;

    public CommentsController(BlogAppContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> Create([Bind("AuthorName", "Body", "PostID")] Comment comment)
    {
        if (ModelState.IsValid)
        {
            comment.CreatedAt = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","Posts", new { id = comment.PostID });
        }
        return RedirectToAction("Details","Posts", new { id = comment.PostID }); ;
    }
    [HttpPost]
    public async Task<IActionResult> Delete (int? id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        var postid = comment.PostID;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details","Posts", new { id = postid });
    }
}