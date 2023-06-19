using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Infrastructure.Repository;
using TMS.Services.Interfaces;

namespace TMS.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskAssignmentController : ControllerBase
	{
		private readonly ITaskAssignmentService _taskAssignmentService;
		private readonly ILogger _logger;

        public TaskAssignmentController(ITaskAssignmentService taskAssignmentService, ILogger<TaskAssignmentController> logger)
        {
            _taskAssignmentService = taskAssignmentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
		public async Task<ResponseDTO<TaskAssignment>> GetTaskAssignment(int id)
		{
			ResponseDTO<TaskAssignment> response = new ResponseDTO<TaskAssignment>();
			int StatusCode = 0;
			bool isSuccess = false;
			TaskAssignment Response = null;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				var result = await _taskAssignmentService.GetById(id);
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
					Message = "Valid dataShow";
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
		[HttpGet("getallTasksAssignment")]
		public async Task<ResponseDTO<IEnumerable<TaskAssignment>>> GetTaskAssignment()
		{
			ResponseDTO<IEnumerable<TaskAssignment>> response = new ResponseDTO<IEnumerable<TaskAssignment>>();
			int StatusCode = 0;
			bool isSuccess = false;
			IEnumerable<TaskAssignment> Response = null;
			string Message = "";
			string ExceptionMessage = "";

			try
			{
				var result = await _taskAssignmentService.GetAll();
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

		[HttpPost("createTasksAssignment")]
		public async Task<ResponseDTO<int>> GetTaskAssignment(TaskAssignment taskAssignment)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				var result = await _taskAssignmentService.Add(taskAssignment);
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
		public async Task<ResponseDTO<int>> UpdateTaskAssignment(TaskAssignment taskAssignment)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				int entry = await _taskAssignmentService.Update(taskAssignment);
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
					Message = "Data has been successFully..!!";
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
		public async Task<ResponseDTO<int>> DeleteTaskAssignment(int id)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				var data = await _taskAssignmentService.Delete(id);
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
