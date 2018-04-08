using GCSEntities.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GCSEntities.Services
{
    public static class LoginService
    {
        public static async Task<bool> Login(string usuario, string senha)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri($"http://queijonerd.pe.hu/gcs/gcsprogramacaotv/index2.php?e={usuario}&p={senha}");

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("e", usuario);
            nvc.Add("p", senha);

            var myHttpClient = new HttpClient();
            var response = await myHttpClient.GetStringAsync(uri);

            String jsonString = response.ToString();
            JsonConvert.DeserializeObject<Usuario>(jsonString);

            return usuario.Equals("admin", StringComparison.OrdinalIgnoreCase) && senha.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }

        private static void LoginCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            
        }

        public static bool Registrar(string email, string senha, string nome)
        {
            return true;
        }
    }
}
