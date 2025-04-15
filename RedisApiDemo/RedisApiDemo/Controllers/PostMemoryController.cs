using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RedisApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RedisApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostMemoryController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _clientFactory;

        public PostMemoryController(IMemoryCache cache, IHttpClientFactory clientFactory)
        {
            _cache = cache;
            _clientFactory = clientFactory;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!_cache.TryGetValue($"post:{id}", out Post cachedPost))
            {
                Thread.Sleep(2000);

                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}");

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var content = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<Post>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set($"post:{id}", post, cacheEntryOptions);

                return Ok(new { source = "api", data = post });
            }

            return Ok(new { source = "memory-cache", data = cachedPost });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!_cache.TryGetValue("posts:all", out List<Post> cachedPosts))
            {
                Thread.Sleep(5000);

                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");

                if (!response.IsSuccessStatusCode)
                    return BadRequest();

                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<Post>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                _cache.Set("posts:all", posts, cacheEntryOptions);

                return Ok(new { source = "api", data = posts });
            }

            return Ok(new { source = "memory-cache", data = cachedPosts });
        }
    }
}