using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountRepository _accountRepository;

        public AccountController(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost]
        public IActionResult Register(string? firstName, string? lastName, string email, string? deptId)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(deptId))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "All of data can't be empty",
                    data = null as object,
                });
            }
            else
            {
                var result = _accountRepository.Register(firstName, lastName, email, deptId);
                if (result > 0)
                {
                    var lastInserted = _accountRepository.GetLastInserted();
                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = "Data added successfully",
                        data = lastInserted,
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Data failed to add",
                        data = result,
                    });
                }
            }
        }
    }
}
