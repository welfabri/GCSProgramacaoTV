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
    }
}
