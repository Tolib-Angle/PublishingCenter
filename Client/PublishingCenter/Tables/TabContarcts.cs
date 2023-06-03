using PublishingCenter_v2.PublishingCenter.Interface;
using PublishingCenter_v2.NHibernate.Data;
using PublishingCenter_v2.NHibernate;
using System.Collections.Generic;
using System;

namespace PublishingCenter_v2.PublishingCenter.Tables
{
    public class TabContarcts
    {
        private static int BLOCK = 10, pages, page = 1, user_num = 1;
        private static int user_action = -1, all_contracts = Repository.Instance.GetCount<Contracts>();
        private static TemplateInterface<Contracts> templateInterface = new TemplateInterface<Contracts>(BLOCK);
        private static TemplateInterface<Writers> templateInterfaceWriters = new TemplateInterface<Writers>(BLOCK);
        private static IList<Contracts> contracts;
        private static bool call_methods = false;
        private static void _init_()
        {
            page = 1;
            all_contracts = Repository.Instance.GetCount<Contracts>();
            user_action = -1;
            templateInterface.Reset();
            if (all_contracts % BLOCK != 0)
                pages = (all_contracts / BLOCK) + 1;
            else
                pages = all_contracts / BLOCK;
            call_methods = false;
        }
        public static void ContractsStart()
        {   
            templateInterface.Reset();
            if (all_contracts % BLOCK != 0)
                pages = (all_contracts / BLOCK) + 1;
            else
                pages = all_contracts / BLOCK;
            ContractsMenu();
        }
        private static void ContractsMenu()
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
                ShowListContracts();
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
                            call_methods = true;
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
            contracts = templateInterface.NextPackage();
            templateInterface.SetNewId(contracts[contracts.Count - 1].id, contracts[0].id);
        }
        private static void ShowListContracts()
        {
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", "N", "Contract N", "Writer", "Date C.", "Term", "Validy", "Date T.");
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");

            foreach (Contracts contractsItem in contracts)
            {
                Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", user_num, contractsItem.contract_number, (contractsItem.writer.name + " " + contractsItem.writer.surname + " " + contractsItem.writer.middle_name), contractsItem.date_of_terminition_contract.ToShortDateString(), contractsItem.term_of_the_contract, (contractsItem.validy_of_the_contract ? "True" : "False"), contractsItem.date_of_terminition_contract.ToShortDateString());
                Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
                user_num++;
            }
        }
        private static void ShowPrev()
        {
            contracts = templateInterface.PrevPackage();
            templateInterface.SetNewId(contracts[0].id, contracts[contracts.Count - 1].id);
        }
        private static void Update()
        {
            bool end_update = false;
            int number = TemplateHelpFunctions.EnterNumber(1, all_contracts);
            Contracts contract = new Contracts();
            contract = templateInterface.GetDataByNumber(number);
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", "N", "Contract N", "Writer", "Date C.", "Term", "Validy", "Date T.");
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", user_num, contract.contract_number, (contract.writer.name + " " + contract.writer.surname + " " + contract.writer.middle_name), contract.date_of_terminition_contract, contract.term_of_the_contract, (contract.validy_of_the_contract ? "True" : "False"), contract.date_of_terminition_contract);
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");

            while (!end_update)
            {
                Console.WriteLine("Enter name parametr for update. Enter \'Exit\' for save update and exit");
                string parametr = Console.ReadLine();
                if (parametr == "Contract N")
                    contract.contract_number = TemplateHelpFunctions.EnterNumber(10000000, 99999999);
                else if (parametr == "Writer")
                {
                    int new_id_writer = TemplateHelpFunctions.EnterNumber(1, Repository.Instance.GetCount<Writers>());
                    contract.writer = templateInterfaceWriters.GetDataByNumber(new_id_writer);
                }
                else if (parametr == "Date C.")
                    contract.date_of_terminition_contract = TemplateHelpFunctions.EnterDateTime(1950, 2050);
                else if (parametr == "Term")
                    contract.term_of_the_contract = TemplateHelpFunctions.EnterNumber(1, 99999999);
                else if (parametr == "Validy")
                    contract.validy_of_the_contract = TemplateHelpFunctions.EnterNumber(1, 2) == 1 ? true : false;
                else if (parametr == "Date T.")
                    contract.date_of_terminition_contract = TemplateHelpFunctions.EnterDateTime(1950, 2050);
                else if (parametr == "Exit")
                {
                    end_update = true;
                    Repository.Instance.Update(contract);
                }
                else
                    Console.WriteLine("Incorrect value!");
            }
        }
        private static void Delete()
        {
            int delete_number = TemplateHelpFunctions.EnterNumber(1, all_contracts);
            Contracts contract = new Contracts();
            contract = templateInterface.GetDataByNumber(delete_number);
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", "N", "Contract N", "Writer", "Date C.", "Term", "Validy", "Date T.");
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
            Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", delete_number, contract.contract_number, (contract.writer.name + " " + contract.writer.surname + " " + contract.writer.middle_name), contract.date_of_cons_contract.ToShortDateString(), contract.term_of_the_contract, (contract.validy_of_the_contract ? "True" : "False"), contract.date_of_terminition_contract.ToShortDateString());
            Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
            Console.WriteLine("Delete this entry?[Yes/No]");
            string str = Console.ReadLine();
            if (str == "Yes")
            {
                Repository.Instance.Delete<Contracts>(contract);
                all_contracts = Repository.Instance.GetCount<Contracts>();
                if (all_contracts % BLOCK != 0)
                    pages = (all_contracts / BLOCK) + 1;
                else
                    pages = all_contracts / BLOCK;
            }
            else if (str == "No") { }
            else
                Console.WriteLine("Incorrect value!");
        }

