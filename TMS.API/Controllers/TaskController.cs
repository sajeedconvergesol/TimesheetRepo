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
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService; 
		private readonly ILogger _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet("{id}")]
		public async Task<ResponseDTO<Tasks>> GetTask(int id)
		{
			ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
			int StatusCode = 0;
			bool isSuccess = false;
			Tasks Response = null;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				var result = await _taskService.GetById(id);
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
		[HttpGet("getallTasks")]
		public async Task<ResponseDTO<IEnumerable<Tasks>>> GetTasks()
		{
			ResponseDTO<IEnumerable<Tasks>> response = new ResponseDTO<IEnumerable<Tasks>>();
			int StatusCode = 0;
			bool isSuccess = false;
			IEnumerable<Tasks> Response = null;
			string Message = "";
			string ExceptionMessage = "";

			try
			{
				var result = await _taskService.GetAll();
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
		[HttpPost("createTasks")]
		public async Task<ResponseDTO<int>> CreateTasks(Tasks tasks)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				var result = await _taskService.Add(tasks);
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
		public async Task<ResponseDTO<int>> UpdateTask(Tasks tasks)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				int entry = await _taskService.Update(tasks);
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
				var data = await _taskService.Delete(id);
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
