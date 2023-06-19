using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;
        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO<Project>> GetProject(int id)
        {
            ResponseDTO<Project> response = new ResponseDTO<Project>();
            int StatusCode = 0;
            bool isSuccess = false;
            Project Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _projectService.GetById(id);
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Valid datashow";
                    Response = result;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                _logger.LogError(ex.ToString(), ex);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Response = Response;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }
        [HttpGet("GetAll")]
        public async Task<ResponseDTO<IEnumerable<Project>>> GetTasks()
        {
            ResponseDTO<IEnumerable<Project>> response = new ResponseDTO<IEnumerable<Project>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable <Project> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _projectService.GetAll();
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Valid datashow";
                    Response = result;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                _logger.LogError(ex.ToString(), ex);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Response = Response;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }
        [HttpPost("CreateProject")]
        public async Task<ResponseDTO<int>> CreateTasks(Project project)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _projectService.Add(project);
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data enter";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Data has been created";
                    Response = result;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                _logger.LogError(ex.ToString(), ex);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Response = Response;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }
        [HttpPut]
        public async Task<ResponseDTO<int>> UpdateTask(Project project)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                int entry = await _projectService.Update(project);
                if (entry == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data enter in filds ";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Data has been successfully..!!";
                    Response = entry;
                }
            }
            catch (Exception ex)
            {



                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                _logger.LogError(ex.ToString(), ex);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Response = Response;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }



        [HttpDelete("{id}")]
        public async Task<ResponseDTO<int>> DeleteTask(int id)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var data = await _projectService.Delete(id);
                if (data == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid data";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Data has been deleted successfully";
                    Response = data;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                _logger.LogError(ex.ToString(), ex);
            }
            response.StatusCode = StatusCode;
            response.IsSuccess = isSuccess;
            response.Response = Response;
            response.Message = Message;
            response.ExceptionMessage = ExceptionMessage;
            return response;
        }
    }
}