        private static void Add()
        {
            Contracts contract = new Contracts();
            contract.contract_number = TemplateHelpFunctions.EnterNumber(10000000, 99999999);
            Console.Write("Enter number writer. ");
            int id_writer = TemplateHelpFunctions.EnterNumber(1, Repository.Instance.GetCount<Writers>());
            contract.writer = templateInterfaceWriters.GetDataByNumber(id_writer);
            Console.Write("Enter date conclusions contract. ");
            contract.date_of_cons_contract = TemplateHelpFunctions.EnterDateTime(1950, 2050);
            Console.Write("Enter term contract. ");
            contract.term_of_the_contract = TemplateHelpFunctions.EnterNumber(1, 999999999);
            Console.Write("Enter validy contract [1 - True, 2 - False]. ");
            contract.validy_of_the_contract = TemplateHelpFunctions.EnterNumber(1, 2) == 1 ? true : false;
            Console.Write("Enter terminition contract. ");
            contract.date_of_terminition_contract = TemplateHelpFunctions.EnterDateTime(1950, 2050);

            all_contracts = Repository.Instance.GetCount<Contracts>();
            if (all_contracts % BLOCK != 0)
                pages = (all_contracts / BLOCK) + 1;
            else
                pages = all_contracts / BLOCK;

            Repository.Instance.Create<Contracts>(contract);
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
                    // TODO
                    string Sql = $"from Contracts as e where " +
                         (int.TryParse(str, out var _) ? $"e.contract_number = {str} or " : "") +
                         (DateTime.TryParse(str, out var _) ? $"e.date_of_cons_contract = '{str}' or " : "") +
                         (DateTime.TryParse(str, out var _) ? $"e.date_of_terminition_contract = '{str}' or " : "") +
                         (int.TryParse(str, out var _) ? $"e.term_of_the_contract = {str} or " : "") +
                         (str == "true" ? $"e.validy_of_the_contract = {str} or " : "") +
                         (str == "false" ? $"e.validy_of_the_contract = {str} or " : "") +
                         $" e.writer.name like '%{str}%'" +
                         $" or e.writer.surname like '%{str}%'" +
                         $" or e.writer.middle_name like '%{str}%'";

                    query.Condition = Sql;

                    IList<Contracts> contracts_tmp = Repository.Instance.FindByCondition<Contracts>(query);

                    user_num = 1;
                    Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
                    Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", "N", "Contract N", "Writer", "Date C.", "Term", "Validy", "Date T.");
                    Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");

                    foreach (Contracts contractsItem in contracts_tmp)
                    {
                        Console.WriteLine("|{0, -8}|{1, -12}|{2, -35}|{3, -12}|{4, -12}|{5, -12}|{6, -12}|", user_num, contractsItem.contract_number, (contractsItem.writer.name + " " + contractsItem.writer.surname + " " + contractsItem.writer.middle_name), contractsItem.date_of_terminition_contract.ToShortDateString(), contractsItem.term_of_the_contract, (contractsItem.validy_of_the_contract ? "True" : "False"), contractsItem.date_of_terminition_contract.ToShortDateString());
                        Console.WriteLine("+--------+------------+-----------------------------------+------------+------------+------------+------------+");
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
