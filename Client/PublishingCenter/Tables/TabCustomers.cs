using PublishingCenter_v2.PublishingCenter.Interface;
using PublishingCenter_v2.NHibernate.Data;
using PublishingCenter_v2.NHibernate;
using System.Collections.Generic;
using System;

namespace PublishingCenter_v2.PublishingCenter.Tables
{
    public class TabCustomers
    {
        private static int BLOCK = 10, pages, page = 1, user_num = 1;
        private static int user_action = -1, all_customers = Repository.Instance.GetCount<Customers>();
        private static TemplateInterface<Customers> templateInterface = new TemplateInterface<Customers>(BLOCK);
        private static IList<Customers> customers;
        private static bool call_methods = false;
        private static void _init_()
        {
            page = 1;
            all_customers = Repository.Instance.GetCount<Customers>();
            user_action = -1;
            templateInterface.Reset();
            if (all_customers % BLOCK != 0)
                pages = (all_customers / BLOCK) + 1;
            else
                pages = all_customers / BLOCK;
            call_methods = false;
        }
        public static void CustomersStart()
        {
            templateInterface.Reset();
            if (all_customers % BLOCK != 0)
                pages = (all_customers / BLOCK) + 1;
            else
                pages = all_customers / BLOCK;
            CustomersMenu();
        }

        private static void CustomersMenu()
        {
            bool exit = false;
            while (!exit)
            {
                if (call_methods)
                {
                    _init_();
                }
                user_num = BLOCK * (page - 1) + 1;
                if (user_action == 1 || user_action == -1)
                    ShowNext();
                else if (user_action == 2)
                    ShowPrev();
                ShowListCustomers();
                Console.WriteLine("Page {0} from {1}", page, pages);
                user_action = TemplateHelpFunctions.ReturnUserAction(page, pages);
                switch (user_action)
                {
                    case 1:
                        {
                            page++;
                            Console.Clear();
                        }
                        break;
                    case 2:
                        {
                            page--;
                            Console.Clear();
                        }
                        break;
                    case 3:
                        {
                            Add();
                            call_methods = true;
                            Console.Clear();
                        }
                        break;
                    case 4:
                        {
                            Update();
                            call_methods = true;
                            Console.Clear();
                        }
                        break;
                    case 5:
                        {
                            Delete();
                            call_methods = true;
                            Console.Clear();
                        }
                        break;
                    case 6:
                        {
                            //Search();
                            Console.Clear();
                        }
                        break;
                    case 7:
                        exit = true;
                        break;
                }
            }
        }

        private static void ShowNext()
        {
            customers = templateInterface.NextPackage();
            templateInterface.SetNewId(customers[customers.Count - 1].id, customers[0].id);
        }
        private static void ShowListCustomers()
        {
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", "N", "Company name", "Address", "Phone", "Full name customer");
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");

            foreach (Customers customersItem in customers)
            {
                Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", user_num, customersItem.customer_name, customersItem.address, customersItem.phone, customersItem.full_name_customer);
                Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
                user_num++;
            }
        }
        private static void ShowPrev()
        {
            customers = templateInterface.PrevPackage();
            templateInterface.SetNewId(customers[0].id, customers[customers.Count - 1].id);
        }

        private static void Add()
        {
            Customers customer = new Customers();
            Console.Write("Enter company name: ");
            customer.customer_name = Console.ReadLine();
            Console.Write("Enter address: ");
            customer.address = Console.ReadLine();
            Console.Write("Enter phone: ");
            customer.phone = Console.ReadLine();
            Console.Write("Enter full customer name: ");
            customer.full_name_customer = Console.ReadLine();

            all_customers = Repository.Instance.GetCount<Customers>();
            if (Repository.Instance.GetCount<Customers>() % BLOCK != 0)
                pages = (all_customers / BLOCK) + 1;
            else
                pages = all_customers / BLOCK;

            Repository.Instance.Create<Customers>(customer);
        }

        private static void Update()
        {
            bool end_update = false;
            int number = TemplateHelpFunctions.EnterNumber(1, all_customers);
            Customers customer = new Customers();
            customer = templateInterface.GetDataByNumber(number);
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", "N", "Company name", "Address", "Phone", "Full name customer");
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", number, customer.customer_name, customer.address, customer.phone, customer.full_name_customer);
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");

            while (!end_update)
            {
                Console.WriteLine("Enter name parametr for update. Enter \'Exit\' for save update and exit");
                string parametr = Console.ReadLine();
                Console.Write("Enter new " + parametr + ": ");
                if (parametr == "Company name")
                    customer.customer_name = Console.ReadLine();
                else if (parametr == "Address")
                    customer.address = Console.ReadLine();
                else if (parametr == "Phone")
                    customer.phone = Console.ReadLine();
                else if (parametr == "Full name customer")
                    customer.full_name_customer = Console.ReadLine();
                else if (parametr == "Exit")
                {
                    end_update = true;
                    Repository.Instance.Update(customer);
                }
                else
                    Console.WriteLine("Incorrect value!");
            }
        }

        private static void Delete()
        {
            int delete_number = TemplateHelpFunctions.EnterNumber(1, all_customers);
            Customers customer = new Customers();
            customer = templateInterface.GetDataByNumber(delete_number);

            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", "N", "Company name", "Address", "Phone", "Full name customer");
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", delete_number, customer.customer_name, customer.address, customer.phone, customer.full_name_customer);
            Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");

            Console.WriteLine("Delete this entry?[Yes/No]");
            string str = Console.ReadLine();
            if (str == "Yes")
            {
                Repository.Instance.Delete<Customers>(customer);
                all_customers = Repository.Instance.GetCount<Customers>();
                if (Repository.Instance.GetCount<Customers>() % BLOCK != 0)
                    pages = (all_customers / BLOCK) + 1;
                else
                    pages = all_customers / BLOCK;
            }
            else if (str == "No") { }
            else
                Console.WriteLine("Incorrect value!");
        }

        private static void Search()
        {
            bool exit = false;
            Query query = new Query();

            while (!exit)
            {
                Console.Write("[Search/Exit]: ");
                string action = Console.ReadLine();
                if (action == "Search")
                {
                    Console.Write("Enter text to search: ");
                    string str = Console.ReadLine();

                    str = TemplateHelpFunctions.UpdateStringForDateBase(str);
                    // TODO update sql quetions
                    string Sql = $"from Customers as e where" +
                         $" e.customer_name like '%{str}%'" +
                         $" or e.address like '%{str}%'" +
                         $" or e.phone like '%{str}%'" +
                         $" or e.full_name_customers like '%{str}%'";

                    query.Condition = Sql;

                    IList<Customers> customers_tmp = Repository.Instance.FindByCondition<Customers>(query);

                    user_num = 1;
                    Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
                    Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", "N", "Company name", "Address", "Phone", "Full name customer");
                    Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");

                    foreach (Customers customersItem in customers_tmp)
                    {
                        Console.WriteLine("|{0, -8}|{1, -40}|{2, -40}|{3, -20}|{4, -20}|", user_num, customersItem.customer_name, customersItem.address, customersItem.phone, customersItem.full_name_customer);
                        Console.WriteLine("+--------+----------------------------------------+----------------------------------------+--------------------+--------------------+");
                        user_num++;
                    }

                }
                else if (action == "Exit") { exit = true; }
                else
                    Console.WriteLine("Incorrect value!");
            }
        }
    }
}
