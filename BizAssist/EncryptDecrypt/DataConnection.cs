using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Data;
using System.Diagnostics;

namespace EncryptDecrypt
{
    public class DataConnection : EncryptAndDecrypt, IDisposable
    {
        //
        SqlConnection Conn = new SqlConnection(@"Data Source =wdb3.my-hosting-panel.com ; Initial Catalog=bizassis_encryptionvalues;Persist Security Info=False;User ID=bizassis_admin;Password=TrustinHashem1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
        //Stteps 1,2,3
        public void ExecuteInsertToDb()
        {
            RetrieveEncryptionValues();
            var date = EncryptedValuesRepository.DateInserted;
            //Change to monthly
            if (date == null || DateTime.Today >= date.AddDays(1))
            {
                //Going foward it may make sense to set the properties straight from SetEncryptValues() and not rerun RetrieveEncryptionValues();
                SetEncryptValues();
                InsertValues();
                Debug.WriteLine("Database updated fetching mew values");
                RetrieveEncryptionValues();
            }
        }
        //2) Here we insert the encrypted values into the db
        private void InsertValues()
        {
            Conn.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = Conn;
            string key = "";
            string value = "";
            foreach (var item in LetterValues)
            {
                var last = LetterValues.Last();
                if (item.Key == last.Key)
                {
                    key += item.Key;
                    value += item.Value;
                }
                else
                {
                    key += item.Key + ",";
                    value += item.Value + ',';
                }
            }

            command.CommandText = $"Insert into EncryptedValues (Id, {key},DateInserted) VALUES ('{Id}',{value},'{IncomingDate}')";
            command.ExecuteNonQuery();
            Conn.Close();
        }
        private void RetrieveEncryptionValues(DateTime? date = null)
        {
            //We need to change the query to monthly
            date = date ?? DateTime.Today;
            Conn.Open();
            SqlCommand command = new SqlCommand($"Select top 1   * from EncryptedValues where DateInserted <= Convert (date,'{date}' ) order by DateInserted desc ", Conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
               // EncryptedValuesRepository.Id = reader.GetGuid(0);
                EncryptedValuesRepository.A = reader.GetString(1);
                EncryptedValuesRepository.B = reader.GetString(2);
                EncryptedValuesRepository.C = reader.GetString(3);
                EncryptedValuesRepository.D = reader.GetString(4);
                EncryptedValuesRepository.E = reader.GetString(5);
                EncryptedValuesRepository.F = reader.GetString(6);
                EncryptedValuesRepository.G = reader.GetString(7);
                EncryptedValuesRepository.H = reader.GetString(8);
                EncryptedValuesRepository.I = reader.GetString(9);
                EncryptedValuesRepository.J = reader.GetString(10);
                EncryptedValuesRepository.K = reader.GetString(11);
                EncryptedValuesRepository.L = reader.GetString(12);
                EncryptedValuesRepository.M = reader.GetString(13);
                EncryptedValuesRepository.N = reader.GetString(14);
                EncryptedValuesRepository.O = reader.GetString(15);
                EncryptedValuesRepository.P = reader.GetString(16);
                EncryptedValuesRepository.Q = reader.GetString(17);
                EncryptedValuesRepository.R = reader.GetString(18);
                EncryptedValuesRepository.S = reader.GetString(19);
                EncryptedValuesRepository.T = reader.GetString(20);
                EncryptedValuesRepository.U = reader.GetString(21);
                EncryptedValuesRepository.V = reader.GetString(22);
                EncryptedValuesRepository.W = reader.GetString(23);
                EncryptedValuesRepository.X = reader.GetString(24);
                EncryptedValuesRepository.Y = reader.GetString(25);
                EncryptedValuesRepository.Z = reader.GetString(26);
                EncryptedValuesRepository.Zero = reader.GetString(27);            
                EncryptedValuesRepository.One = reader.GetString(28);
                EncryptedValuesRepository.Two = reader.GetString(29);
                EncryptedValuesRepository.Three = reader.GetString(30);
                EncryptedValuesRepository.Four = reader.GetString(31);
                EncryptedValuesRepository.Five = reader.GetString(32);
                EncryptedValuesRepository.Six = reader.GetString(33);
                EncryptedValuesRepository.Seven = reader.GetString(34);
                EncryptedValuesRepository.Eight = reader.GetString(35);
                EncryptedValuesRepository.Nine = reader.GetString(36);
                EncryptedValuesRepository.At = reader.GetString(37);
                EncryptedValuesRepository.Dot = reader.GetString(38);
                EncryptedValuesRepository.DateInserted = reader.GetDateTime(39);
            }
            Conn.Close();
        }

        internal static DataTable Entries = new DataTable();
        public static List<string> DecryptedNames = new List<string>();


        public string DecryptData(string entry,DateTime? date=null )
        {
            date = date ?? DateTime.Today;
            //  RetrieveEntriesFromDb;
            if (date!=EncryptedValuesRepository.DateInserted)
            {
                RetrieveEncryptionValues(date);
            }
             Decrypt(entry);
          
            string str=    ToLower(DecryptedEntry);
            return str;         
        }
        public void Dispose()
        {
            Conn.Dispose();
        }
    }
}
