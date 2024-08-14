using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.API.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.API.Attributes;

public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserAttributeFilter));
