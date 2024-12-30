using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Response;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories;

using Application.Interfaces;
using Data;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

public class ProductRepository(ProductDbContext context) : IProduct
{
    public async Task<Response> CreateAsync(Product entity)
    {
        try
        {
            // Check if the product already exists
            var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
            if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
            {
                return new Response(false, $"{entity.Name} already exists.");
            }

            entity.Id = Guid.NewGuid();
            var currentProduct = context.Products.Add(entity).Entity;
            await context.SaveChangesAsync();
            if (currentProduct is not null && currentProduct.Id != Guid.Empty)
            {
                return new Response(true, $"{currentProduct.Name} created successfully.");
            }
            else
            {
                return new Response(false, $"An error occurred while add {currentProduct!.Name}.");
            }
        }
        catch (Exception ex)
        {
            // Log the original exception
            LogExceptions.LogException(ex);

            // Display scary-free message to the user
            return new Response(false, "An error occurred while creating the product.");
        }
    }

    public async Task<Response> DeleteAsync(Product entity)
    {
        try
        {
            // Check if the product not exists
            var getProduct = await GetByIdAsync(entity.Id);
            if (getProduct is null)
            {
                return new Response(false, "Product not found.");
            }
            context.Products.Remove(entity);
            await context.SaveChangesAsync();
            return new Response(true, $"{entity.Name} deleted successfully.");
        }
        catch (Exception ex)
        {
            // Log the original exception
            LogExceptions.LogException(ex);

            // Display scary-free message to the user
            return new Response(false, "An error occurred while deleting the product.");
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            var products = await context.Products.AsNoTracking().ToListAsync();
            return products is not null ? products : null!;
        }
        catch (Exception ex)
        {
            // Log the original exception
            LogExceptions.LogException(ex);

            // Display scary-free message to the user
            throw new InvalidOperationException("An error occurred while retrieving the products.");
        }
    }

    public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
            return product is not null ? product : null!;
        }
        catch (Exception ex)
        {
            // Log the original exception
            LogExceptions.LogException(ex);

            // Display scary-free message to the user
            throw new InvalidOperationException("An error occurred while retrieving the products.");
        }
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        try
        {
            var product = await context.Products.FindAsync(id);
            return product is not null ? product : null!;
        }
        catch (Exception ex)
        {
            // Log the original exception
            LogExceptions.LogException(ex);

            // Display scary-free message to the user
            throw new InvalidOperationException("An error occurred while retrieving the products.");
        }
    }

    public async Task<Response> UpdateAsync(Product entity)
    {
        try
        {
            // Check if the product not exists
            var getProduct = await GetByIdAsync(entity.Id);
            if (getProduct is null)
            {
                return new Response(false, "Product not found.");
            }
            context.Entry(getProduct).State = EntityState.Modified;
            context.Products.Update(entity);
            await context.SaveChangesAsync();
            return new Response(true, $"{entity.Name} updated successfully.");
        }
        catch (Exception ex)
        {
            // Log the original exception
            LogExceptions.LogException(ex);

            // Display scary-free message to the user
            return new Response(false, "An error occurred while updating the product.");
        }
    }
}
