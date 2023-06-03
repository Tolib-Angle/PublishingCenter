using System;

namespace PublishingCenter_v2.PublishingCenter.Interface
{
    public class TemplateHelpFunctions
    {
        public static int EnterNumber(int start, int end)
        {
            string str;
            int res;
            while (true)
            {
                Console.Write("Enter number: ");
                str = Console.ReadLine();

                if (!int.TryParse(str, out var _))
                    Console.WriteLine("Incorrect value!");
                else
                {
                    res = Convert.ToInt32(str);
                    if (res < start || res > end)
                        Console.WriteLine("Incorrect value!");
                    else
                        return res;
                }
            }
        }
        public static int ReturnUserAction(int currentPage, int all_pages)
        {
            int result;
            if (currentPage == 1)
            {
                Console.WriteLine("1 - Next, 2 - Add, 3 - Update, 4 - Delete, 5 - Search, 6 - Exit");
                result = EnterNumber(1, 6);
                if (result != 1)
                    result++;
            }
            else if (currentPage == all_pages)
            {
                Console.WriteLine("1 - Prev, 2 - Add, 3 - Update, 4 - Delete, 5 - Search, 6 - Exit");
                result = EnterNumber(1, 6);
                result++;
            }
            else
            {
                Console.WriteLine("1 - Next, 2 - Prev, 3 - Add, 4 - Update, 5 - Delete, 6 - Search, 7 - Exit");
                result = EnterNumber(1, 7);
            }
            return result;
        }
        public static string UpdateStringForDateBase(string str)
        {
            string res = "";
            for(int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\'')
                {
                    res += str[i];
                    res += '\'';
                }
                else
                    res += str[i];

            }
            return res;
        }
        public static DateTime EnterDateTime(int start, int end)
        {
            Console.Write("Enter date time [year/month/day]: ");
            int year = EnterNumber(start, end);
            int month = EnterNumber(1, 12);
            int day = EnterNumber(1, 31);

            DateTime res = new DateTime(year, month, day);

            return res;
            
        }
    }
}
