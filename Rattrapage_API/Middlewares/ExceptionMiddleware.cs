using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Rattrapage_API.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rattrapage_API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (ArtisteNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ArtisteAlreadyExistsException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (ArtistesListNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Une erreur inattendue est survenue.");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new { message };
            var jsonResponse = JsonConvert.SerializeObject(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
