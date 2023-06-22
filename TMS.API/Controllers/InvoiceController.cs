using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger _logger;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IMapper _mapper;
        private readonly ITimesheetMasterService _timesheetMasterService;
        private readonly ITimesheetDetailsService _timesheetDetailService;

        public InvoiceController(IInvoiceService invoiceService, IInvoiceDetailService invoiceDetailService, IMapper mapper, ITimesheetMasterService timesheetMasterService, ITimesheetDetailsService timesheetDetailService)
        {
            _timesheetDetailService = timesheetDetailService;
            _invoiceService = invoiceService;
            _invoiceDetailService = invoiceDetailService;
            _mapper = mapper;
            _timesheetMasterService = timesheetMasterService;

        }

        #region GetInvoiceById

        [HttpGet("{id}")]
        public async Task<ResponseDTO<Invoice>> GetInvoice(int id)
        {
            ResponseDTO<Invoice> response = new ResponseDTO<Invoice>();
            int StatusCode = 0;
            bool isSuccess = false;
            Invoice Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _invoiceService.GetById(id);
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

        #endregion

        #region GetAllInvoice

        [HttpGet("GetAllInvoice")]
        public async Task<ResponseDTO<IEnumerable<Invoice>>> GetAllInvoice()
        {
            ResponseDTO<IEnumerable<Invoice>> response = new ResponseDTO<IEnumerable<Invoice>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<Invoice> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _invoiceService.GetAll();
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "No invoice found";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Invoices fetched successfully";
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

        #endregion

        [HttpPost("AddInvoice")]
        public async Task<ResponseDTO<int>> AddInvoice(InvoiceRequestDTO invoiceRequestDTO)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timeSheetMaster = await _timesheetMasterService.GetTimesheetMaster(invoiceRequestDTO.TimeSheetMasterId);
                var timeSheetDetails = await _timesheetDetailService.GetByTimeSheetMasterId(invoiceRequestDTO.TimeSheetMasterId);

                if (timeSheetMaster != null)
                {
                    //Mapping the Invoice Model to InvoiceRequestDTO
                    var invoice = _mapper.Map<Invoice>(invoiceRequestDTO);
                    var invoiceDetails = _mapper.Map<List<InvoiceDetails>>(invoiceRequestDTO.InvoiceDetailRequestDTO);

                    double ratePerHour = 0;
                    if (invoiceDetails.Any())
                    {
                        ratePerHour = invoiceDetails[0].RatePerHour;
                    }

                    if (invoiceDetails.Any())
                    {
                        double totalHours = timeSheetDetails.Sum(x => x.Hours);
                        invoice.TotalAmount = totalHours * ratePerHour;
                    }

                    var invoiceId = await _invoiceService.Add(invoice);

                    if (timeSheetDetails.Any())
                    {
                        invoiceDetails.Clear();
                        foreach (var timeSheetDetail in timeSheetDetails)
                        {
                            InvoiceDetails invDetail = new InvoiceDetails
                            {
                                HoursBilled = timeSheetDetail.Hours,
                                InvoiceId = invoiceId,
                                RatePerHour = ratePerHour,
                                TaskAssignmentId = timeSheetDetail.TaskAssignmentId
                            };
                            invoiceDetails.Add(invDetail);
                        }
                    }

                    await _invoiceDetailService.Add(invoiceDetails);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Invoice generated successsfully";
                    Response = invoiceId;
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = false;
                    Message = "Timesheet not found";
                    Response = 0;
                }

            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while fetching data.";
                ExceptionMessage = ex.Message.ToString();
                //_logger.LogError(ex.ToString(), ex);
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
