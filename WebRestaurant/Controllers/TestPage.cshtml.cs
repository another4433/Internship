using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebRestaurant.Controllers;

public class TestPage(ILogger<TestPage> logger) : PageModel
{
    [HttpGet("{typeAnything}")]
    public void OnGet(string typeAnything)
    {
        logger.LogInformation($"TestPage called and received {typeAnything}");
    }
}