using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;
using TMS.Services.Services;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectDocumentService _projectDocumentService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger,
            IProjectDocumentService projectDocumentService, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _projectDocumentService = projectDocumentService;
            _projectService = projectService;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
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
                    StatusCode = 200;
                    Message = "project not found";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Project fetched successfully";
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
        [HttpGet("GetAllProjects")]
        public async Task<ResponseDTO<IEnumerable<Project>>> GetAllProjects()
        {
            ResponseDTO<IEnumerable<Project>> response = new ResponseDTO<IEnumerable<Project>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<Project> Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _projectService.GetAll();
                if (result == null)
                {
                    isSuccess = false;
                    StatusCode = 200;
                    Message = "No project found";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Project list fetched successfully";
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
        [HttpPost("AddProject")]
        public async Task<ResponseDTO<int>> AddProject(Project project)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var projectId = await _projectService.Add(project);
                if (projectId == 0)
                {
                    isSuccess = false;
                    StatusCode = 200;
                    Message = "Error occured while project add";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Project added successfully";
                    Response = projectId;
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
        [HttpPut("{id}")]
        public ResponseDTO<int> UpdateProject(Project project)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                int existingProjectId = _projectService.Update(project);
                if (existingProjectId == 0)
                {
                    isSuccess = false;
                    StatusCode = 200;
                    Message = "Project not updated";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Project updated successfully";
                    Response = existingProjectId;
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
        public async Task<ResponseDTO<int>> DeleteProject(int id)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                await _projectService.Delete(id);
                if (id == 0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occured while project delete";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Project deleted successfully";
                    Response = id;
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

        [HttpPost("UploadProjectDocument")]
        public async Task<ResponseDTO<string>> UploadProjectDocument(ProjectDocRequestDTO projectDocument)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            int StatusCode = 0;
            bool isSuccess = false;
            string Response = "";
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var project = await _projectService.GetById(projectDocument.ProjectId);
                if (project != null)
                {
                    string[] allowedExtension = { ".jpeg", ".png", ".pdf" };
                    var fileExtension = Path.GetExtension(projectDocument.File.FileName);
                    if (!allowedExtension.Contains(fileExtension))
                    {
                        isSuccess = false;
                        StatusCode = 400;
                        Message = "Valid Document extentions are jpeg, png and pdf.";
                    }
                    else
                    {
                        long fileSizeLimit = 2 * 1024 * 1024; // 2 MB in bytes
                        if (projectDocument.File.Length > fileSizeLimit)
                        {
                            response.StatusCode = 200;
                            response.IsSuccess = false;
                            response.Message = "File size exceeds the maximum limit of 2 MB.";
                            return response;
                        }

                        // File Name With Random GUID
                        var fileName = Path.GetFileNameWithoutExtension(projectDocument.File.FileName);
                        Guid RandomGuid = Guid.NewGuid();
                        fileName = fileName + "-" + RandomGuid + fileExtension;

                        ProjectDocuments projectDoc = new ProjectDocuments()
                        {
                            ProjectId = projectDocument.ProjectId,
                            DocumentName = fileName,
                            CreatedBy = projectDocument.CreatedBy,
                            CreatedOn = projectDocument.CreatedOn,
                            LastModifiedBy = projectDocument.LastModifiedBy,
                            ModifiedOn = projectDocument.ModifiedOn
                        };

                        var projectDocumentId = await _projectDocumentService.Add(projectDoc);
                        if (projectDocumentId > 0)
                        {

                            // Crating Directory if not 
                            var basePath = Path.Combine(Directory.GetCurrentDirectory() + _configuration.GetSection("ImageUploadFolder").Value + projectDocument.ProjectId);
                            fileName = Path.GetFileNameWithoutExtension(projectDocument.File.FileName);
                            bool basePathExists = System.IO.Directory.Exists(basePath);
                            if (!basePathExists) Directory.CreateDirectory(basePath);

                            StatusCode = 200;
                            isSuccess = true;
                            Message = "Project document uploaded successfully";
                            Response = "Project document uploaded successfully";
                        }
                    }
                }
                else
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid Project.";
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while Performing Task.";
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

        [HttpPost("GetAllProjectDocuments")]
        public async Task<ResponseDTO<List<DocumentResponseDTO>>> GetAllProjectDocuments(int projectId)
        {
            ResponseDTO<List<DocumentResponseDTO>> response = new ResponseDTO<List<DocumentResponseDTO>>();
            int StatusCode = 0;
            bool isSuccess = false;
            List<DocumentResponseDTO> Response = new();
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var project = await _projectService.GetById(projectId);
                if (project != null)
                {
                    var basePath = _configuration.GetSection("BaseURL").Value + _configuration.GetSection("ImageUploadFolder").Value + projectId;

                    bool basePathExists = System.IO.Directory.Exists(basePath);
                    if (basePathExists)
                    {
                        var projectDocuments = await _projectDocumentService.GetByProjectId(projectId);
                        if (projectDocuments.Any())
                        {
                            var projectDocs = _mapper.Map<List<ProjectDocuments>>(projectDocuments);
                            foreach (var projectDoc in projectDocs)
                            {
                                var docPath = basePath + "/" + projectDoc.DocumentName;

                                DocumentResponseDTO documentResponse = new DocumentResponseDTO()
                                {
                                    ProjectId = projectId,
                                    DocumentUrl = docPath,
                                    Id = projectDoc.Id,
                                    DocumentName = projectDoc.DocumentName
                                };
                                Response.Add(documentResponse);
                            }
                            isSuccess = true;
                            StatusCode = 200;
                            Message = "List Of Document For Given Project Found";
                        }
                    }
                    else
                    {
                        isSuccess = false;
                        StatusCode = 400;
                        Message = "No Project Document Found.";
                    }
                }
            }
            catch (Exception error)
            {
                isSuccess = false;
                StatusCode = 500;
                Message = "Failed while Performing Task.";
                ExceptionMessage = error.Message.ToString();
                _logger.LogError(error.ToString(), error);
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
