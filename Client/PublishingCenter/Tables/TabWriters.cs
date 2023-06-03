using PublishingCenter_v2.PublishingCenter.Interface;
using PublishingCenter_v2.NHibernate.Data;
using PublishingCenter_v2.NHibernate;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PublishingCenter_v2.PublishingCenter.Tables
{
    public class TabWriters
    {
        private static int BLOCK = 10, pages, page = 1, user_num = 1;
        private static int user_action = -1, all_writers = Repository.Instance.GetCount<Writers>();
        private static TemplateInterface<Writers> templateInterface = new TemplateInterface<Writers>(BLOCK);
        private static IList<Writers> writers;
        private static bool call_methods = false;
        private static void _init_()
        {
            page = 1;
            all_writers = Repository.Instance.GetCount<Writers>();
            user_action = -1;
            templateInterface.Reset();
            if (all_writers % BLOCK != 0)
                pages = (all_writers / BLOCK) + 1;
            else
                pages = all_writers / BLOCK;
            call_methods = false;
        }
        public static void WritersStart()
        {
            templateInterface.Reset();
            if (all_writers % BLOCK != 0)
                pages = (all_writers / BLOCK) + 1;
            else
                pages = all_writers / BLOCK;
            WritersMenu();
        }
        private static void WritersMenu()
        {
            bool exit = false;
            while (!exit)
            {
                if (call_methods)
                {
                    _init_();
                }
                if (user_action == 1 || user_action == -1)
                    ShowNext();
                else if (user_action == 2)
                    ShowPrev();
                ShowListWriters();
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
            writers = templateInterface.NextPackage();
            templateInterface.SetNewId(writers[writers.Count - 1].id, writers[0].id);
        }
        private static void ShowListWriters()
        {
            user_num = BLOCK * (page - 1) + 1;
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", "N", "Passport N", "Surname", "Name", "Middle Name", "Address", "Phone");
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");

            foreach (Writers writersItem in writers)
            {
                Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", user_num, writersItem.passport_number, writersItem.surname, writersItem.name, writersItem.middle_name, writersItem.address, writersItem.phone);
                Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
                user_num++;
            }
        }
        private static void ShowPrev()
        {
            writers = templateInterface.PrevPackage();
            templateInterface.SetNewId(writers[writers.Count - 1].id, writers[0].id);
        }
        private static void Update()
        {
            bool end_update = false;
            int number = TemplateHelpFunctions.EnterNumber(1, all_writers);
            Writers writer = new Writers();
            writer = templateInterface.GetDataByNumber(number);
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", "N", "Passport N", "Surname", "Name", "Middle Name", "Address", "Phone");
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", number, writer.passport_number, writer.surname, writer.name, writer.middle_name, writer.address, writer.phone);
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");

            while (!end_update)
            {
                Console.WriteLine("Enter name parametr for update. Enter \'Exit\' for save update and exit");
                string parametr = Console.ReadLine();
                Console.Write("Enter new " + parametr + ": ");
                if (parametr == "Passport N")
                    writer.passport_number = TemplateHelpFunctions.EnterNumber(10000000, 99999999);
                else if (parametr == "Surname")
                    writer.surname = Console.ReadLine();
                else if (parametr == "Name")
                    writer.name = Console.ReadLine();
                else if (parametr == "Middle Name")
                    writer.middle_name = Console.ReadLine();
                else if (parametr == "Address")
                    writer.address = Console.ReadLine();
                else if (parametr == "Phone")
                    writer.phone = Console.ReadLine();
                else if (parametr == "Exit")
                {
                    end_update = true;
                    Repository.Instance.Update(writer);
                }
                else
                    Console.WriteLine("Incorrect value!");
            }
        }
        private static void Delete()
        {
            int delete_number = TemplateHelpFunctions.EnterNumber(1, all_writers);
            Writers writer = new Writers();
            writer = templateInterface.GetDataByNumber(delete_number);
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", "N", "Passport N", "Surname", "Name", "Middle Name", "Address", "Phone");
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", delete_number, writer.passport_number, writer.surname, writer.name, writer.middle_name, writer.address, writer.phone);
            Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
            Console.WriteLine("Delete this entry?[Yes/No]");
            string str = Console.ReadLine();
            if (str == "Yes")
            {
                Repository.Instance.Delete<Writers>(writer);
                all_writers = Repository.Instance.GetCount<Writers>();
                if (Repository.Instance.GetCount<Writers>() % BLOCK != 0)
                    pages = (all_writers / BLOCK) + 1;
                else
                    pages = all_writers / BLOCK;
            }
            else if (str == "No") { }
            else
                Console.WriteLine("Incorrect value!");
        }

        private static void Add()
        {
            Writers writer = new Writers();
            writer.passport_number = TemplateHelpFunctions.EnterNumber(10000000, 99999999);
            Console.Write("Enter surname: ");
            writer.surname = Convert.ToString(Console.ReadLine());
            Console.Write("Enter name: ");
            writer.name = Convert.ToString(Console.ReadLine());
            Console.Write("Enter middle name: ");
            writer.middle_name = Convert.ToString(Console.ReadLine());
            Console.Write("Enter address: ");
            writer.address = Convert.ToString(Console.ReadLine());
            Console.Write("Enter phone: ");
            writer.phone = Convert.ToString(Console.ReadLine());

            all_writers = Repository.Instance.GetCount<Writers>();
            if (Repository.Instance.GetCount<Writers>() % BLOCK != 0)
                pages = (all_writers / BLOCK) + 1;
            else
                pages = all_writers / BLOCK;

            Repository.Instance.Create<Writers>(writer);
        }

        private static void Search()
        {
            bool exit = false;

            while (!exit)
            {
                Query query = new Query();
                Console.Write("[Search/Exit]: ");
                string action = Console.ReadLine();
                if (action == "Search")
                {
                    Console.Write("Enter text to search: ");
                    string str = Console.ReadLine();

                    str = TemplateHelpFunctions.UpdateStringForDateBase(str);

                    string Sql = $"surname ilike '%{str}%' or " +
                         (int.TryParse(str, out var _) ? $" passport_number = {str} or " : "") +
                         $" or name ilike '%{str}%'" +
                         $" or middle_name ilike '%{str}%'" +
                         $" or address ilike '%{str}%'";

                    query.Condition = Sql;

                    IList<Writers> writers_tmp = Repository.Instance.FindByCondition<Writers>(query);

                    user_num = 1;
                    Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
                    Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", "N", "Passport N", "Surname", "Name", "Middle Name", "Address", "Phone");
                    Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");

                    foreach (Writers writersItem in writers_tmp)
                    {
                        Console.WriteLine("|{0, -8}|{1, -12}|{2, -20}|{3, -20}|{4, -20}|{5, -40}|{6, -20}|", user_num, writersItem.passport_number, writersItem.surname, writersItem.name, writersItem.middle_name, writersItem.address, writersItem.phone);
                        Console.WriteLine("+-------+-------------+--------------------+--------------------+--------------------+----------------------------------------+--------------------+");
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
