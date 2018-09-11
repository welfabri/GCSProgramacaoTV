using GCSEntities.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GCSEntities.Services
{
    public static class LoginService
    {
        public static async Task<Usuario> Login(string usuario, string senha)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri($"{GCSConstantes.HOSTPRINCIPAL}/?e={usuario}&p={senha}");

            NameValueCollection nvc = new NameValueCollection
            {
                { "e", usuario },
                { "p", senha }
            };

            using (var myHttpClient = new HttpClient())
            {
                try
                {
                    using (var response = await myHttpClient.GetAsync(uri).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            String jsonString = await response.Content.ReadAsStringAsync();
                            Usuario u = JsonConvert.DeserializeObject<Usuario>(jsonString);
                            return u;
                        }
                        else
                            return null;
                    }
                } catch
                {
                    return null;
                }
            }            
        }

        private static void LoginCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            
        }

        public static async Task<Usuario> Registrar(string email, string senha, string nome, 
            string cpf, string sexo)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri($"{GCSConstantes.HOSTPRINCIPAL}/registrar.php?e={email}&p={senha}&n={nome}&c={cpf}&s={sexo}");

            NameValueCollection nvc = new NameValueCollection
            {
                { "e", email },
                { "p", senha }
            };

            using (var myHttpClient = new HttpClient())
            {
                try
                {
                    using (var response = await myHttpClient.GetAsync(uri).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            String jsonString = await response.Content.ReadAsStringAsync();
                            Usuario u = JsonConvert.DeserializeObject<Usuario>(jsonString);
                            return u;
                        }
                        else
                            return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
