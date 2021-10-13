using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.ReadOpperations;
using PaymentGateway.Application.WriteOpperations;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string CreateAccount(CreateAccountCommand command)
        {
            //CreateAccountOperation request = new CreateAccountOperation(new EventSender());

            _createAccountCommandHandler.PerformOperation(command);

            return "OK";
        }

        [HttpGet]
        [Route("ListOfAccounts")]
        public List<ListOfAccounts.Model> GetListOfAccounts([FromQuery] ListOfAccounts.Query query)
        {
            var result = _queryHandler.PerformOperation(query);
            return result;
        }


    }
}
