using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Services.Interfaces;
using TMS.Services.Services;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly ILogger _logger;


        public InvoiceDetailController(IInvoiceDetailService invoiceDetailService, ILogger logger)
        {
            _invoiceDetailService = invoiceDetailService;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<ResponseDTO<InvoiceDetails>> GetProject(int id)
        {
            ResponseDTO<InvoiceDetails> response = new ResponseDTO<InvoiceDetails>();
            int StatusCode = 0;
            bool isSuccess = false;
            InvoiceDetails Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _invoiceDetailService.GetById(id);
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
        public async Task<ResponseDTO<IEnumerable<InvoiceDetails>>> GetDetails()
        {
            ResponseDTO<IEnumerable<InvoiceDetails>> response = new ResponseDTO<IEnumerable<InvoiceDetails>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<InvoiceDetails> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _invoiceDetailService.GetAll();
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
        public async Task<ResponseDTO<long>> CreateTasks(InvoiceDetails invoiceDetails)
        {
            ResponseDTO<long> response = new ResponseDTO<long>();
            int StatusCode = 0;
            bool isSuccess = false;
            long Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _invoiceDetailService.Add(invoiceDetails);
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
        public async Task<ResponseDTO<long>> UpdateTask(InvoiceDetails invoiceDetails)
        {
            ResponseDTO<long> response = new ResponseDTO<long>();
            long StatusCode = 0;
            bool isSuccess = false;
            long Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                long entry = await _invoiceDetailService.Update(invoiceDetails);
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
                var data = await _invoiceDetailService.Delete(id);
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
