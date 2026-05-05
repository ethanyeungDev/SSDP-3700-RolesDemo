using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolesDemo.Models;
using RolesDemo.Repositories;
using System.Diagnostics;

namespace RolesDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="productRepository">The product repository.</param>
    public HomeController(ILogger<HomeController> logger,
                          ProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Displays the home page.
    /// </summary>
    /// <returns>The Index view.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays the secure area with a sorted list of products.
    /// Requires the user to be authenticated.
    /// </summary>
    /// <returns>The SecureArea view with sorted products.</returns>
    [Authorize]
    public IActionResult SecureArea()
    {
        var products = _productRepository.GetAll();

        var sortedProducts = products
            .OrderBy(p => int.Parse(p.ProductId))
            .ToList();

        return View(sortedProducts);
    }

    /// <summary>
    /// Displays the product creation form with the next available product ID pre-filled.
    /// Requires the user to be authenticated.
    /// </summary>
    /// <returns>The Create view with a new <see cref="Product"/> pre-populated with the next ID.</returns>
    [Authorize]
    public IActionResult Create()
    {
        var products = _productRepository.GetAll();

        int nextId = products.Any()
            ? products.Select(p => int.Parse(p.ProductId)).Max() + 1
            : 1;

        var product = new Product
        {
            ProductId = nextId.ToString()
        };

        return View(product);
    }

    /// <summary>
    /// Handles the product creation form submission.
    /// Requires the user to be authenticated.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <returns>Redirects to SecureArea on success; otherwise redisplays the Create view.</returns>
    [HttpPost]
    [Authorize]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.Create(product);
            return RedirectToAction(nameof(SecureArea));
        }

        return View(product);
    }

    /// <summary>
    /// Displays the privacy policy page.
    /// </summary>
    /// <returns>The Privacy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Displays the error page with request diagnostic information.
    /// </summary>
    /// <returns>The Error view with an <see cref="ErrorViewModel"/>.</returns>
    [ResponseCache(Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
