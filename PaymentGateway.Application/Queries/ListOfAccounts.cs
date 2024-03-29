﻿//using Abstractions;
using FluentValidation;
using MediatR;
using PaymentGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.Queries
{
    public class ListOfAccounts /*: IReadOperation*/
    {
        //public class Validator : IValidator<Query>   //acesta s-a schimbat cu Abstract Validator din FluentValidation
        public class Validator : AbstractValidator<Query>
        {
            private readonly Database _database;
            public Validator(Database _database)
            {
                RuleFor(q => q).Must(query =>
                {
                    var person = query.PersonId.HasValue ?
                    _database.Persons.FirstOrDefault(x => x.IdPerson == query.PersonId) :
                    _database.Persons.FirstOrDefault(x => x.Cnp == query.Cnp);

                    return person != null;
                }).WithMessage("Customer not found");
            }
        }
        public class Validator2 : AbstractValidator<Query>
        {
            private readonly Database _database;
            public Validator2(Database _database)
            {
                
                RuleFor(q => q).Must(query =>
                {
                    
                    var person = query.PersonId.HasValue ?
                    _database.Persons.FirstOrDefault(x => x.IdPerson == query.PersonId) :
                    _database.Persons.FirstOrDefault(x => x.Cnp == query.Cnp);

                    return person != null;
                }).WithMessage("Customer not found");
            }
        }

        public class Query:IRequest<List<Model>>
        {
            public int? PersonId { get; set; }
            public string Cnp { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly Database _database;
            private readonly IValidator<Query> _validator;


            public QueryHandler(Database database) //, IValidator<Query> validator    <-am scos din lista de parametri asta
            {
                _database = database;
                
            }

            public Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var isValid = _validator.Validate(request);

                //if (!isValid)
                //{
                //    throw new Exception("Person not found");
                //}

                var person = request.PersonId.HasValue ?
                  _database.Persons.FirstOrDefault(x => x.IdPerson == request.PersonId) :
                  _database.Persons.FirstOrDefault(x => x.Cnp == request.Cnp);

                var db = _database.Accounts.Where(x => x.IdPerson == person.IdPerson);
                var result = db.Select(x => new Model
                {
                    Balance = x.Balance,
                    Currency = x.Currency,
                    Iban = x.IbanCode,
                    Id = x.IdAccount,
                    Limit = x.Limit,
                    Status = x.Status,
                    Type = x.Type
                }).ToList();
                return Task.FromResult(result);
            }

       
        }

        public class Model
        {
            public int Id { get; set; }
            public double Balance { get; set; }
            public string Currency { get; set; }
            public string Iban { get; set; }
            public string Status { get; set; }
            public double Limit { get; set; }
            public string Type { get; set; }
        }

    }
}
