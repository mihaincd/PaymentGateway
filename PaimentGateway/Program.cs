using PaymentGateway.Application.WriteOpperations;
using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;

namespace PaimentGateway
{
    class Program
    {
        /// <summary>
        /// 1.verificare de stoc(ptr fiecare produs)
        /// 2.verificare fonduri in cont(total)
        /// 3.creare Tranzactie(1)
        /// 4.creare TranzactieXProdus (ptr fiecare produs)
        /// 5.save to database
        /// 6.emitere eveniment
        /// </summary>
       
        static void Main(string[] args)
        {
            Database database = Database.GetInstance();
            Console.WriteLine(database.Accounts.Count);

            EventSender eventSender = new EventSender();

            Account firstAccount = new Account();
            firstAccount.Balance = 100;

            EnrollCustomerCommand command = new EnrollCustomerCommand();
            command.Name = "Dorin";
            command.AccountType = "Deposit";
            command.Currency = "$";
            command.UniqueIdentifier = "1234556789";
            command.ClientType = "Individual";

            EnrollCustomerOperation enrolledClient = new EnrollCustomerOperation(eventSender);
            enrolledClient.PerformOperation(command);

            CreateAccountCommand newAccount = new CreateAccountCommand();
            newAccount.Status="Active";
            newAccount.Sold = 212;
            newAccount.Limit = 300;
            newAccount.ClientType = "Individual";
            newAccount.Cnp = "234567654321";
            newAccount.Curency = "$";
            newAccount.PersonId = 1;
            newAccount.IbanCode = "Ro2145";

            CreateAccountOperation createdAccount = new CreateAccountOperation(eventSender);
            createdAccount.PerformOperation(newAccount);


            DepositMoneyCommand depositMoney = new DepositMoneyCommand();
            depositMoney.DepositAmmount = 222;
            depositMoney.AcountId = 2;
            depositMoney.Curency = "$";
            depositMoney.DateOfOperation = DateTime.UtcNow;

            DepositMoneyOperation depositOperation = new DepositMoneyOperation(eventSender);
            depositOperation.PerformOperation(depositMoney);


            WithdrawMoneyCommand withdrawMoney = new WithdrawMoneyCommand();
            withdrawMoney.AcountId = 2;
            withdrawMoney.Curency = "$";
            withdrawMoney.WithdrawAmmount = 100;
            withdrawMoney.DateOfOperation = DateTime.UtcNow;

            WithdrawMoneyOperation withdrawOperation = new WithdrawMoneyOperation(eventSender);
            withdrawOperation.PerformOperation(withdrawMoney);


            CreateProductCommand newProduct = new CreateProductCommand();
            newProduct.Name = "Masline Verzi";
            newProduct.Value = 3;
            newProduct.Curency = "$";
            newProduct.Limit = 5;

            CreateProductOperation createProductOperation = new CreateProductOperation(eventSender);
            createProductOperation.PerformOperation(newProduct);


            PurchaseProductCommand newPurchaseProduct = new PurchaseProductCommand();
            newPurchaseProduct.IdAccount = newAccount.PersonId;
            newPurchaseProduct.Limit = 3;
            newPurchaseProduct.Name = "ciorbica";
            newPurchaseProduct.Quantity = 10;
            newPurchaseProduct.Value = 1;
            newPurchaseProduct.Curency = "$";

            PurchaseProductOperation purchaseProductOperation = new PurchaseProductOperation(eventSender);
            purchaseProductOperation.PerformOperation(newPurchaseProduct);
        }
    }
}
