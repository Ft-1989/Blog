using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers;

public class BlogController : Controller
{
    private readonly BlogDbContext _dbContext;

    public BlogController(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = "User, Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "UserAndAdmin")]
    public IActionResult Create(BlogPost blogPost)
    {
        if (ModelState.IsValid)
        {
            _dbContext.BlogPosts.Add(blogPost);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        return View(blogPost);
    }
}