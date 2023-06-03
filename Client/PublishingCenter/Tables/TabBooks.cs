using System;
using System.Collections.Generic;
using PublishingCenter_v2.NHibernate;
using PublishingCenter_v2.NHibernate.Data;
using PublishingCenter_v2.PublishingCenter.Interface;
using System.Linq;

namespace PublishingCenter_v2.PublishingCenter.Tables
{
    public class TabBooks
    {
        private static int BLOCK = 10, pages, page = 1, user_num = 1;
        private static int user_action = -1, all_books = Repository.Instance.GetCount<Books>();
        private static TemplateInterface<Books> templateInterface = new TemplateInterface<Books>(BLOCK);
        private static TemplateInterface<Writers> templateInterfaceWriters = new TemplateInterface<Writers>(BLOCK);
        private static IList<Books> books;
        private static bool call_methods = false;
        private static void _init_()
        {
            page = 1;
            all_books = Repository.Instance.GetCount<Books>();
            user_action = -1;
            templateInterface.Reset();
            if (all_books % BLOCK != 0)
                pages = (all_books / BLOCK) + 1;
            else
                pages = all_books / BLOCK;
            call_methods = false;
        }
        public static void BooksStart()
        {
            templateInterface.Reset();
            if (all_books % BLOCK != 0)
                pages = (all_books / BLOCK) + 1;
            else
                pages = all_books / BLOCK;
            BooksMenu();
        }

