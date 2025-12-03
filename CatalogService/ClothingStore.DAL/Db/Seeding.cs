using ClothingStore.DAL.Entities;
using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Db
{
    public static class Seeding
    {
        public static async Task SeedAsync(ClothingStoreContext context)
        {
            if (context.Categories.Any())
                return; // БД вже заповнена

            var faker = new Faker();

            // ----------------- Categories (Clothing Categories) -----------------
            var categories = new List<Category>
            {
                new Category { Name = "Men's Clothing", Description = "Clothing for men", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Women's Clothing", Description = "Clothing for women", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Kids' Clothing", Description = "Clothing for children", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Shoes", Description = "Footwear for all ages", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Accessories", Description = "Fashion accessories", IsActive = true, CreatedAt = DateTime.UtcNow }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // ----------------- Suppliers -----------------
            var suppliers = new Faker<Supplier>()
                .RuleFor(s => s.Name, f => f.Company.CompanyName())
                .RuleFor(s => s.Email, f => f.Internet.Email())
                .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.IsActive, true)
                .RuleFor(s => s.CreatedAt, DateTime.UtcNow)
                .Generate(5);

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();

            // ----------------- Products (Clothing Items) -----------------
            var clothingItems = new[]
            {
                "Classic White T-Shirt", "Denim Jeans", "Cotton Polo Shirt", "Wool Sweater", "Leather Jacket",
                "Summer Dress", "Running Sneakers", "Formal Shirt", "Casual Shorts", "Winter Coat",
                "Baseball Cap", "Silk Scarf", "Leather Boots", "Hoodie", "Tank Top",
                "Blazer", "Cardigan", "Leggings", "Skirt", "Sandals"
            };

            var products = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.PickRandom(clothingItems))
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(25, 500), 2))
                .RuleFor(p => p.SKU, f => $"SKU-{f.Random.AlphaNumeric(8).ToUpper()}")
                .RuleFor(p => p.IsActive, true)
                .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).CategoryId)
                .Generate(15);

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // ----------------- ProductDetails (1:1) -----------------
            var brands = new[] { "Nike", "Adidas", "Zara", "H&M", "Uniqlo", "Gap", "Levi's", "Calvin Klein" };
            var materials = new[] { "Cotton", "Polyester", "Wool", "Leather", "Silk", "Denim", "Linen", "Cashmere" };
            var colors = new[] { "Black", "White", "Navy", "Gray", "Red", "Blue", "Beige", "Brown" };
            var sizes = new[] { "XS", "S", "M", "L", "XL", "XXL" };
            var careInstructions = new[] { "Machine wash cold", "Hand wash only", "Dry clean only", "Do not bleach" };

            var details = products.Select(p => new ProductDetail
            {
                Brand = faker.PickRandom(brands),
                Material = faker.PickRandom(materials),
                Color = faker.PickRandom(colors),
                Size = faker.PickRandom(sizes),
                CareInstructions = faker.PickRandom(careInstructions),
                StockQuantity = faker.Random.Int(0, 100),
                ProductId = p.ProductId,
                Product = p
            }).ToList();

            await context.ProductDetails.AddRangeAsync(details);
            await context.SaveChangesAsync();

            // ----------------- ProductSuppliers (M:N) -----------------
            var random = new Random();
            var productSuppliers = new List<ProductSupplier>();

            foreach (var product in products)
            {
                var selectedSuppliers = suppliers
                    .OrderBy(s => random.Next())
                    .Take(random.Next(1, 4)) // 1-3 suppliers per product
                    .ToList();

                foreach (var supplier in selectedSuppliers)
                {
                    productSuppliers.Add(new ProductSupplier
                    {
                        ProductId = product.ProductId,
                        SupplierId = supplier.SupplierId,
                        SupplierPrice = Math.Round(product.Price * (decimal)(0.6 + random.NextDouble() * 0.2), 2), // 60-80% of product price
                        SupplyDate = DateTime.UtcNow.AddDays(-random.Next(0, 90))
                    });
                }
            }

            await context.Set<ProductSupplier>().AddRangeAsync(productSuppliers);
            await context.SaveChangesAsync();
        }
    }
}
