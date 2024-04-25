using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        private async Task<UserDTO?> GetUserDTO()
        {
            var username = User.Identity?.Name;
            if (username == null)
            {
                return null;
            }

            return await _userService.GetUserByUsernameAsync(username);
        }

        [Authorize(Roles = "Producer, Admin")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddProduct([FromBody] ProductAddDTO productDto)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");
            var response = await _productService.AddProductAsync(productDto, requestingUser);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

        [Authorize(Roles = "Producer, Admin")]
        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> UpdateProduct([FromBody] ProductUpdateDTO productDto)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _productService.UpdateProductAsync(productDto, requestingUser);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

        [Authorize(Roles = "Producer, Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponse>> DeleteProduct(Guid productId)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _productService.DeleteProductAsync(productId, requestingUser);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResponse<ProductDTO>>> GetProductById(Guid id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<RequestResponse<PagedResponse<ProductDTO>>>> GetProductsByProducer([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                                    // the PaginationSearchQueryParams properties to the object in the method parameter.
        {
            var response = await _productService.GetProductsByProducer(pagination);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

    }
}