        private static void BooksMenu()
        {
            bool exit = false;
            while (!exit)
            {
                if (call_methods)
                {
                    _init_();
                }
                user_num = BLOCK * (page - 1) + 1;
                if (user_action != 2 || user_action == -1)
                    ShowNext();
                else if (user_action == 2)
                    ShowPrev();
                ShowListBooks();
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
            books = templateInterface.NextPackage();
            templateInterface.SetNewId(books[books.Count - 1].id, books[0].id);
        }

        private static void ShowPrev()
        {
            
            books = templateInterface.PrevPackage();
            templateInterface.SetNewId(books[0].id, books[books.Count - 1].id);
        }
        private static string AllWriters(IList<Writers> writers)
        {
            string res = "";
            foreach (Writers writersItem in writers)
            {
                res += writersItem.name;
                res += " ";
                res += writersItem.surname[0];
                res += ". ";
                res += writersItem.middle_name[0];
                res += ". ";
            }
            return res;
        }
        private static void ShowListBooks()
        {
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", "N", "Cepher book", "Pub. name", "Title", "Circulation", "Date R.", "Cost price", "Sale price", "Fee", "Authors");
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");

            foreach (Books booksItem in books)
            {
                string all_writers = AllWriters(booksItem.writers);
                Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", user_num, booksItem.cipher_of_the_book, booksItem.name, booksItem.title, booksItem.circulation, booksItem.release_date.ToShortDateString(), booksItem.cost_price, booksItem.sale_price, booksItem.fee, all_writers);
                Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
                user_num++;
            }
        }
        private static void Delete()
        {
            int delete_number = TemplateHelpFunctions.EnterNumber(1, all_books);
            Books booksItem = new Books();
            booksItem = templateInterface.GetDataByNumber(delete_number);
            string all_writers = AllWriters(booksItem.writers);
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", "N", "Cepher book", "Pub. name", "Title", "Circulation", "Date R.", "Cost price", "Sale price", "Fee", "Authors");
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", delete_number, booksItem.cipher_of_the_book, booksItem.name, booksItem.title, booksItem.circulation, booksItem.release_date.ToShortDateString(), booksItem.cost_price, booksItem.sale_price, booksItem.fee, all_writers);
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");

            Console.WriteLine("Delete this entry?[Yes/No]");
            string str = Console.ReadLine();
            if (str == "Yes")
            {
                Repository.Instance.Delete<Books>(booksItem);
                all_books = Repository.Instance.GetCount<Books>();
                if (Repository.Instance.GetCount<Books>() % BLOCK != 0)
                    pages = (all_books / BLOCK) + 1;
                else
                    pages = all_books / BLOCK;
            }
            else if (str == "No") { }
            else
                Console.WriteLine("Incorrect value!");
        }
        private static void Update()
        {
            bool end_update = false;
            int number = TemplateHelpFunctions.EnterNumber(1, all_books);
            Books booksItem = new Books();
            booksItem = templateInterface.GetDataByNumber(number);
            string all_writers = AllWriters(booksItem.writers);
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", "N", "Cepher book", "Pub. name", "Title", "Circulation", "Date R.", "Cost price", "Sale price", "Fee", "Authors");
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", number, booksItem.cipher_of_the_book, booksItem.name, booksItem.title, booksItem.circulation, booksItem.release_date.ToShortDateString(), booksItem.cost_price, booksItem.sale_price, booksItem.fee, all_writers);
            Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");

            while (!end_update)
            {
                Console.WriteLine("Enter name parametr for update. Enter \'Exit\' for save update and exit");
                string parametr = Console.ReadLine();
                Console.Write("Enter new " + parametr + ": ");
                if (parametr == "Cepher book")
                    booksItem.cipher_of_the_book = TemplateHelpFunctions.EnterNumber(10000000, 99999999);
                else if (parametr == "Pub. name")
                    booksItem.name = Console.ReadLine();
                else if (parametr == "Title")
                    booksItem.title = Console.ReadLine();
                else if (parametr == "Circulation")
                    booksItem.circulation = TemplateHelpFunctions.EnterNumber(1, 10000000);
                else if (parametr == "Date R.")
                    booksItem.release_date = TemplateHelpFunctions.EnterDateTime(1950, 2050);
                else if (parametr == "Cost price")
                    booksItem.cost_price = (float)TemplateHelpFunctions.EnterNumber(1, 10000000);
                else if (parametr == "Sale price")
                    booksItem.sale_price = (float)TemplateHelpFunctions.EnterNumber(1, 10000000);
                else if (parametr == "Fee")
                    booksItem.fee = (float)TemplateHelpFunctions.EnterNumber(1, 10000000);
                else if (parametr == "Authors")
                {
                    bool ext = false;
                    while (!ext)
                    {
                        Console.Write("[Add/Delete/Exit]");
                        string str = Console.ReadLine();
                        if (str == "Add")
                        {
                            booksItem.writers.Add(templateInterfaceWriters.GetDataByNumber(TemplateHelpFunctions.EnterNumber(1, Repository.Instance.GetCount<Writers>())));
                        }
                        else if (str == "Delete")
                        {
                            booksItem.writers.Remove(templateInterfaceWriters.GetDataByNumber(TemplateHelpFunctions.EnterNumber(1, Repository.Instance.GetCount<Writers>())));
                        }
                        else if (str == "Exit")
                            ext = true;
                        else
                            Console.WriteLine("Incorrect value!");
                    }
                }
                else if (parametr == "Exit")
                {
                    end_update = true;
                    Repository.Instance.Update(booksItem);
                }
                else
                    Console.WriteLine("Incorrect value!");
            }
        }
        private static void Add()
        {
            Books book = new Books();
            Console.Write("Enter cepher book. ");
            book.cipher_of_the_book = TemplateHelpFunctions.EnterNumber(10000000, 99999999);
            Console.Write("Enter publishing name: ");
            book.name = Console.ReadLine();
            Console.Write("Enter title: ");
            book.title = Console.ReadLine();
            Console.Write("Enter cerculation. ");
            book.circulation = TemplateHelpFunctions.EnterNumber(1, 999999999);
            Console.Write("Enter date release. ");
            book.release_date = TemplateHelpFunctions.EnterDateTime(1950, 2050);
            Console.Write("Enter cost price. ");
            book.cost_price = (float)TemplateHelpFunctions.EnterNumber(1, 9999999);
            Console.Write("Enter sale price. ");
            book.sale_price = (float)TemplateHelpFunctions.EnterNumber(1, 9999999);
            Console.Write("Enter fee. ");
            book.fee = (float)TemplateHelpFunctions.EnterNumber(1, 9999999);
            bool exit = false;
            int i = 0;
            while (!exit)
            {
                Console.Write("Enter [Add/Exit]");
                string str = Console.ReadLine();

                if (str == "Add")
                {
                    i++;
                    int writer = TemplateHelpFunctions.EnterNumber(1, Repository.Instance.GetCount<Writers>());
                    book.writers.Add(templateInterfaceWriters.GetDataByNumber(writer));
                }
                else if (str == "Exit" && i != 0)
                    exit = true;
                else
                    Console.WriteLine("Incorrect value or enter min 1 authors!");

            }

            all_books = Repository.Instance.GetCount<Books>();
            if (Repository.Instance.GetCount<Books>() % BLOCK != 0)
                pages = (all_books / BLOCK) + 1;
            else
                pages = all_books / BLOCK;

            Repository.Instance.Create<Books>(book);
        }
        private static void Search()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Write("[Search/Exit]: ");
                string action = Console.ReadLine();
                if (action == "Search")
                {
                    Query query = new Query();
                    Console.Write("Enter text to search: ");
                    string str = Console.ReadLine();

                    str = TemplateHelpFunctions.UpdateStringForDateBase(str);

                    string Sql = $"from Books as e where " +
                         (int.TryParse(str, out var _) ? $"e.cepher_of_the_book = {str} or " : "") +
                         (int.TryParse(str, out var _) ? $"e.cost_price = {str} or " : "") +
                         (int.TryParse(str, out var _) ? $"e.sale_price = {str} or " : "") +
                         (int.TryParse(str, out var _) ? $"e.fee = {str} or " : "") +
                         (int.TryParse(str, out var _) ? $"e.release_date = '{str}' or " : "") +
                         (DateTime.TryParse(str, out var _) ? $"e.date_of_terminition_contract = '{str}' or " : "") +
                         $" e.name like '%{str}%'" +
                         $" or e.title like '%{str}%'";

                    query.Condition = Sql;

                    IList<Books> books_tmp = Repository.Instance.FindByCondition<Books>(query);

                    user_num = 1;
                    Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
                    Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", "N", "Cepher book", "Pub. name", "Title", "Circulation", "Date R.", "Cost price", "Sale price", "Fee", "Authors");
                    Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");

                    foreach (Books booksItem in books_tmp)
                    {
                        string all_writers = AllWriters(booksItem.writers);
                        Console.WriteLine("|{0, -8}|{1, -12}|{2, -40}|{3, -40}|{4, -12}|{5, -12}|{6, -12}|{7, -12}|{8, -12}|{9, -50}|", user_num, booksItem.cipher_of_the_book, booksItem.name, booksItem.title, booksItem.circulation, booksItem.release_date.ToShortDateString(), booksItem.cost_price, booksItem.sale_price, booksItem.fee, all_writers);
                        Console.WriteLine("+--------+------------+----------------------------------------+----------------------------------------+------------+------------+------------+------------+------------+--------------------------------------------------+");
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
