using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLog;
using NorthwindConsole.Models;

//todo connect to Northwind - done
//add records to products and cagtegories - done
//edit products and cagtegories - done
//display all products and cagtegories - option 4 - done
//display specific products and cagtegories with all fields - option 3 variation
//Delete records from products and categories - done
//use data annotation and handle all user errors gracefully & log errors to NLogger - done-ish

//display all records, just product, choice between active or inactive, or all
//display Specific product, all fields
//

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            NorthwindContext NWContext = new NorthwindContext();
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display Category and related products");
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("5) Add Records");
                    Console.WriteLine("6) Edit Category");
                    Console.WriteLine("7) Delete Category");
                    Console.WriteLine("8) Edit Product");
                    Console.WriteLine("9) Delete Product");
                    Console.WriteLine("10) Display Products");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");

                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            NWContext.DisplayCategories();
                            break;
                        case "2":
                            Console.Clear();
                            NWContext.AddCategory();
                            break;
                        case "3":
                            Console.Clear();
                            NWContext.DisplayCategoriesAndProducts();
                            break;
                        case "4":
                            Console.Clear();
                            NWContext.DisplayAll();
                            break;
                        case "5":
                            Console.Clear();
                            NWContext.AddRecord();
                            break;
                        case "6":
                            Console.Clear();
                            NWContext.EditCategory();
                            break;
                        case "7":
                            Console.Clear();
                            NWContext.DeleteCategory();
                            break;
                        case "8":
                            Console.Clear();
                            NWContext.EditProduct();
                            break;
                        case "9":
                            Console.Clear();
                            NWContext.DeleteProduct();
                            break;
                        case "10":
                            Console.Clear();
                            NWContext.DisplayProducts();
                            break;


                        default:
                            //Console.Clear();
                            //NWContext.ListSuppliers();
                            break;
                    }


                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
