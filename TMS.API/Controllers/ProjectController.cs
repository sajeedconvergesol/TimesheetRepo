using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Infrastructure.Interfaces;
using TMS.Services.Interfaces;
using TMS.Services.Services;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDocumentService _projectDocumentService;
        private readonly ILogger _logger;
        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger, IProjectDocumentService projectDocumentService)
        {
            _projectDocumentService = projectDocumentService;
            _projectService = projectService;
            _logger = logger;
        }

        #region GetProjectById
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

        #endregion 

        #region GetAllProjectDetails
        [HttpGet("GetAll")]
        public async Task<ResponseDTO<IEnumerable<Project>>> GetAllProjectDetails()
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

        #region AddProject
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
        #endregion

        #region UpdateProject
        [HttpPut("{id}")]
        public async Task<ResponseDTO<int>> UpdateProject(int id, Project project)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var existingProject = await _projectService.GetById(id);
                if (existingProject == null)
                {
                    StatusCode = 400;
                    isSuccess = false;
                    Message = "Invalid data entered. Project not found.";
                }
                else
                {
                    existingProject.ProjectName = project.ProjectName;
                    existingProject.StartDate = project.StartDate;
                    existingProject.EndDate = project.EndDate;

                    // Call your project service update method to save changes
                    var updatedProject = await _projectService.Update(existingProject);

                    if (updatedProject != null)
                    {
                        StatusCode = 200;
                        isSuccess = true;
                        Response = updatedProject;
                        Message = "Project has been updated successfully.";
                    }
                    else
                    {
                        StatusCode = 500;
                        isSuccess = false;
                        Message = "Failed to update the project.";
                    }
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

        #region DeleteProject

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
        #endregion


        [HttpPost("{id}/UploadProjectDocument")]
        public async Task<ResponseDTO<int>> UploadProjectDocument(int id ,ProjectDocuments project)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var projectId = await _projectService.GetById(id);
                if (projectId == null)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Invalid Project Id.";
                }
                else
                {
                    var createDocument = await _projectDocumentService.Add(project);
                    if(createDocument != null) { 
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Data has been created";
                    Response = createDocument;
                    }
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
