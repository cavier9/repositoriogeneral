using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acortarurlcjcm.Model
{
    public class clsUrl
    {
        public Guid ID { get; set; }
        public string URL { get; set; }
        public string AcortarURL { get; set; }
        public string Token { get; set; }
        public int Clicked { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
