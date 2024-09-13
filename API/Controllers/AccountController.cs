using API.Repositories;
using API.Repositories.Interfaces;
using API.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("Login")]
        
        public IActionResult Login(Credentials credentials)
        {
            if(string.IsNullOrWhiteSpace(credentials.Username) && string.IsNullOrWhiteSpace(credentials.Password)){
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Username or Password can't be empty",
                    data = null as object,
                });
            }
            if(string.IsNullOrWhiteSpace(credentials.Username))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Username can't be empty",
                    data = null as object,
                });
            }
            if(string.IsNullOrWhiteSpace(credentials.Password))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Password can't be empty",
                    data = null as object,
                });
            }
            try
            {
                var result = _accountRepository.Login(credentials);
                if (result)
                {
                    var user = _accountRepository.GetAccountDataByCreds(credentials.Username);
                    var token = _accountRepository.GenerateToken(new CredsPayload
                    {
                        Username = credentials.Username,
                       
                    });
                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = $"Login success!, Welcome {user.FullName}",
                        data = token,
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Login failed, Password is Incorrect",
                        data = result,
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = e.Message,
                    data = null as object,
                });
            }
        }
        

        [HttpGet]
        
        public IActionResult Get()
        {
            var employees = _accountRepository.GetAccountData();
            // Wrap it on if else statement
            var countEmployees = _accountRepository.CountEmployee();

            if (employees != null && countEmployees != 0)
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = $"{countEmployees} Data fetched successfully",
                    data = employees,
                });
            }
            else
            {
                return NotFound(new
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = "Data is not found",
                    data = employees,
                });
            }
        }

        [HttpPost]
        
        public IActionResult Post([FromBody] EmployeeCreateVM2 employee)
        {

            if (string.IsNullOrWhiteSpace(employee.FirstName) && string.IsNullOrWhiteSpace(employee.LastName) && string.IsNullOrWhiteSpace(employee.Dept_Id))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "All of data can't be empty",
                    data = null as object,
                });
            }
            if (string.IsNullOrWhiteSpace(employee.Email))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Email of data can't be empty",
                    data = null as object,
                });
            }

            try
            {
                var result = _accountRepository.AddAccount(employee.FirstName, employee.LastName, employee.Email,employee.Password, employee.Dept_Id);
                if (result > 0)
                {
                    var lastInserted = _accountRepository.GetLastInsertedAccount();
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
            catch (Exception e)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = e.Message,
                    data = null as object,
                });
            }
        }

        [HttpDelete("{accountId}")]
        
        public IActionResult Delete(string accountId)
        {
            try
            {
                var result = _accountRepository.DeleteAccount(accountId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = "Data deleted successfully",
                        data = result,
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Data failed to delete",
                        data = result,
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = e.Message,
                    data = null as object,
                });
            }
        }

    }
}
