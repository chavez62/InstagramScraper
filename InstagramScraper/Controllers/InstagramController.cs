using InstagramScraper.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace InstagramScraper.Controllers
{
    public class InstagramController : Controller
    {
        private readonly InstagramService _instagramService;

        public InstagramController(InstagramService instagramService)
        {
            _instagramService = instagramService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string username)
        {
            try
            {
                var profile = await _instagramService.GetProfileAsync(username);
                return View("Profile", profile);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProfileImage(string imageUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(imageUrl);
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    return File(imageBytes, "image/jpeg");
                }
            }
            catch
            {
                // Return a default image if the fetch fails
                return RedirectToAction("DefaultProfileImage");
            }
        }

   
    }
}

