using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisApiDemo.Models;

namespace RedisApiDemo.Controllers
{
    public class ImageGalleryController : Controller
    {
        public IActionResult Index()
        {
            var images = new List<ImageView>
            {
                new ImageView { Id = 1, Title = "Mountain View", ImageUrl = "https://picsum.photos/id/1011/800/600" },
                new ImageView { Id = 2, Title = "White Buildings", ImageUrl = "https://picsum.photos/id/164/800/600" },
                new ImageView { Id = 3, Title = "Courtyard", ImageUrl = "https://picsum.photos/id/1060/800/600" },
                new ImageView { Id = 4, Title = "Elephant", ImageUrl = "https://picsum.photos/id/1074/800/600" },
                new ImageView { Id = 5, Title = "Payment Terminal", ImageUrl = "https://picsum.photos/id/1059/800/600" },
                new ImageView { Id = 6, Title = "Laptop", ImageUrl = "https://picsum.photos/id/0/800/600" }
            };

            return View(images);
        }
    }

    
}