using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data;

public class BlogAppContext: IdentityDbContext<IdentityUser>
{
    public BlogAppContext(DbContextOptions<BlogAppContext> options) : base(options)
    {

    }

    public DbSet<BlogApp.Models.Post> Posts { get; set; } = default!;
    public DbSet<BlogApp.Models.Category> Categories { get; set; } = default!;
    public DbSet<BlogApp.Models.Comment> Comments { get; set; } = default!;
}