using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using TMS.API.DTOs;
using TMS.API.Enums;
using TMS.Core;
using TMS.Services.Interfaces;
using TMS.Services.Services;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimesheetMasterService _timesheetMasterService;
        private readonly ITimesheetDetailsService _timesheetDetailsService;
        private readonly ITaskAssignmentService _taskAssignmentService;
        private readonly ITaskService _taskService;
        private readonly ITimesheetApprovalsService _timesheetApprovalsService;
        private readonly IUserResolverService _userResolverService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProjectService _projectService;
        private readonly IMailService _mailService;

        #region Ctor
        public TimeSheetController(ITimesheetMasterService timesheetMasterService,
            ITimesheetDetailsService timesheetDetailsService,
           ITaskAssignmentService taskAssignmentService,
           ITaskService taskService,
           ITimesheetApprovalsService timesheetApprovalsService,
           IUserResolverService userResolverService,
           ILogger<TimeSheetController> logger, IMapper mapper, IConfiguration config,IUserService userService, IWebHostEnvironment webHostEnvironment, IProjectService projectService, IMailService mailService)
        {
            _timesheetMasterService = timesheetMasterService;
            _timesheetDetailsService = timesheetDetailsService;
            _taskAssignmentService = taskAssignmentService;
            _taskService = taskService;
            _timesheetApprovalsService = timesheetApprovalsService;
            _userResolverService = userResolverService;
            _logger = logger;
            _mapper = mapper;
            _config = config;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _projectService = projectService;
            _mailService = mailService;
        }
        #endregion

        #region Task Assignment APIs

        #region GetTaskAssignment
        [HttpGet("GetTaskAssignment")]
        public async Task<ResponseDTO<TaskAssignmentResponseDTO>> GetTaskAssignment(int id)
        {
            ResponseDTO<TaskAssignmentResponseDTO> response = new ResponseDTO<TaskAssignmentResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TaskAssignmentResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var taskAssignment = await _taskAssignmentService.GetById(id);
                if (taskAssignment == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Task assignment not found";
                }
                else
                {
                    var taskAssignmentResponse = _mapper.Map<TaskAssignmentResponseDTO>(taskAssignment);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task assignment fetched successfully";
                    Response = taskAssignmentResponse;
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

        #region GetTaskAssignments
        [HttpGet("GetTaskAssignments")]
        public async Task<ResponseDTO<IEnumerable<TaskAssignmentResponseDTO>>> GetTaskAssignments()
        {
            ResponseDTO<IEnumerable<TaskAssignmentResponseDTO>> response = new ResponseDTO<IEnumerable<TaskAssignmentResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<TaskAssignmentResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";

            try
            {
                var result = await _taskAssignmentService.GetAll();
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "No task assignment found";
                }
                else
                {
                    var taskAssignments = _mapper.Map<List<TaskAssignmentResponseDTO>>(result);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task assignments fetched successfully";
                    Response = taskAssignments;
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

        #region AddTaskAssignment
        [HttpPost("AddTaskAssignment")]
        public async Task<ResponseDTO<int>> AddTaskAssignment(TaskAssignmentRequestDTO taskAssignmentRequest)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var taskAssignment = _mapper.Map<TaskAssignment>(taskAssignmentRequest);
                var taskAssignmentId = await _taskAssignmentService.Add(taskAssignment);
                if (taskAssignmentId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while task assignment";
                }
                else
                {
                    var employee = await _userService.GetById(taskAssignmentRequest.EmployeeId.ToString());
                    var createdBy = await _userService.GetById(taskAssignmentRequest.CreatedBy.ToString());
                    var project = await _projectService.GetById(taskAssignmentRequest.ProjectId);
                    var task = await _taskService.GetById(taskAssignmentRequest.TaskId);

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(_config["EmailSender:SenderEmail"]));
                    email.To.Add(MailboxAddress.Parse(employee.Email));
                    email.Subject = "New Task Assigned";

                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string templateFilePath = Path.Combine(wwwrootPath, "EmailTemplates/Task_Notification_Email_template.html");
                    string htmlTemplate = await System.IO.File.ReadAllTextAsync(templateFilePath);
                    htmlTemplate = htmlTemplate.Replace("#UserName#", employee.FirstName + " " + employee.LastName).Replace("#ProjectTitle#", project.ProjectName).Replace("#TaskTitle#", task.TaskName).Replace("#TaskDueDate#", taskAssignmentRequest.DueDate.ToShortDateString()).Replace("#CreatedBy#", createdBy.FirstName+" "+createdBy.LastName);

                    email.Body = new TextPart(TextFormat.Html) { Text = htmlTemplate };
                    MailData mailData = new MailData
                    {
                        EmailBody = htmlTemplate,
                        EmailSubject = "New Task Assigned",
                        EmailToId = employee.Email,
                        EmailToName = employee.FirstName + " " + employee.LastName
                    };

                    var sendMail = _mailService.SendMail(mailData);
                    if (!sendMail)
                    {
                        Message += ",Email Not Send";
                    }
                    else
                    {
                        Message += ",Email Send";
                    }

                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task assigned successfully";
                    Response = taskAssignment.Id;
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

        #region UpdateTaskAssignment
        [HttpPut("UpdateTaskAssignment")]
        public ResponseDTO<int> UpdateTaskAssignment(TaskAssignmentRequestDTO taskAssignmentRequest)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var taskAssignment = _mapper.Map<TaskAssignment>(taskAssignmentRequest);
                var updatedTaskAssignmentId = _taskAssignmentService.Update(taskAssignment);
                if (updatedTaskAssignmentId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while task assignment update";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task assignment updated successfully";
                    Response = updatedTaskAssignmentId;
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

        #region TaskAssignmentByUserId
        [HttpGet("TaskAssignmentByUserId")]
        public async Task<ResponseDTO<List<TaskAssignmentResponseDTO>>> TaskAssignmentByUserId(int userId)
        {
            ResponseDTO<List<TaskAssignmentResponseDTO>> response = new ResponseDTO<List<TaskAssignmentResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            List<TaskAssignmentResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var taskAssignedToUser = await _taskAssignmentService.GetTaskAssignedToUser(userId);
                if (taskAssignedToUser == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while task assignment update";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task assignment updated successfully";
                    var taskAssignedToUserMaped = _mapper.Map<List<TaskAssignmentResponseDTO>>(taskAssignedToUser);
                    Response = taskAssignedToUserMaped;
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

        #endregion

        #region DeleteTaskAssignment
        [HttpDelete("DeleteTaskAssignment")]
        public async Task<ResponseDTO<bool>> DeleteTaskAssignment(int id)
        {
            ResponseDTO<bool> response = new ResponseDTO<bool>();
            int StatusCode = 0;
            bool isSuccess = false;
            bool Response = false;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var isDeleted = await _taskAssignmentService.Delete(id);
                Response = isDeleted;
                if (!isDeleted)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Task assignment couldn't be removed";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task assignment deleted successfully";
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

        #endregion

        #region Task APIs

        #region GetTask
        [HttpGet("GetTask")]
        public async Task<ResponseDTO<TaskResponseDTO>> GetTask(int id)
        {
            ResponseDTO<TaskResponseDTO> response = new ResponseDTO<TaskResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TaskResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _taskService.GetById(id);
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Task not found";
                }
                else
                {
                    var task = _mapper.Map<TaskResponseDTO>(result);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task fetched successfully";
                    Response = task;
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

        #region GetTasks
        [HttpGet("GetTasks")]
        public async Task<ResponseDTO<IEnumerable<TaskResponseDTO>>> GetTasks()
        {
            ResponseDTO<IEnumerable<TaskResponseDTO>> response = new ResponseDTO<IEnumerable<TaskResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<TaskResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";

            try
            {
                var result = await _taskService.GetAll();
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "No tasks found";
                }
                else
                {
                    var tasks = _mapper.Map<List<TaskResponseDTO>>(result);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Tasks fetched successfully";
                    Response = tasks;
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

        #region AddTask
        [HttpPost("createTasks")]
        public async Task<ResponseDTO<int>> CreateTasks(TaskRequestDTO taskRequest)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var task = _mapper.Map<Tasks>(taskRequest);
                var taskId = await _taskService.Add(task);
                if (taskId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while task add";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task added successfully";
                    Response = taskId;
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

        #region UpdateTask
        [HttpPut("UpdateTask")]
        public ResponseDTO<int> UpdateTask(TaskRequestDTO taskRequest)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var task = _mapper.Map<Tasks>(taskRequest);
                int taskId = _taskService.Update(task);
                if (taskId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while task update";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task updated successfully";
                    Response = taskId;
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

        #region DeleteTask
        [HttpDelete("DeleteTask")]
        public async Task<ResponseDTO<bool>> DeleteTask(int id)
        {
            ResponseDTO<bool> response = new();
            int StatusCode = 0;
            bool isSuccess = false;
            bool Response = false;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var isDeleted = await _taskService.Delete(id);
                Response = isDeleted;
                if (!isDeleted)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while task remove";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Task removed successfully";
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

        #endregion

        #region TimeSheetApproval APIs

        #region GetTimeSheetApproval
        [HttpGet("GetTimeSheetApproval")]
        public async Task<ResponseDTO<TimeSheetApprovalResponseDTO>> GetTimeSheetApproval(int id)
        {
            ResponseDTO<TimeSheetApprovalResponseDTO> response = new ResponseDTO<TimeSheetApprovalResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimeSheetApprovalResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timeSheetApproval = await _timesheetApprovalsService.GetTimesheetApproval(id);
                if (timeSheetApproval == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Timesheet approval not found";
                }
                else
                {
                    var timeSheetApprovalResponse = _mapper.Map<TimeSheetApprovalResponseDTO>(timeSheetApproval);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Timesheet approval fetched successfully";
                    Response = timeSheetApprovalResponse;
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

        #region GetTimesheetApprovals
        [HttpGet("GetTimesheetApprovals")]
        public async Task<ResponseDTO<IEnumerable<TimeSheetApprovalResponseDTO>>> GetTimesheetApprovals()
        {
            ResponseDTO<IEnumerable<TimeSheetApprovalResponseDTO>> response = new ResponseDTO<IEnumerable<TimeSheetApprovalResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<TimeSheetApprovalResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";

            try
            {
                var result = await _timesheetApprovalsService.GetTimesheetApprovals();
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "No timesheet found for approval";
                }
                else
                {
                    var timesheetApprovals = _mapper.Map<List<TimeSheetApprovalResponseDTO>>(result);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Timesheet approvals fetched successfully";
                    Response = timesheetApprovals;
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

        #region GetTimeApprovalByTimeSheetId
        [HttpGet("GetTimeApprovalByTimeSheetId")]
        public async Task<ResponseDTO<TimeSheetApprovalResponseDTO>> GetTimeApprovalByTimeSheetId(int timesheetMasterId)
        {
            ResponseDTO<TimeSheetApprovalResponseDTO> response = new ResponseDTO<TimeSheetApprovalResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimeSheetApprovalResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timeSheetApproval = _timesheetApprovalsService.GetTimeApprovalByTimeSheetId(timesheetMasterId);
                if (timeSheetApproval == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Timesheet approval not found";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Valid data";
                    var timesheetApproval = _mapper.Map<TimeSheetApprovalResponseDTO>(timeSheetApproval);
                    Response = timesheetApproval;
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

        #region AddTimesheetApproval
        [HttpPost("AddTimesheetApproval")]
        public async Task<ResponseDTO<TimeSheetApprovalResponseDTO>> AddTimesheetApproval(TimeSheetApprovalRequestDTO approvalRequest)
        {
            ResponseDTO<TimeSheetApprovalResponseDTO> response = new ResponseDTO<TimeSheetApprovalResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimeSheetApprovalResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var currentUser = await _userResolverService.GetCurrentUser();
                approvalRequest.CreatedBy = currentUser.Id;
                approvalRequest.CreatedOn = DateTime.UtcNow;
                var timeSheetApproval = _mapper.Map<TimeSheetApprovals>(approvalRequest);
                var approvalId = await _timesheetApprovalsService.CreateTimesheetApproval(timeSheetApproval);
                if (approvalId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred in timesheet approval";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Timesheet approval added successfully";
                    Response = _mapper.Map<TimeSheetApprovalResponseDTO>(timeSheetApproval);
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

        #region UpdateTimeSheetApproval
        [HttpPut("UpdateTimesheetApproval")]
        public async Task<ResponseDTO<TimeSheetApprovalResponseDTO>> UpdateTimesheetApproval(TimeSheetApprovalRequestDTO approvalRequest)
        {
            ResponseDTO<TimeSheetApprovalResponseDTO> response = new ResponseDTO<TimeSheetApprovalResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimeSheetApprovalResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var currentUser = await _userResolverService.GetCurrentUser();
                approvalRequest.LastModifiedBy = currentUser.Id;
                approvalRequest.LastModifiedOn = DateTime.UtcNow;
                var timeSheetApproval = _mapper.Map<TimeSheetApprovals>(approvalRequest);
                int approvalId = _timesheetApprovalsService.UpdateTimesheetApproval(timeSheetApproval);
                if (approvalId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while timesheet approval update";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Data has been successfully..!!";
                    var approvalResponse = _mapper.Map<TimeSheetApprovalResponseDTO>(timeSheetApproval);
                    Response = approvalResponse;
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

        #region DeleteTimeSheetApproval
        [HttpDelete("DeleteTimesheetApproval")]
        public async Task<ResponseDTO<bool>> DeleteTimesheetApproval(int id)
        {
            ResponseDTO<bool> response = new();
            int StatusCode = 0;
            bool isSuccess = false;
            bool Response = false;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                bool isDeleted = await _timesheetApprovalsService.DeleteTimesheetApproval(id);
                if (!isDeleted)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "TimeSheet couldn't remove due to error.";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "TimeSheet approval deleted successfully";
                    Response = isDeleted;
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

        #endregion

        #region TimeSheet Master APIs

        #region GetTimesheetMaster
        [HttpGet("GetTimesheetMaster")]
        public async Task<ResponseDTO<TimesheetMasterResponseDTO>> GetTimesheetMaster(int id)
        {
            ResponseDTO<TimesheetMasterResponseDTO> response = new ResponseDTO<TimesheetMasterResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimesheetMasterResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timesheetMasterId = await _timesheetMasterService.GetTimesheetMaster(id);
                if (timesheetMasterId == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid timesheetMasterId";
                }
                else
                {
                    var timesheetDetail = await _timesheetDetailsService.GetByTimeSheetMasterId(timesheetMasterId.Id);
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Valid DataShow";
                    var timeSheet = _mapper.Map<TimesheetMasterResponseDTO>(timesheetMasterId);
                    timeSheet.TimeSheetDetails = timesheetDetail;
                    Response = timeSheet;
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

        #region GetTimesheetMastersList
        [HttpGet("GetTimesheetMastersList")]
        public async Task<ResponseDTO<IEnumerable<TimesheetMasterResponseDTO>>> GetTimesheetMastersList()
        {
            ResponseDTO<IEnumerable<TimesheetMasterResponseDTO>> response = new ResponseDTO<IEnumerable<TimesheetMasterResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<TimesheetMasterResponseDTO> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timeSheetMastersList = await _timesheetMasterService.GetTimesheetMastersAll();
                if (timeSheetMastersList == null)
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
                    var timeSheet = _mapper.Map<IEnumerable<TimesheetMasterResponseDTO>>(timeSheetMastersList);

                    foreach (var item in timeSheet)
                    {
                        item.TimeSheetDetails = await _timesheetDetailsService.GetByTimeSheetMasterId(item.Id);
                    }
                    Response = timeSheet;
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

        #region GetTimesheetMasterByUserId
        [HttpGet("GetTimesheetMasterByUserId")]

        public async Task<ResponseDTO<TimesheetMasterResponseDTO>> GetTimesheetMasterByUserId(int userId)
        {
            ResponseDTO<TimesheetMasterResponseDTO> response = new();
            int StatusCode = 0;
            bool isSuccess = false;
            TimesheetMasterResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timesheetMasterList = await _timesheetMasterService.GetByUserId(userId);
                if (timesheetMasterList.Any())
                {
                    var timeSheetApprovals = await _timesheetApprovalsService.GetTimesheetApprovals();

                    foreach (var item in timesheetMasterList)
                    {
                        var timeSheetApproval = timeSheetApprovals.Where(x => x.TimeSheetMasterId == item.Id).FirstOrDefault();

                        if (timeSheetApproval != null)
                        {
                            if (timeSheetApproval.ApprovalStatus == 0)
                            {
                                item.TimeSheetStatus = 0;
                                //you will have to show some flag for approval for example send for approval
                            }
                            else if (timeSheetApproval.ApprovalStatus == Convert.ToInt32(TimeSheetStatus.Approved))
                            {
                                item.TimeSheetStatus = Convert.ToInt32(TimeSheetStatus.Approved);
                                //you will have to show some flag for invoice for example create invoice
                            }
                            else if (timeSheetApproval.ApprovalStatus == Convert.ToInt32(TimeSheetStatus.InProgress))
                            {
                                item.TimeSheetStatus = Convert.ToInt32(TimeSheetStatus.InProgress);
                            }
                            else if (timeSheetApproval.ApprovalStatus == Convert.ToInt32(TimeSheetStatus.Pending))
                            {
                                item.TimeSheetStatus = Convert.ToInt32(TimeSheetStatus.Pending);
                            }
                            else if (timeSheetApproval.ApprovalStatus == Convert.ToInt32(TimeSheetStatus.Rejected))
                            {
                                item.TimeSheetStatus = Convert.ToInt32(TimeSheetStatus.Rejected);
                            }
                            else if (timeSheetApproval.ApprovalStatus == Convert.ToInt32(TimeSheetStatus.Completed))
                            {
                                item.TimeSheetStatus = Convert.ToInt32(TimeSheetStatus.Completed);
                            }
                        }
                    }
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Timesheets fetched successfully.";
                    var timeSheet = _mapper.Map<TimesheetMasterResponseDTO>(timesheetMasterList);
                    Response = timeSheet;
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Timesheets by user not found";
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

        #region AddTimeSheet
        [HttpPost("AddTimeSheet")]
        public async Task<ResponseDTO<TimesheetMasterResponseDTO>> AddTimeSheet(TimeSheetMasterRequestDTO timeSheetRequest)
        {
            ResponseDTO<TimesheetMasterResponseDTO> response = new ResponseDTO<TimesheetMasterResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimesheetMasterResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var timeSheet = _mapper.Map<TimeSheetMaster>(timeSheetRequest);
                var timeSheetMasterId = await _timesheetMasterService.CreateTimesheetMaster(timeSheet);

                //Add timesheet details
                if (timeSheetRequest.TimeSheetDetails.Any())
                {
                    //Set timesheet master Id to details list
                    timeSheetRequest.TimeSheetDetails.ForEach(x => { x.TimeSheetMasterId = timeSheetMasterId; });
                    List<TimeSheetDetails> timeSheetDetails= _mapper.Map<List<TimeSheetDetails>>(timeSheetRequest.TimeSheetDetails);
                    bool isDetailsAdded = await _timesheetDetailsService.Add(timeSheetDetails);
                }

                if (timeSheetMasterId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while timesheet add.";
                }
                else
                {
                    timeSheet.Id = timeSheetMasterId;
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Data has been created successFully..!!";
                    var timeSheetResponse = _mapper.Map<TimesheetMasterResponseDTO>(timeSheet);
                    Response = timeSheetResponse;
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

        #region UpdateTimeSheet
        [HttpPut("UpdateTimesheetMaster")]
        public ResponseDTO<TimesheetMasterResponseDTO> UpdateTimesheetMaster(TimeSheetMaster timesheetMaster)
        {
            ResponseDTO<TimesheetMasterResponseDTO> response = new ResponseDTO<TimesheetMasterResponseDTO>();
            int StatusCode = 0;
            bool isSuccess = false;
            TimesheetMasterResponseDTO Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                int updatedTimeSheetId = _timesheetMasterService.UpdateTimesheetMaster(timesheetMaster);
                if (updatedTimeSheetId == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error while update timesheet";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Timesheet updated successFully.";
                    var timesheetMasterUpdated = _mapper.Map<TimesheetMasterResponseDTO>(timesheetMaster);
                    Response = timesheetMasterUpdated;
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

        #region DeleteTimeSheet
        [HttpDelete("DeleteTimesheetMaster")]
        public async Task<ResponseDTO<bool>> DeleteTimesheetMaster(int id)
        {
            ResponseDTO<bool> response = new();
            int StatusCode = 0;
            bool isSuccess = false;
            bool Response = false;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                bool isDetailsDeleted = _timesheetDetailsService.DeleteByTimeSheetId(id);
                bool isDeleted = await _timesheetMasterService.DeleteTimesheetMaster(id);
                if (!isDeleted)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Timesheet couldn't remove due to error.";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Timesheet master and details deleted successfully";
                    Response = isDeleted;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while deleting data.";
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

        #endregion
    }
}