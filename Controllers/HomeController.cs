using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [ActivatorUtilitiesConstructor]
    public HomeController(ILogger<HomeController> logger, BlogDbContext dbContext)
        : this(logger)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        if (_dbContext != null)
        {
            var blogPosts = _dbContext.BlogPosts.ToList();
            return View(blogPosts);
        }
        
        // Handle the case where _dbContext is null

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
