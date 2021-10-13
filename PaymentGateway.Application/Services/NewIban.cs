using PaymentGateway.Data;
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

        public NewIban(Database database)
        {
            //var database = Database.GetInstance();
            //var accounts = database.Accounts;//.ForEach(acc => acc.Iban);

            //List<String> ibans = accounts.ForEach(e =>  e.Iban);
            _database = database;
        }
        public string GetNewIban()
        {
            List<String> ibans = _database.Accounts.Select(x => x.IbanCode).ToList();

            if (ibans.Count() == 0)
                return "1";

            return (Int64.Parse(ibans.Last()) + 1).ToString();
        }
    }
}
