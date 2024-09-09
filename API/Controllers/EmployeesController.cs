using API.Models;
using API.Repositories;
using API.Repositories.Interfaces;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            var employees = _employeeRepository.GetAllEmployee();
            // Wrap it on if else statement
            if (employees != null && employees.Count() != 0)
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Data fetched successfully",
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

        [HttpGet("GetAllEmployeeData")]
        public IActionResult GetAllEmployeeData()
        {
            var employees = _employeeRepository.EmployeeVMData2();
            // Wrap it on if else statement
            if (employees != null && employees.Count() != 0)
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Data fetched successfully",
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
        [HttpGet("EmpData")]
        public IActionResult EmployeeData()
        {
            var employees = _employeeRepository.EmployeeData();
            // Wrap it on if else statement
            if (employees != null && employees.Count() != 0)
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Data fetched successfully",
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
        public IActionResult AddEmployee(EmployeeCreateVM employee)
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

            //var result = _employeeRepository.AddEmployee(firstName, lastName, email, deptId);
            //if (result > 0)
            //{
            //    var lastInserted = _employeeRepository.GetLastInsertedAccount();
            //    return Ok(new
            //    {
            //        statusCode = StatusCodes.Status200OK,
            //        message = "Data added successfully",
            //        data = lastInserted,
            //    });
            //}
            //else
            //{
            //    return BadRequest(new
            //    {
            //        statusCode = StatusCodes.Status400BadRequest,
            //        message = "Data failed to add",
            //        data = result,
            //    });
            //}

            try
            {
                var result = _employeeRepository.AddEmployee(employee.FirstName, employee.LastName, employee.Email, employee.Dept_Id);
                if (result > 0)
                {
                    var lastInserted = _employeeRepository.GetLastInsertedAccount();
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



        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(string? employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {

                return BadRequest(new
                {

                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Id can't be null or empty",
                    data = null as object

                });
            }

            var employees = _employeeRepository.GetEmployeeById(employeeId);
            if (employees != null)
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = $"Data with Id {employeeId} fetched successfully",
                    data = employees,
                });
            }
            else
            {
                return NotFound(new
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = $"Data with Id {employeeId} not found",
                    data = null as object,
                });
            }
        }

        [HttpPut]
        public IActionResult UpdateEmployee(Employee? employee)
        {
            var get = _employeeRepository.GetEmployeeEntityById(employee.Employee_Id);
            if (get != null)
            {
                var result = _employeeRepository.UpdateEmployee(employee);
                if (result > 0)
                {

                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = "Data updated successfully",
                        data = new
                        {
                            EmployeeId = employee.Employee_Id,
                            EmployeeFirstName = employee.FirstName,
                            EmployeeLastName = employee.LastName,
                            EmployeeDeptId = employee.Dept_Id
                        },
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Data failed to updated",
                        data = new
                        {
                            EmployeeId = employee.Employee_Id,
                            EmployeeFirstName = employee.FirstName,
                            EmployeeLastName = employee.LastName,
                            EmployeeDeptId = employee.Dept_Id
                        },
                    });
                }
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound,
                message = $"Data with Id {employee.Employee_Id} is not found",
                data = get,
            });
        }

        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(string? employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {

                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Id can't be null or empty",
                    data = null as object,
                });
            }
            else
            {
                var get = _employeeRepository.GetEmployeeById(employeeId);
                var result = _employeeRepository.DeleteEmployee(employeeId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = $"Data with Id {employeeId} deleted successfully",
                        data = get,
                    });
                }
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = $"Data with Id {employeeId} is not found",
                    data = get,
                });
            }
        }
    }
}
