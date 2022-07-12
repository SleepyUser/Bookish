using Microsoft.AspNetCore.Mvc;

namespace Bookish.Controllers;

public class CatalogController : Controller
{
    private readonly ILogger<CatalogController> _logger;
    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }
    // GET
    
    public IActionResult Index()
    {
        return View();
    }
}