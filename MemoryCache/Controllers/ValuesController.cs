using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCache
{
    [Route("api/[controller]")]
    public class MemoryCacheController
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet("set")]
        public void Set(string name) => memoryCache.Set("name", name);

        [HttpGet]
        public string Get()
        {
            return memoryCache.TryGetValue("name", out string name) ? name.Substring(3) : null;
        }

        [HttpDelete]
        public void Delete() => memoryCache.Remove("name");

        [HttpGet("setDate")]
        public void SetDate(string name)
        {
            memoryCache.Set("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet("getDate")]
        public DateTime GetDate()
        {
            return memoryCache.Get<DateTime>("date");
        }
    }
}