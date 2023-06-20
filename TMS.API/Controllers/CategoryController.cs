using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core;
using TMS.Infrastructure.Interfaces;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;
        public CategoryController(ICategoryService categoryService, ILogger logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        #region GetAllCategory
        [HttpGet("GetAllCategory")]
        public async Task<ResponseDTO<IEnumerable<Category>>> GetAllCategory()
        {
            ResponseDTO<IEnumerable<Category>> response = new ResponseDTO<IEnumerable<Category>>();
            int StatusCode = 0;
            bool isSuccess = false;
            IEnumerable<Category> Response = null;
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
        #endregion
    }
}
