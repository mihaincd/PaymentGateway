using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.ReadOpperations;
using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using PaymentGateway.PublishedLanguage.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static PaymentGateway.PublishedLanguage.Commands.PurchaseProductCommand;
using PaymentGateway;

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

        static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            //setup
            var services = new ServiceCollection();

            var source =new CancellationTokenSource();
            var cancellationToken = source.Token;
            services.RegisterBusinessServices(Configuration);

            services.Scan(scan => scan
               .FromAssemblyOf<ListOfAccounts>()
               .AddClasses(classes => classes.AssignableTo<IValidator>())
               .AsImplementedInterfaces()
               .WithScopedLifetime());


            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(ValidationPreProcessor<>));

            services.AddScopedContravariant<INotificationHandler<INotification>, AllEventsHandler>(typeof(CustomerEnrolled).Assembly);


            services.AddMediatR(new[] { typeof(ListOfAccounts).Assembly, typeof(AllEventsHandler).Assembly }); // get all IRequestHandler and INotificationHandler classes


            //services.AddSingleton<IEventSender, EventSender>();
            services.AddSingleton(Configuration);

            //build
            var serviceProvider = services.BuildServiceProvider();
            var database = serviceProvider.GetRequiredService<Database>();
            var ibanService = serviceProvider.GetRequiredService<NewIban>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

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
            //EnrollCustomerOperation enrolledClient = serviceProvider.GetRequiredService<EnrollCustomerOperation>();
            //enrolledClient.Handle(command, default).GetAwaiter().GetResult();
            await mediator.Send(command, cancellationToken);

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
            //CreateAccountOperation createdAccount = serviceProvider.GetRequiredService<CreateAccountOperation>();
            //createdAccount.Handle(newAccount, default).GetAwaiter().GetResult();
            await mediator.Send(newAccount, cancellationToken);


            DepositMoneyCommand depositMoney = new()
            {
                DepositAmmount = 222,
                AcountId = 2,
                Curency = "$",
                DateOfOperation = DateTime.UtcNow
            };
            //DepositMoneyOperation depositOperation = serviceProvider.GetRequiredService<DepositMoneyOperation>();
            //depositOperation.Handle(depositMoney, default).GetAwaiter().GetResult();
            await mediator.Send(depositMoney, cancellationToken);



            WithdrawMoneyCommand withdrawMoney = new()
            {
                AcountId = 2,
                Curency = "$",
                WithdrawAmmount = 100,
                DateOfOperation = DateTime.UtcNow
            };
            //WithdrawMoneyOperation withdrawOperation = serviceProvider.GetRequiredService<WithdrawMoneyOperation>();
            //withdrawOperation.PerformOperation(withdrawMoney);
            await mediator.Send(withdrawMoney, cancellationToken);



            CreateProductCommand newProduct = new()
            {
                Name = "Masline Verzi",
                Value = 3,
                Curency = "$",
                Limit = 5
            };

            //CreateProductOperation createProductOperation = serviceProvider.GetRequiredService<CreateProductOperation>();
            //createProductOperation.PerformOperation(newProduct);

            await mediator.Send(newProduct, cancellationToken);



            PurchaseProductCommand purchaseProductCommand = new()
            {
                IbanCode = (Int64.Parse(ibanService.GetNewIban()) - 1).ToString(),
                UniqueIdentifier = "2970304234566",
                ProductDetails = new List<PurchaseProductDetail>
            {
                new PurchaseProductDetail { ProductId = newProduct.IdProduct, Quantity = 3 }
            }
            };
            //PurchaseProductOperation purchaseProductOperation = serviceProvider.GetRequiredService<PurchaseProductOperation>();
            //purchaseProductOperation.Handle(purchaseProductCommand, default).GetAwaiter().GetResult();
            await mediator.Send(purchaseProductCommand, cancellationToken);



            var query = new ListOfAccounts.Query
            {
                PersonId = 1
            };
            //var handler = serviceProvider.GetRequiredService<ListOfAccounts.QueryHandler>();
            //var result = handler.Handle(query, default).GetAwaiter().GetResult();

            var result = await mediator.Send(query, cancellationToken);
        }
    }
}
