

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogApi.Controllers {

    [Produces("application/json")]
    [Route("api/picture")]
    public class  PicturesController: ControllerBase 
    {
        private readonly IHostingEnvironment _env;
        public PicturesController(IHostingEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetImage(int id)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot + "/Picture/", "shoes-" + id + ".png");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");

        }

    

    }

}