using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;


namespace RedisCache.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    readonly IDistributedCache _distributedCache;

    public ValuesController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }


    [HttpGet("set")]
    public async Task<IActionResult> Set(string firstName, string lastName)
    {
        await _distributedCache.SetStringAsync("name", firstName);
        byte[] surnameBytes = Encoding.UTF8.GetBytes(lastName);
        await _distributedCache.SetAsync("surname", surnameBytes);

        return Ok();
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        if (_distributedCache == null)
        {
            // Handle the situation when _distributedCache is null
            return BadRequest("Distributed cache is not available.");
        }

        try
        {
            var firstName = await _distributedCache.GetStringAsync("name");
            var lastNameBinary = await _distributedCache.GetAsync("surname");

            if (lastNameBinary != null)
            {
                var lastName = Encoding.UTF8.GetString(lastNameBinary);
                return Ok(new { firstName, lastName });
            }
            else
            {
                // Handle the situation when lastNameBinary is null
                return BadRequest("Last name not found in the cache.");
            }
        }
        catch (Exception)
        {
            // Handle the exception or log it for further investigation
            return StatusCode(500, "An error occurred while retrieving data from the cache.");
        }
    }
}
