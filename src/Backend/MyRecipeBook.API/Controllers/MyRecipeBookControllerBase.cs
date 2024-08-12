using Microsoft.AspNetCore.Mvc;

namespace MyRecipeBook.API.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class MyRecipeBookControllerBase : ControllerBase;
