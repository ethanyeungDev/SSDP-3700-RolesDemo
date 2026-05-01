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

    public HomeController(ILogger<HomeController> logger,
                          ProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult SecureArea()
    {
        var products = _productRepository.GetAll();

        var sortedProducts = products
            .OrderBy(p => int.Parse(p.ProductId))
            .ToList();

        return View(sortedProducts);
    }

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

    public IActionResult Privacy()
    {
        return View();
    }

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
