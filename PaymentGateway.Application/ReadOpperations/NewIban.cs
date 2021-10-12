﻿using PaymentGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.ReadOpperations
{
    public class NewIban
    {
        private readonly Database _database;

        public static string GetNewIban()
        {
            var database = Database.GetInstance();
            var accounts = database.Accounts;//.ForEach(acc => acc.Iban);

            //List<String> ibans = accounts.ForEach(e =>  e.Iban);

            List<String> ibans = new List<String>();
            foreach (var acc in accounts)
            {
                ibans.Add(acc.IbanCode);
            }
            if (ibans.ToArray().Count() == 0)
                return "1";

            return (Int64.Parse(ibans.Last()) + 1).ToString();
        }
    }
}
