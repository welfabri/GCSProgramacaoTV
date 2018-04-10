﻿using GCSEntities.Classes;
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
        public static async Task<Usuario> Login(string usuario, string senha)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri($"http://queijonerd.pe.hu/gcs/gcsprogramacaotv/index2.php?e={usuario}&p={senha}");

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("e", usuario);
            nvc.Add("p", senha);

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
                } catch(Exception ex)
                {
                    return null;
                }
            }            
        }

        private static void LoginCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            
        }

        public static Usuario Registrar(string email, string senha, string nome)
        {
            return null;
        }
    }
}
