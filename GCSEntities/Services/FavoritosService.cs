using GCSEntities.Classes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GCSEntities.Services
{
    public static class FavoritosService
    {
        public static async Task<string> GravarFavorito(int idUsuario, string listaCanaisFavoritos)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri($"{GCSConstantes.HOSTPRINCIPAL}/favoritossite.php");

            Dictionary<string, string> nvc = new Dictionary<string, string>();
            nvc.Add("idusuario", idUsuario.ToString());
            nvc.Add("canaisfavoritos", listaCanaisFavoritos);
            nvc.Add("acao", "i");
            var content = new FormUrlEncodedContent(nvc);

            using (var myHttpClient = new HttpClient())
            {
                try
                {
                    using (var response = await myHttpClient.PostAsync(uri, content).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            String jsonString = await response.Content.ReadAsStringAsync();

                            return jsonString;
                        }
                        else
                            return "Erro ao tentar acessar o servidor";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<string> ListaFavoritos(int idUsuario)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri($"{GCSConstantes.HOSTPRINCIPAL}/favoritossite.php");

            Dictionary<string, string> nvc = new Dictionary<string, string>();
            nvc.Add("idusuario", idUsuario.ToString());            
            nvc.Add("acao", "c");
            var content = new FormUrlEncodedContent(nvc);

            using (var myHttpClient = new HttpClient())
            {
                try
                {
                    using (var response = await myHttpClient.PostAsync(uri, content).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            String jsonString = await response.Content.ReadAsStringAsync();

                            return jsonString;
                        }
                        else
                            return "Erro ao tentar acessar o servidor";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
