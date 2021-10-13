using Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using PaymentGateway.Application.ReadOpperations;
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
            var services = new ServiceCollection();
            services.RegisterBusinessServices(Configuration);

            services.AddSingleton<IEventSender, EventSender>();
            services.AddSingleton(Configuration);

            //build
            var serviceProvider = services.BuildServiceProvider();
            var database = serviceProvider.GetRequiredService<Database>();
            var ibanService = serviceProvider.GetRequiredService<NewIban>();

            //use


            //Database database = Database.GetInstance();
            Console.WriteLine(database.Accounts.Count);

            //EventSender eventSender = new EventSender();

            Account firstAccount = new();
            firstAccount.Balance = 100;

            EnrollCustomerCommand command = new()
            {
                Name = "Dorin",
                AccountType = "Deposit",
                Currency = "$",
                UniqueIdentifier = "1234556789",
                ClientType = "Individual"
            };

            EnrollCustomerOperation enrolledClient = serviceProvider.GetRequiredService<EnrollCustomerOperation>();
            enrolledClient.PerformOperation(command);

            CreateAccountCommand newAccount = new()
            {
                Status = "Active",
                Sold = 212,
                Limit = 300,
                ClientType = "Individual",
                Cnp = "234567654321",
                Curency = "$",
                PersonId = 1,
                IbanCode = (Int64.Parse(ibanService.GetNewIban()) - 1).ToString()
            };

            CreateAccountOperation createdAccount = serviceProvider.GetRequiredService<CreateAccountOperation>();
            createdAccount.PerformOperation(newAccount);


            DepositMoneyCommand depositMoney = new()
            {
                DepositAmmount = 222,
                AcountId = 2,
                Curency = "$",
                DateOfOperation = DateTime.UtcNow
            };

            DepositMoneyOperation depositOperation = serviceProvider.GetRequiredService<DepositMoneyOperation>();
            depositOperation.PerformOperation(depositMoney);


            WithdrawMoneyCommand withdrawMoney = new()
            {
                AcountId = 2,
                Curency = "$",
                WithdrawAmmount = 100,
                DateOfOperation = DateTime.UtcNow
            };

            WithdrawMoneyOperation withdrawOperation = serviceProvider.GetRequiredService<WithdrawMoneyOperation>();
            withdrawOperation.PerformOperation(withdrawMoney);


            CreateProductCommand newProduct = new()
            {
                Name = "Masline Verzi",
                Value = 3,
                Curency = "$",
                Limit = 5
            };

            CreateProductOperation createProductOperation = serviceProvider.GetRequiredService<CreateProductOperation>();
            createProductOperation.PerformOperation(newProduct);


            PurchaseProductCommand purchaseProductCommand = new()
            {
                IbanCode = (Int64.Parse(ibanService.GetNewIban()) - 1).ToString(),
                UniqueIdentifier = "2970304234566",
                ProductDetails = new List<PurchaseProductDetail>
            {
                new PurchaseProductDetail { ProductId = newProduct.IdProduct, Quantity = 3 }
            }
            };
            PurchaseProductOperation purchaseProductOperation = serviceProvider.GetRequiredService<PurchaseProductOperation>();
            purchaseProductOperation.PerformOperation(purchaseProductCommand);


            var query = new ListOfAccounts.Query
            {
                PersonId = 1
            };
            var handler = serviceProvider.GetRequiredService<ListOfAccounts.QueryHandler>();
            var result = handler.PerformOperation(query);

        }
    }
}
