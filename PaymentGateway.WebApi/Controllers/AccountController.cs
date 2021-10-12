using Microsoft.AspNetCore.Mvc;
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
        public AccountController(CreateAccountOperation createAccountCommandHandler)
        {
            _createAccountCommandHandler = createAccountCommandHandler;
        }

        [HttpPost]
        [Route("Create")]
        public string CreateAccount(CreateAccountCommand command)
        {
            //CreateAccountOperation request = new CreateAccountOperation(new EventSender());

            _createAccountCommandHandler.PerformOperation(command);

            return "OK";
        }

        
    }
}
