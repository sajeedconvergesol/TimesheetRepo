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
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;
        public CategoryController(ICategoryService categoryService, ILogger<ProjectController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO<Category>> GetCategory(int id)
        {
            ResponseDTO<Category> response = new ResponseDTO<Category>();
            int StatusCode = 0;
            bool isSuccess = false;
            Category? Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _categoryService.GetById(id);
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
        public async Task<ResponseDTO<IEnumerable<Category>>> GetAll()
        {
            ResponseDTO<IEnumerable<Category>> response = new ResponseDTO<IEnumerable<Category>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<Category>? Response = null;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var result = await _categoryService.GetAll();
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
        [HttpPost("AddCategory")]
        public async Task<ResponseDTO<int>> AddCategory(Category category)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var categoryId = await _categoryService.Add(category);
                if (categoryId==0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while category add";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Category added successfully";
                    Response = categoryId;
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
        public async Task<ResponseDTO<int>> UpdateCategory(Category category)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                int existingCategoryId = await _categoryService.Update(category);
                if (existingCategoryId==0)
                {
                    isSuccess = false;
                    StatusCode = 400;
                    Message = "Error occurred while category update";
                }
                else
                {
                    StatusCode = 200;
                    isSuccess = true;
                    Message = "Category updated successfully";
                    Response = existingCategoryId;
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
        public async Task<ResponseDTO<int>> DeleteCategory(int id)
        {
            ResponseDTO<int> response = new ResponseDTO<int>();
            int StatusCode = 0;
            bool isSuccess = false;
            int Response = 0;
            string Message = "";
            string ExceptionMessage = "";
            try
            {
                var data = await _categoryService.Delete(id);
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
