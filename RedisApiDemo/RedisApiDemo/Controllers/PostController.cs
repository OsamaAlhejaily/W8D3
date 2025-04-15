using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using RedisApiDemo.Models;

namespace RedisApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConnectionMultiplexer _redis;

        public PostController(IHttpClientFactory clientFactory, IConnectionMultiplexer redis)
        {
            _clientFactory = clientFactory;
            _redis = redis;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cache = _redis.GetDatabase();
            string cacheKey = $"post:{id}";
            var cachedPost = cache.StringGet(cacheKey);

            if (cachedPost.HasValue)
            {
                var posts = JsonSerializer.Deserialize<Post>(cachedPost);
                return Ok(new { source = "cache", data = posts });
            }

            Thread.Sleep(5000);

            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var content = await response.Content.ReadAsStringAsync();
            var post = JsonSerializer.Deserialize<Post>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var serializedPost = JsonSerializer.Serialize(post);
            cache.StringSet(cacheKey, serializedPost, TimeSpan.FromMinutes(5));

            return Ok(new { source = "api", data = post });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cache = _redis.GetDatabase();
            string cacheKey = "posts:all";
            var cachedPosts = cache.StringGet(cacheKey);

            if (cachedPosts.HasValue)
            {
                var posts = JsonSerializer.Deserialize<List<Post>>(cachedPosts);
                return Ok(new { source = "cache", data = posts });
            }

            Thread.Sleep(2000);

            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var content = await response.Content.ReadAsStringAsync();
            var post = JsonSerializer.Deserialize<List<Post>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var serializedPosts = JsonSerializer.Serialize(post);
            cache.StringSet(cacheKey, serializedPosts, TimeSpan.FromMinutes(5));

            return Ok(new { source = "api", data = post });
        }
    }
}