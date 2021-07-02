using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocialMedia.Infrastucture.Filters
{
    public class GolbalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(BusinessException))
            {
                BusinessException exception = (BusinessException)context.Exception;

                object validation = new
                {
                    Status = 400,
                    Title = "BadRequest",
                    Detail = exception.Message
                };

                object json = new
                {
                    errors = new[] { validation }
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
            }
        }
    }
}
