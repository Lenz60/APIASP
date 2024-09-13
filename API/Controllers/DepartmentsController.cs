using API.Models;
using API.Repositories;
using API.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class DepartmentsController : ControllerBase
    {
        private DepartmentRepository _departmentRepository;

        public DepartmentsController(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        
        public IActionResult GetAllDepartments()
        {
            var departments = _departmentRepository.GetAllDepartments();
            // Wrap it on if else statement
            if (departments != null && departments.Count() != 0)
            {


                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Data fetched successfully",
                    data = departments,
                });
            }
            else
            {

                return NotFound(new
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = "Data is not found",
                    data = departments,
                });
            }
        }

        [HttpPost]
        
        public IActionResult AddDepartment([FromBody]AddDepartment addDepartment)
        {
            if (string.IsNullOrWhiteSpace(addDepartment.Initial) && string.IsNullOrWhiteSpace(addDepartment.Name))
            {

                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Both of data can't be empty",
                    data = null as object,
                });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addDepartment.Initial))
                {

                    return BadRequest(new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Dept Initial can't be empty",
                        data = new
                        {
                            DeptInitial = addDepartment.Initial,
                            DeptName = addDepartment.Name
                        },
                    });
                }
                if (string.IsNullOrWhiteSpace(addDepartment.Name))
                {

                    return BadRequest(new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Dept Name can't be empty",
                        data = new
                        {
                            DeptInitial = addDepartment.Initial,
                            DeptName = addDepartment.Name
                        },
                    });
                }
                var result = _departmentRepository.AddDepartment(addDepartment.Initial, addDepartment.Name);
                if (result > 0)
                {
                    var lastInserted = _departmentRepository.GetLastInserted();

                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = "Data is successfully added",
                        data = lastInserted
                    });
                }

                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Data can't be empty",
                    data = new
                    {
                        DeptInitial = addDepartment.Initial,
                        DeptName = addDepartment.Name
                    },
                });
            }
        }

        [HttpPut]
        
        public IActionResult UpdateDepartment(Department? department)
        {
            var get = _departmentRepository.GetDepartmentById(department.Dept_Id);
            if (get != null)
            {
                var result = _departmentRepository.UpdateDepartment(department);
                if (result > 0)
                {

                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = "Data updated successfully",
                        data = new
                        {
                            DeptId = department.Dept_Id,
                            DeptInitial = department.Dept_Initial,
                            DeptName = department.Dept_Name
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
                            DeptId = department.Dept_Id,
                            DeptInitial = department.Dept_Initial,
                            DeptName = department.Dept_Name
                        },
                    });
                }
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound,
                message = $"Data with Id {department.Dept_Id} is not found",
                data = get,
            });
        }

        [HttpDelete("{deptId}")]
        
        public IActionResult DeleteDepartment(string? deptId)
        {
            if (string.IsNullOrWhiteSpace(deptId))
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
                var get = _departmentRepository.GetDepartmentById(deptId);
                var result = _departmentRepository.DeleteDepartment(deptId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = StatusCodes.Status200OK,
                        message = $"Data with Id {deptId} deleted successfully",
                        data = get,
                    });
                }
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = $"Data with Id {deptId} is not found",
                    data = get,
                });
            }
        }

        [HttpGet("{deptId}")]
        
        public IActionResult GetDepartmentById(string? deptId)
        {
            if (string.IsNullOrWhiteSpace(deptId))
            {

                return BadRequest(new
                {

                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Id can't be null or empty",
                    data = null as object

                });
            }

            var departments = _departmentRepository.GetDepartmentById(deptId);
            if (departments != null)
            {
                return Ok(new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = $"Data with Id {deptId} fetched successfully",
                    data = departments,
                });
            }
            else
            {
                return NotFound(new
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = $"Data with Id {deptId} not found",
                    data = null as object,
                });
            }
        }
    }
}
