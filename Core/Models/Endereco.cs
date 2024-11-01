using System.Text.Json.Serialization;

namespace Core.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public int SignUpId { get; set; }
        [JsonIgnore]
        public SignUp SignUp { get; set; }
    }
}
