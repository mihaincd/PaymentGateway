using Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using PaymentGateway.Application.WriteOpperations;
using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.IO;
using static PaymentGateway.PublishedLanguage.WriteSide.PurchaseProductCommand;

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
        static IConfiguration Configuration;

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            //setup
            var services =new ServiceCollection();
            services.RegisterBusinessServices(Configuration);

            services.AddSingleton<IEventSender, EventSender>();
            services.AddSingleton(Configuration);

            //build
            var serviceProvider = services.BuildServiceProvider();

            //use


            Database database = Database.GetInstance();
            Console.WriteLine(database.Accounts.Count);

            EventSender eventSender = new EventSender();

            Account firstAccount = new Account();
            firstAccount.Balance = 100;

            EnrollCustomerCommand command = new EnrollCustomerCommand
            {
                Name = "Dorin",
                AccountType = "Deposit",
                Currency = "$",
                UniqueIdentifier = "1234556789",
                ClientType = "Individual"
            };

            EnrollCustomerOperation enrolledClient = serviceProvider.GetRequiredService<EnrollCustomerOperation>();
            enrolledClient.PerformOperation(command);

            CreateAccountCommand newAccount = new CreateAccountCommand
            {
                Status = "Active",
                Sold = 212,
                Limit = 300,
                ClientType = "Individual",
                Cnp = "234567654321",
                Curency = "$",
                PersonId = 1,
                IbanCode = "Ro2145"
            };

            CreateAccountOperation createdAccount = serviceProvider.GetRequiredService<CreateAccountOperation>();
            createdAccount.PerformOperation(newAccount);


            DepositMoneyCommand depositMoney = new DepositMoneyCommand
            {
                DepositAmmount = 222,
                AcountId = 2,
                Curency = "$",
                DateOfOperation = DateTime.UtcNow
            };

            DepositMoneyOperation depositOperation = serviceProvider.GetRequiredService<DepositMoneyOperation>();
            depositOperation.PerformOperation(depositMoney);


            WithdrawMoneyCommand withdrawMoney = new WithdrawMoneyCommand
            {
                AcountId = 2,
                Curency = "$",
                WithdrawAmmount = 100,
                DateOfOperation = DateTime.UtcNow
            };

            WithdrawMoneyOperation withdrawOperation = serviceProvider.GetRequiredService<WithdrawMoneyOperation>();
            withdrawOperation.PerformOperation(withdrawMoney);


            CreateProductCommand newProduct = new CreateProductCommand
            {
                Name = "Masline Verzi",
                Value = 3,
                Curency = "$",
                Limit = 5
            };

            CreateProductOperation createProductOperation = serviceProvider.GetRequiredService<CreateProductOperation>();
            createProductOperation.PerformOperation(newProduct);


            PurchaseProductCommand purchaseProductCommand = new PurchaseProductCommand
            {
                IbanCode = "69RO69INGB4204206969",
                UniqueIdentifier = "2970304234566",
                ProductDetails = new List<PurchaseProductDetail>
            {
                new PurchaseProductDetail { ProductId = newProduct.IdProduct, Quantity = 3 }
            }
            };
            PurchaseProductOperation purchaseProductOperation = serviceProvider.GetRequiredService<PurchaseProductOperation>();
            purchaseProductOperation.PerformOperation(purchaseProductCommand);
        }
    }
}
