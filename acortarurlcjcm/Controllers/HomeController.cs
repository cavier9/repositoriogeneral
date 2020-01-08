using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using acortarurlcjcm.App;
using Volt.Http;
using LiteDB;
using acortarurlcjcm.Model;

namespace acortarurlcjcm.Controllers
{
	
	public class HomeController : Controller
	{
        
        [HttpGet, Route("/")]
		public IActionResult Index() {
			return View();
		}

		[HttpPost, Route("/")]
		public IActionResult PostURL([FromBody] string url) {
            // Acortar la URL y devolver el token como una cadena json
            clsAcortar shortURL = new clsAcortar();
            try {
                //Si la url no contiene el prefijo HTTP con ella
                if (!url.Contains("http")) {
                    url = "http://" + url;
                }
                //verificar si la URL acortada ya existe en nuestra base de datos
                    if (shortURL.ExisteUrl(url))
                    {
                        Response.StatusCode = 405;
                        return Json(new URLResponse() { url = url, status = "acortado", token = null });
                    }
                shortURL.Acortarurl(url);
                return Json(shortURL.Token);
            } catch (Exception ex) {
                if (ex.Message == "URL ya existe") {
                    Response.StatusCode = 400;
                    using (var db = new LiteDatabase(Properties.Resources.Conexionlite))
                    {
                        return Json(shortURL.RetornarUrl(url));
                    }
                        
				}
				throw new Exception(ex.Message);
			}
			//return StatusCode(500);
		}

		[HttpGet, Route("/{token}")]
		public IActionResult Redireccionar([FromRoute] string token) {
            clsAcortar shortURL = new clsAcortar();
            return Redirect(shortURL.UrlToken(token));
         }
         
		
		private string HallarRedireccion(string url){
			string result = string.Empty;
			using (var client = new HttpClient())
			{
				var response = client.GetAsync(url).Result;
				if (response.IsSuccessStatusCode)
				{
					result = response.Headers.Location.ToString();
				}
			}
			return result;
		}


        [HttpGet, Route("/Lista")]
        public IActionResult Lista()
        {
            clsAcortar shortURL = new clsAcortar();
            return View(shortURL.Listaurl());
        }

    }
}
