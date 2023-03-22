using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repository;

public class ProductRepository:IBrandRepository,ITypeRepository,IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await  _context.Brands.Find(p => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context.Types.Find(t => true).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Products.Find(p => true).ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsByName(string name)
    {
        FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context.Products.Find(filters).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsByBrand(string name)
    {
        FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
        return await _context.Products.Find(filters).ToListAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updatedResult =await _context.Products.ReplaceOneAsync(p=>p.Id==product.Id,product);
        return updatedResult.IsAcknowledged && updatedResult.ModifiedCount>0;

    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Id, id);
        var deletedResult = await _context.Products.DeleteOneAsync(filters);
        return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
    }
}