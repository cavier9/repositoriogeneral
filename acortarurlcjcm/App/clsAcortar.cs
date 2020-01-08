using LiteDB;
using System;
using System.Linq;
using acortarurlcjcm.Model;
using System.Collections.Generic;

namespace acortarurlcjcm.App{
	

	public class clsAcortar{
      
        public string Token { get; set; } 
		private clsUrl biturl;

		private clsAcortar GenerateToken() {
			string urlsafe = string.Empty;
			Enumerable.Range(48, 75).Where(i => i < 58 || i > 64 && i < 91 || i > 96).OrderBy(o => new Random().Next()).ToList().ForEach(i => urlsafe += Convert.ToChar(i));
			Token = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
			return this;
		}

        public bool ExisteUrl(string url) {
            //verificar si la URL acortada ya existe en nuestra base de datos
            using (var db = new LiteDatabase(Properties.Resources.Conexionlite))
            {
                if (db.GetCollection<clsUrl>("urls").Exists(u => u.AcortarURL == url))
                {                    
                    return true;
                }
            }
            return false;
        }

        public URLResponse RetornarUrl(string url)
        {
            using (var db = new LiteDatabase(Properties.Resources.Conexionlite))
            {
                return new URLResponse() { url = url, status = "URL ya existe", token = db.GetCollection<clsUrl>("urls").Find(u => u.URL == url).FirstOrDefault().Token };
            }
        }

		public void Acortarurl(string url) {
                using (var db = new LiteDatabase(Properties.Resources.Conexionlite)) {
                var urls = db.GetCollection<clsUrl>("urls");
                //Mientras el token existe en nuestro LiteDB, generamos uno nuevo
                //Básicamente significa que si ya existe un token, simplemente generamos uno nuevo
                while (urls.Exists(u => u.Token == GenerateToken().Token)) ;
                biturl = new clsUrl() { Token = Token, URL = url, AcortarURL = new OtraConfiguracion().Config.BASE_URL + Token ,Clicked=0, Created = DateTime.Now};
                if (urls.Exists(u => u.URL == url))
                    throw new Exception("URL ya existe");
                urls.Insert(biturl);
            }
		}

        public string UrlToken(string token) {
            using (var db = new LiteDatabase(Properties.Resources.Conexionlite))
            {
                var tbl = db.GetCollection<clsUrl>("urls");
                clsUrl objurl = db.GetCollection<clsUrl>("urls").FindOne(u => u.Token == token);
                objurl.Clicked = objurl.Clicked + 1;
                tbl.Update(objurl);
                return db.GetCollection<clsUrl>("urls").FindOne(u => u.Token == token).URL;
            }
        }

        public List<clsUrl> Listaurl()
        {
            using (var db = new LiteDatabase(Properties.Resources.Conexionlite))
            {
                
                return db.GetCollection<clsUrl>("urls").FindAll().ToList();
            }
        }

    }
}
