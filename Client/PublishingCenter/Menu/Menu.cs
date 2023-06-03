using PublishingCenter.PublishingCenter.Tables;
using System;

namespace PublishingCenter.PublishingCenter.Menu
{
    public class Menu
    {
        public static void Start()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Publishing Center");
                Console.WriteLine("1. Writers\n2. Contracts\n3. Customers\n4. Books\n5. Orders\n6. Exit");
                string str = Console.ReadLine();
                if (str == "Writers")
                {
                    Console.Clear();
                    try
                    {
                        TabWriters.WritersStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if (str == "Contracts")
                {
                    Console.Clear();
                    
                    try
                    {
                        TabContarcts.ContractsStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
                else if (str == "Customers")
                {
                    Console.Clear();
                    
                    try
                    {
                        TabCustomers.CustomersStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if (str == "Books")
                {
                    Console.Clear();
                    
                    try
                    {
                        TabBooks.BooksStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if (str == "Orders")
                {
                    Console.Clear();
                    
                    try
                    {
                        TabOrders.OrdersStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if (str == "Exit")
                    exit = true;
                else
                    Console.WriteLine("Incorrect value!");
            }
        }
    }
}
