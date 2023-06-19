using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;

namespace TMS.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TimeSheetMasterController : ControllerBase
	{
		private readonly ITimesheetMasterService _timesheetMasterService;
		private readonly ILogger _logger;

        public TimeSheetMasterController(ITimesheetMasterService timesheetMasterService, ILogger<TimeSheetMasterController> logger)
        {
            _timesheetMasterService = timesheetMasterService;
            _logger = logger;
        }

        [HttpGet("{id}")]
		public async Task <ResponseDTO<TimeSheetMaster>> GetTimesheetMaster(int id) 
		{	
			ResponseDTO<TimeSheetMaster> response = new ResponseDTO<TimeSheetMaster>();
			int StatusCode = 0;
			bool isSuccess = false;
			TimeSheetMaster Response = null;
			string Message = "";	
			string ExceptionMessage = "";
			try
			{				
				var result = await _timesheetMasterService.GetTimesheetMaster(id);
				if(result == null)
				{
					isSuccess = false;
					StatusCode = 400;
					Message = "Invalid TimeSheetMaster";
				}
				else
				{
					StatusCode = 200;
					isSuccess = true;
					Message = "Valid DataShow";
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
		[HttpGet("getallMasters")]
		public async Task<ResponseDTO<IEnumerable<TimeSheetMaster>>> GetTimesheetMasters()
		{
			ResponseDTO<IEnumerable<TimeSheetMaster>> response = new ResponseDTO<IEnumerable<TimeSheetMaster>>();
			int StatusCode = 0;
			bool isSuccess = false;
			IEnumerable<TimeSheetMaster> Response = null;
			string Message = "";
			string ExceptionMessage = "";
				try
				{
					var result = await _timesheetMasterService.GetTimesheetMastersAll();
					if (result == null)
					{
						isSuccess = false;
						StatusCode = 400;
						Message = "Invalid TimeSheetMaster Get All";
					}
					else
					{
						StatusCode = 200;
						isSuccess = true;
						Message = "Valid DataShow";
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

		[HttpPost("createMaster")]
		public async Task<ResponseDTO<int>> CreateTimesheetMaster(TimeSheetMaster timesheetMaster)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
				try
				{
					var result = await _timesheetMasterService.CreateTimesheetMaster(timesheetMaster);
					if (result == null)
					{
						isSuccess = false;
						StatusCode = 400;
						Message = "Invalid Data Enter";
					}
					else
					{
						StatusCode = 200;
						isSuccess = true;
						Message = "Data Has Been Created";
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
		public async Task<ResponseDTO<int>> UpdateTimesheetMaster(TimeSheetMaster timesheetMaster)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
		     try 
				{	        
				int entry = await _timesheetMasterService.UpdateTimesheetMaster(timesheetMaster);
				if (entry == null)
				{
					isSuccess = false;
					StatusCode = 400;
					Message = "Invalid Data Enter in Filds ";
				}
				else
				{
					StatusCode = 200;
					isSuccess = true;
					Message = "Data Has been SuccessFully..!!";
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

		[HttpDelete("DeleteTimesheetMaster")]
		public async Task<ResponseDTO<int>> DeleteTimesheetMaster(int id)
		{
			ResponseDTO<int> response = new ResponseDTO<int>();
			int StatusCode = 0;
			bool isSuccess = false;
			int Response = 0;
			string Message = "";
			string ExceptionMessage = "";
			try
			{
				var data = await _timesheetMasterService.DeleteTimesheetMaster(id);
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
