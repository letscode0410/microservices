using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public static  class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        bool checkProducts=productCollection.Find(p => true).Any();
        string path = Path.Combine("Data", "SeedData", "products.json");
        if (!checkProducts)
        {
            var typeText = File.ReadAllText(path);
            var productList = JsonSerializer.Deserialize<List<Product>>(typeText);
            productList?.ForEach(x=>productCollection.InsertOneAsync(x));
        }
    }
}