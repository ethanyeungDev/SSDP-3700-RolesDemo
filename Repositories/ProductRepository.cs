namespace RolesDemo.Repositories;

using RolesDemo.Data;
using RolesDemo.Models;

public class ProductRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all products ordered by product ID.
    /// </summary>
    /// <returns>An enumerable collection of all <see cref="Product"/> records.</returns>
    public IEnumerable<Product> GetAll()
    {
        return _context.Products
            .OrderBy(p => p.ProductId)
            .ToList();
    }

    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The product ID to search for.</param>
    /// <returns>The matching <see cref="Product"/>, or <c>null</c> if not found.</returns>
    public Product? GetById(string id)
    {
        return _context.Products
            .FirstOrDefault(p => p.ProductId == id);
    }

    /// <summary>
    /// Creates a new product and saves it to the database.
    /// </summary>
    /// <param name="product">The product to create.</param>
    public void Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    /// <param name="product">The product with updated values.</param>
    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    public void Delete(string id)
    {
        var product = GetById(id);

        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Checks whether a product with the given ID exists in the database.
    /// </summary>
    /// <param name="id">The product ID to check.</param>
    /// <returns><c>true</c> if the product exists; otherwise <c>false</c>.</returns>
    public bool Exists(string id)
    {
        return _context.Products
            .Any(p => p.ProductId == id);
    }
}
