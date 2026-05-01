namespace RolesDemo.Repositories;

using RolesDemo.Data;
using RolesDemo.Models;

public class ProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all products
    public IEnumerable<Product> GetAll()
    {
        return _context.Products
            .OrderBy(p => p.ProductId)
            .ToList();
    }

    // Get product by ID
    public Product? GetById(string id)
    {
        return _context.Products
            .FirstOrDefault(p => p.ProductId == id);
    }

    // Create product
    public void Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    // Update product
    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    // Delete product
    public void Delete(string id)
    {
        var product = GetById(id);

        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    // Check if the product exists
    public bool Exists(string id)
    {
        return _context.Products
            .Any(p => p.ProductId == id);
    }
}
