using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.WriteOpperations;
using PaymentGateway.PublishedLanguage.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CreateAccountOperation _createAccountCommandHandler;
        private readonly ListOfAccounts.QueryHandler _queryHandler;
        public AccountController(CreateAccountOperation createAccountCommandHandler, ListOfAccounts.QueryHandler queryHandler)
        {
            _createAccountCommandHandler = createAccountCommandHandler;
            _queryHandler = queryHandler;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<string> CreateAccount(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            //CreateAccountOperation request = new CreateAccountOperation(new EventSender());


            await _createAccountCommandHandler.Handle(command, cancellationToken);

            return "OK";
        }

        [HttpGet]
        [Route("ListOfAccounts")]
        // query: http://localhost:5000/api/Account/ListOfAccounts?PersonId=1&Cnp=1961231..
        // route: http://localhost:5000/api/Account/ListOfAccounts/1/1961231..
        public async Task<List<ListOfAccounts.Model>> GetListOfAccounts([FromQuery] ListOfAccounts.Query query, CancellationToken cancellationToken)
        {
            var result = await _queryHandler.Handle(query, cancellationToken);
            return result;
        }


    }
}
