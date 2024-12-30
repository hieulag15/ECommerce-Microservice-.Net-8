using eCommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Presentation.Controllers;

using Application.DTOs;
using Application.DTOs.Conversions;
using Application.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProduct productInterface) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProductDTO>> GetAllProducts()
    {
        var products = await productInterface.GetAllAsync();
        if (products is null)
        {
            return NotFound("No products found");
        }
        var (_, _products) = ProductConversion.FromEntity(null!, products);
        return _products is not null ? Ok(_products) : NotFound("No products found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(Guid id)
    {
        var product = await productInterface.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound("No product found");
        }
        var (_product, _) = ProductConversion.FromEntity(product, null);
        return _product is not null ? Ok(_product) : NotFound("No product found");
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var product = ProductConversion.ToEntity(productDTO);
        var result = await productInterface.CreateAsync(product);
        return result.Flag ? Ok("Product created successfully") : BadRequest("Failed to create product");
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateProduct(ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var product = ProductConversion.ToEntity(productDTO);
        var result = await productInterface.UpdateAsync(product);
        return result.Flag ? Ok("Product updated successfully") : BadRequest("Failed to update product");
    }

    [HttpDelete]
    public async Task<ActionResult<Response>> DeleteProduct(ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }
        var product = ProductConversion.ToEntity(productDTO);
        var result = await productInterface.DeleteAsync(product);
        return result.Flag ? Ok("Product deleted successfully") : BadRequest("Failed to delete product");
    }
}
