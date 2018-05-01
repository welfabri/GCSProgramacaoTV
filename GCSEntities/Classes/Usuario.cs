using GCSEntities.Classes.Converters;
using Newtonsoft.Json;

namespace GCSEntities.Classes
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        [JsonConverter(typeof(JsonBoolConverter))]
        public bool Ativo { get; set; }
        public string Sexo { get; set; }

        public string DescricaoSexo => Sexo == "M" ? "Masculino" : "Feminino";

        public string ImagemCliente
        {
            get
            {
                string i = "http://queijonerd.pe.hu/gcs/images/user-";
                i += Sexo == "M" ? "m" : "f";
                i +=".jpg";

                return i;
            }
        }
    }
}
