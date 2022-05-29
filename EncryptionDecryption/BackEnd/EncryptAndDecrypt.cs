using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;
//using System.Security.Cryptography;

namespace EncryptDecrypt
{
    public class EncryptAndDecrypt
    {
        protected Guid Id { get; set; }
        protected DateTime IncomingDate { get; set; }

        protected Dictionary<string, string> LetterValues = new Dictionary<string, string>();
        Random random = new Random();
        Random random2 = new Random();
        protected string Alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdeefghijklmnopqrstuvwxyz!@#$%^&*()-+=~[]{}?'.,";
        protected int Numbers = 1234567890;
        //  RNGCryptoServiceProvider provider = RandomNumberGenerator.Create();
        // 1)Here we put into the LetterValues Dictionary the key (column name) and value that will go into the db
        protected void SetEncryptValues()
        {
            var Pk = Guid.NewGuid();
            Id = Pk;
            IncomingDate = DateTime.Today;
           
            foreach (var item in Alph)
            {
                if (item == 'a')
                {
                    break;
                }              
                    var numb = random.Next(3, 7);
                    char[] arr = new char[numb];
                    for (int i = 0; i < numb; i++)
                    {
                      arr[i] = Alph[random2.Next(Alph.Length)];
                    }
                     var value = new string(arr);
                if (char.IsDigit(item))
                {
                    int number = (int)char.GetNumericValue(item);
                    var toWord = number.ToWords();
                    LetterValues.Add(toWord, "'" + value + "'");
                }
                else
                {
                    string str = item.ToString();
                    LetterValues.Add(str, "'" + value + "'");
                }
                }
            
        }
        private string EncryptededEntry { get; set; }
        //Take a string and decrypt it
        protected string EncryptEntry(string entry)
        {
            var str = entry.ToUpper();
            foreach (var item in str)
            {
                if (item == 'A')
                {
                    EncryptededEntry += EncryptedValuesRepository.A;
                }
                else if (item == 'B')
                {
                    EncryptededEntry += EncryptedValuesRepository.B;
                }
                else if (item == 'C')
                {
                    EncryptededEntry += EncryptedValuesRepository.C;
                }
                else if (item == 'D')
                {
                    EncryptededEntry += EncryptedValuesRepository.D;
                }
                else if (item == 'E')
                {
                    EncryptededEntry += EncryptedValuesRepository.E;
                }
                else if (item == 'F')
                {
                    EncryptededEntry += EncryptedValuesRepository.F;
                }
                else if (item == 'G')
                {
                    EncryptededEntry += EncryptedValuesRepository.G;
                }
                else if (item == 'H')
                {
                    EncryptededEntry += EncryptedValuesRepository.H;
                }
                else if (item == 'I')
                {
                    EncryptededEntry += EncryptedValuesRepository.I;
                }
                else if (item == 'J')
                {
                    EncryptededEntry += EncryptedValuesRepository.J;
                }
                else if (item == 'K')
                {
                    EncryptededEntry += EncryptedValuesRepository.K;
                }
                else if (item == 'L')
                {
                    EncryptededEntry += EncryptedValuesRepository.L;
                }
                else if (item == 'M')
                {
                    EncryptededEntry += EncryptedValuesRepository.M;
                }
                else if (item == 'N')
                {
                    EncryptededEntry += EncryptedValuesRepository.N;
                }
                else if(item=='O')
                {
                    EncryptededEntry += EncryptedValuesRepository.O;
                }
                else if (item == 'P')
                {
                    EncryptededEntry += EncryptedValuesRepository.P;
                }
                else if (item == 'Q')
                {
                    EncryptededEntry += EncryptedValuesRepository.Q;
                }
                else if (item == 'R')
                {
                    EncryptededEntry += EncryptedValuesRepository.R;
                }
                else if (item == 'S')
                {
                    EncryptededEntry += EncryptedValuesRepository.S;
                }
                else if (item == 'T')
                {
                    EncryptededEntry += EncryptedValuesRepository.T;
                }
                else if (item == 'U')
                {
                    EncryptededEntry += EncryptedValuesRepository.U;
                }
                else if (item == 'V')
                {
                    EncryptededEntry += EncryptedValuesRepository.V;
                }
                else if (item == 'W')
                {
                    EncryptededEntry += EncryptedValuesRepository.W;
                }
                else if (item == 'X')
                {
                    EncryptededEntry += EncryptedValuesRepository.X;
                }
                else if (item == 'Y')
                {
                    EncryptededEntry += EncryptedValuesRepository.Y;
                }
                else if (item == 'Z')
                {
                    EncryptededEntry += EncryptedValuesRepository.Z;
                }
                else if (item == '1')
                {
                    EncryptededEntry += EncryptedValuesRepository.One;
                }
                else if (item == '2')
                {
                    EncryptededEntry += EncryptedValuesRepository.Two;
                }
                else if (item == '3')
                {
                    EncryptededEntry += EncryptedValuesRepository.Three;
                }
                else if (item == '4')
                {
                    EncryptededEntry += EncryptedValuesRepository.Four;
                }
                else if (item == '5')
                {
                    EncryptededEntry += EncryptedValuesRepository.Five;
                }
                else if (item == '6')
                {
                    EncryptededEntry += EncryptedValuesRepository.Six;
                }
                else if (item == '7')
                {
                    EncryptededEntry += EncryptedValuesRepository.Seven;
                }
                else if (item == '8')
                {
                    EncryptededEntry += EncryptedValuesRepository.Eight;
                }
                else if (item == '9')
                {
                    EncryptededEntry += EncryptedValuesRepository.Nine;
                }
                else if (item == '0')
                {
                    EncryptededEntry += EncryptedValuesRepository.Zero;
                }
            }
                return EncryptededEntry;
        }
        protected string PredecryptedEntry { get; set; }
       protected string DecryptedEntry { get; set; }
        protected DateTime DateOfInsertion { get; set; }
        private string Sub(string entry,int end)
        {
            if (entry.Length >= end)
            {
                return entry.Substring(0, end);
            }
            else
            {
                return entry;
            }
        }
        private bool CheckProperties(string str)
        {
              bool found = false;
           Type type = typeof(EncryptedValuesRepository);
                foreach (var item in type.GetProperties())
                {
                if(item.PropertyType!=typeof(string))
                {
                    break;
                }
                    string value = (string)item.GetValue(null, null);
                    if (value == str)
                    {
                        DecryptedEntry += item.Name;
                        found = true;
                        break;
                    }
                }
                return found;
        }
        protected void Decrypt(string entry)
        {
                int i = 0;
            int[] arr = { 3, 4, 5, 6 };
            while(entry.Length>0)
            {
                int index = arr[i];
            string sub = Sub(entry, index);
           var boo= CheckProperties(sub);
                if (boo == false)
                {
                    i++;
                }
                //The problem is that the replace removes 2 of the same values
                else if(boo==true && entry.Length>=index)
                {

                    //StringBuilder stringBuilder = new StringBuilder(entry);
                    //stringBuilder.Remove(0, index );
                    //entry = stringBuilder.ToString();
                    entry = entry.Remove(0, index);
                   // entry = entry.Substring(index);
                }
         
                if (i >3 && boo == false||index>entry.Length)
                {
                    i = 0;
                }
            }
        }
        protected string ToLower( string str)
        {
            DecryptedEntry= char.ToUpper(str[0]) + str.Substring(1).ToLower();
            return DecryptedEntry;
        }
      }
    }
