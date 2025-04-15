# ASP.NET Core API with Caching and Image Optimization


## Part 1: Redis Caching

Implements Redis caching to store data from an external API (JSONPlaceholder).

- `PostController.cs`: Fetches and caches post data using Redis
## Part 2: In-Memory Caching

Implements Microsoft's IMemoryCache for temporary in-memory storage.

- `PostMemoryController.cs`: Similar to PostController but uses IMemoryCache
- Configured in Program.cs with `services.AddMemoryCache()`
- Key feature: Process-specific caching that's simpler to set up than Redis

## Part 3: Dynamic UI Updates

Creates a frontend that fetches data from the API without page reloads.

- `index.html`: Main page with tabs for both caching types
- Uses Fetch API to make AJAX requests to the backend
- Key feature: Shows real-time comparison between cached and non-cached data

## Part 4: Lazy Loading Images

Optimizes image loading by deferring until images are visible.

- `ImageGalleryController.cs`: Provides image data to the view
- `Index.cshtml`: Gallery view with lazy loading implementation
- Key feature: Improves page load performance by loading images only when it can be visible

## How to Run

1. Ensure Redis is running on localhost:6379
2. Clone the repository
3. Open in Visual Studio
4. Build and run the application
5. Access the different features at:
   - `/api/post/{id}` - Redis cached data
   - `/api/postmemory/{id}` - Memory cached data
   - `http://localhost:YourPort/` - Dynamic UI with tabs
   - `/ImageGallery` - Lazy loaded image gallery

## Technologies Used

- ASP.NET Core 6
- StackExchange.Redis
- Microsoft's IMemoryCache
- JavaScript Fetch API
- IntersectionObserver for lazy loading