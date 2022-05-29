using EncryptDecrypt;
using System;
using System.Collections.Generic;


namespace EncryptionDecryption
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
           while (true)
            {
                using (DataConnection conn = new DataConnection())
                {
                    Console.WriteLine("Please type what you want to do");
                    string a= Console.ReadLine();
                    if (a.Contains("ins"))
                    {
                        conn.ExecuteInsertToDb();
                        Console.WriteLine("Please type the value");
                        string entry = Console.ReadLine();
                        conn.InsertEncryptyedVal(entry);
                    }

                    else if (a.Contains("rea"))
                    {
                        conn.DecryptData();
                         list = DataConnection.DecryptedNames;
                        foreach (var item in list)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
                
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                list.Clear();
                Console.Clear();
            }
        }
    }
}
