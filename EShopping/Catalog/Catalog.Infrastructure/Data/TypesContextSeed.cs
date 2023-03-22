using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public static class TypesContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection)
    {
        var checkTypes = typeCollection.Find(x => true).Any();
        var path = Path.Combine("Data", "SeedData", "types.json");
        if (!checkTypes)
        {
            var typeText = File.ReadAllText(path);
            var typeList = JsonSerializer.Deserialize<List<ProductType>>(typeText);
            typeList?.ForEach(x=>typeCollection.InsertOneAsync(x));
        }
    }
}