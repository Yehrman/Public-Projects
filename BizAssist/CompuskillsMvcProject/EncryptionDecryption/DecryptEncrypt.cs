using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EncryptDecrypt;
using MvcProjectDbConn;

namespace CompuskillsMvcProject.EncryptionDecryption
{
    public class DecryptEncrypt
    {
        DataConnection data = new DataConnection();
        EncryptAndDecrypt encrypt = new EncryptAndDecrypt();
       
        public Client Decrypt(Client client)
        {
           var date = client.DateAdded;
         client.FirstName=   data.DecryptData(client.FirstName,date);
          client.LastName=  data.DecryptData(client.LastName,date);
            client.Email = data.DecryptData(client.Email, date).ToLower();
         
        // client.PhoneNumber= data.DecryptData(client.PhoneNumber,date);
      //    client.Address= data.DecryptData(client.Address,date);
            return client;
        }
        public Client Encrypt(Client client)
        {
            client.FirstName = encrypt.EncryptEntry(client.FirstName);
            client.LastName = encrypt.EncryptEntry(client.LastName);
            client.Email = encrypt.EncryptEntry(client.Email);
         //   client.Address = encrypt.EncryptEntry(client.Address);
            return client;
        }
   
    }
}