using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookException)
            HandleMyRecipeBookException(context);
       
    }


    private static void HandleMyRecipeBookException(ExceptionContext context)
    {
        if (context.Exception is InvalidLoginException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorMessage(context.Exception.Message));
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            
        }
        else if (context.Exception is ErrorOnValidatorException)
        {
            var exception = (ErrorOnValidatorException) context.Exception;
            context.Result = new BadRequestObjectResult(new ResponseErrorMessage(exception.ErrorMessage.ToList()!));
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            
        }
        
    }
    private static void UnknowError(ExceptionContext context)
    {
        context.Result = new ObjectResult(new ResponseErrorMessage(ResourceLanguage.UNKNOW_ERROR));
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}