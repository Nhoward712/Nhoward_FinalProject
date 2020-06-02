using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using NLog;
using Nhoward_FinalProject.Models;
namespace Nhoward_FinalProject.Models
{
    public class NorthwindContext : DbContext
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NorthwindContext() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }


        public void AddCategory()
        {
            Category category = new Category();
            Console.WriteLine("Enter Category Name:");
            category.CategoryName = Console.ReadLine();
            Console.WriteLine("Enter the Category Description:");
            category.Description = Console.ReadLine();

            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if (isValid)
            {
                var db = new NorthwindContext();
                // check for unique name
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db
                    db.Categories.Add(category);

                    db.SaveChanges();
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public void DisplayCategories()
        {
            var db = new NorthwindContext();
            var query = db.Categories.OrderBy(p => p.CategoryName);

            Console.WriteLine($"{query.Count()} records returned");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryName} - {item.Description}");
            }
            Console.WriteLine("");
        }

        internal void AddRecord()
        {
            var db = new NorthwindContext();


            var query = db.Categories.OrderBy(p => p.CategoryId);
            var category = db.Categories;
            Console.WriteLine("Select the category to add a product to:");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
            }
            int id = int.Parse(Console.ReadLine());
            var cat = category.FirstOrDefault(b => b.CategoryId == id);



            //var sQuery = db.Suppliers.OrderBy(s => s.SupplierId).ToList();
            //var supplier = db.Suppliers;
            //Console.WriteLine("Who Supplies this item?");
            //try
            //{
            //    foreach (var i in sQuery)
            //    {
            //        Console.WriteLine($"{i.SupplierId}) {i.CompanyName}");
            //    }
            //}
            //catch
            //{
            //    Console.WriteLine("");
            //}
            //int supID = int.Parse(Console.ReadLine());
            //var sup = supplier.FirstOrDefault(s => s.SupplierId == supID);




            Product product = new Product();
            product.SupplierId = 1;
            product.CategoryId = cat.CategoryId;
            Console.Clear();
            Console.WriteLine("Product Name");//cant be existing product name
            String productName = Console.ReadLine();
            product.ProductName = productName;

            Console.WriteLine("Quantity per Unit");
            String quantityPerUnit = Console.ReadLine();
            product.QuantityPerUnit = quantityPerUnit;

            Console.WriteLine("Unit Price");
            decimal unitPrice = decimal.Parse(Console.ReadLine());
            product.UnitPrice = unitPrice;

            Console.WriteLine("Units On Hand");
            short onHand = short.Parse(Console.ReadLine());
            product.UnitsInStock = onHand;

            Console.WriteLine("Units On Order");
            short ordered = short.Parse(Console.ReadLine());
            product.UnitsOnOrder = ordered;

            Console.WriteLine("Reorder Level");
            short reorder = short.Parse(Console.ReadLine());
            product.ReorderLevel = reorder;

            string active;
            bool discontinued = true;
            do
            {
                Console.WriteLine("Is product active");
                Console.WriteLine("(0) - Active");
                Console.WriteLine("(1) - Inactive or Discontinued");
                active = (Console.ReadLine());
            } while (active != "1" && active != "0");
            discontinued = active == "0" ? false : true;
            product.Discontinued = discontinued;
            db.Products.Add(product);

            db.SaveChanges();




        }

        internal void ListSuppliers()
        {
            var db = new NorthwindContext();
            var query = db.Suppliers.OrderBy(s => s.SupplierId).ToList();
            foreach (var i in query)
            {
                Console.WriteLine($"{i.SupplierId}{i.CompanyName}");
            }
        }

        internal void EditProduct()
        {
            var db = new NorthwindContext();
            var query = db.Categories.OrderBy(p => p.CategoryId);

            Console.WriteLine("Select the category whose products you want to edit:");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
            }
            int id = int.Parse(Console.ReadLine());

            Console.Clear();
            logger.Info($"CategoryId {id} selected");
            Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            Console.WriteLine($"{category.CategoryName} - {category.Description}");
            foreach (Product p in category.Products)
            {
                Console.WriteLine($"{p.ProductID}) {p.ProductName}");
            }
            Console.WriteLine("Enter the product ID of that you would like to edit:");
            int prodId = int.Parse(Console.ReadLine());
            var product = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            Console.WriteLine("Quantity per Unit");
            String quantityPerUnit = Console.ReadLine();
            product.QuantityPerUnit = quantityPerUnit;

            Console.WriteLine("Unit Price");
            decimal unitPrice = decimal.Parse(Console.ReadLine());
            product.UnitPrice = unitPrice;

            Console.WriteLine("Units On Hand");
            short onHand = short.Parse(Console.ReadLine());
            product.UnitsInStock = onHand;

            Console.WriteLine("Units On Order");
            short ordered = short.Parse(Console.ReadLine());
            product.UnitsOnOrder = ordered;

            Console.WriteLine("Reorder Level");
            short reorder = short.Parse(Console.ReadLine());
            product.ReorderLevel = reorder;

            string active;
            bool discontinued = true;
            do
            {
                Console.WriteLine("Is product active");
                Console.WriteLine("(0) - Active");
                Console.WriteLine("(1) - Inactive or Discontinued");
                active = (Console.ReadLine());
            } while (active != "1" && active != "0");
            discontinued = active == "0" ? false : true;
            product.Discontinued = discontinued;
            db.SaveChanges();





        }

        internal void EditCategory()
        {
            var db = new NorthwindContext();
            var categories = db.Categories;
            Console.WriteLine("Which Category would you like to edit?");
            foreach (var item in db.Categories)
            {
                Console.WriteLine("{0,-10}{1,-20}", item.CategoryId, item.CategoryName);
                Console.WriteLine();
            }
            int choice = int.Parse(Console.ReadLine());
            var category = categories.FirstOrDefault(b => b.CategoryId == choice);

            Console.WriteLine("What would you like to rename your category to?");
            string catName = Console.ReadLine();
            Console.WriteLine("Description");
            string desc = Console.ReadLine();

            category.CategoryName = catName;
            category.Description = desc;
            db.SaveChanges();
        }

        internal void DeleteProduct()
        {
            var db = new NorthwindContext();
            var query = db.Categories.OrderBy(p => p.CategoryId);

            Console.WriteLine("Select the category whose products you want to edit:");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
            }
            int id = int.Parse(Console.ReadLine());

            Console.Clear();
            logger.Info($"CategoryId {id} selected");
            Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            Console.WriteLine($"{category.CategoryName} - {category.Description}");

            foreach (Product p in category.Products)
            {
                Console.WriteLine($"{p.ProductID}) {p.ProductName}");
            }
            Console.WriteLine("Enter the product ID of that you would like to delete:");
            int prodId = int.Parse(Console.ReadLine());
            var product = db.Products.FirstOrDefault(p => p.ProductID == prodId);
            Console.WriteLine("");
            db.Products.Remove(product);
            db.SaveChanges();
            logger.Info($"Product ID {prodId} deleted");

        }

        internal void DeleteCategory()
        {
            var db = new NorthwindContext();
            var query = db.Categories.OrderBy(p => p.CategoryId);

            Console.WriteLine("Select the category ID you want to delete:");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
            }
            int id = int.Parse(Console.ReadLine());

            Console.Clear();
            logger.Info($"CategoryId {id} selected");
            Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            Console.WriteLine($"{category.CategoryId}{category.CategoryName}");

            var count = category.Products.Count();
            Console.WriteLine($"{count}");
            if (count > 0)
            {
                try
                {
                    do
                    {
                        foreach (Product p in category.Products)
                        {
                            Console.WriteLine("removing the following products");
                            Console.WriteLine($"{p.ProductID}) {p.ProductName}");
                            var product = db.Products.FirstOrDefault(d => d.ProductID == p.ProductID);
                            db.Products.Remove(product);
                        }

                    } while (true);
                }
                catch
                {
                    logger.Info($"User choice is not valid");
                    Console.WriteLine("Not a valid choice.  ");
                }

            }

            db.Categories.Remove(category);

            db.SaveChanges();

            logger.Info($"Category Id {id} deleted");
        }

        internal void DisplayAll()
        {
            var db = new NorthwindContext();
            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryName}");
                foreach (Product p in item.Products.Where(r => !r.Discontinued))
                {

                    Console.WriteLine($"\t{p.ProductName}");
                }
            }
        }

        internal void DisplayCategoriesAndProducts()
        {
            //Display all records in the Products table(ProductName only) - user decides if they want to see 
            //all products, discontinued products, or active(not discontinued) products.Discontinued products 
            //should be distinguished from active products.

            var db = new NorthwindContext();
            var query = db.Categories.OrderBy(p => p.CategoryId);

            Console.WriteLine("Select the category whose products you want to display:");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
            }
            int id = int.Parse(Console.ReadLine());
            Console.Clear();
            logger.Info($"CategoryId {id} selected");

            Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            Console.WriteLine($"{category.CategoryName} - {category.Description}");

            var count = category.Products.Count();
            Console.WriteLine($"{count} Items returned");
            if (count > 0)
            {
                try
                {
                    Console.WriteLine("Which ProductID would you like to see details for?");
                    int choice = 0;
                    do
                    {
                        foreach (Product p in category.Products)
                        {
                            Console.WriteLine($"{ p.ProductID} {p.ProductName}");
                        }
                        Console.WriteLine("\n\n");
                        choice = int.Parse(Console.ReadLine());
                        Product spec = db.Products.FirstOrDefault(d => d.ProductID == choice);
                        Console.WriteLine("{0,-10}{1,-20}{2,-25}{3,-15}{4,-20}{5,-20}{6,-15}", "ProdID", "Product Name", "Pack Size", "Reorder Level", "Unit Price", "Units in Stock", "Units on order");

                        Console.WriteLine("{0,-10}{1,-20}{2,-25}{3,-15}{4,-20}{5,-20}{6,-15}", spec.ProductID, spec.ProductName, spec.QuantityPerUnit, spec.ReorderLevel, spec.UnitPrice, spec.UnitsInStock, spec.UnitsOnOrder);
                        Console.WriteLine();
                    } while (choice < 0);
                }
                catch
                {
                    logger.Info($"User choice is not valid");
                    Console.WriteLine("Not a valid choice.  ");
                }
            }
        }
        internal void DisplayProducts()
        {
            var db = new NorthwindContext();
            var query = db.Products.OrderBy(p => p.ProductID);


            Console.WriteLine("Do you wantsn to display:\n (A)ll products\n (D)iscountinued Products\n a(C)tive products\n");
            String choice = Console.ReadLine();
            switch (choice)
            {
                case "C":
                    query = db.Products.Where(p => !p.Discontinued).OrderBy(p => p.ProductID);
                    break;
                case "D":
                    query = db.Products.Where(p => p.Discontinued).OrderBy(p => p.ProductID);
                    break;
                case "A":
                    query = db.Products.OrderBy(p => p.ProductID);
                    break;
            }
            foreach (var item in query)
            {
                Console.WriteLine($"{item.ProductName}");
            }

        }



    }


}
