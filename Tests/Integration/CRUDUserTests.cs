using RestSharp;
using Xunit;
using System.Net;
using Newtonsoft.Json;

namespace Tests.System
{
    public class SignUpSystemTests
    {
        private const string BaseUrl = "https://localhost:7250/api/SignUp"; // Ajuste para o host da API

        [Fact]
        public async Task Should_Create_User_With_MultipartFormData()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true // Ignorar validação de certificado HTTPS
            };
            var client = new RestClient(options);

            // Configurando a requisição para enviar como multipart/form-data
            var createRequest = new RestRequest("/", Method.Post);
            createRequest.AlwaysMultipartFormData = true;

            // Adicionando os campos conforme o front-end
            createRequest.AddParameter("username", "newuser");
            createRequest.AddParameter("nomeSocial", "New User");
            createRequest.AddParameter("cpf", "12345678969");
            createRequest.AddParameter("nacionalidade", "Brasil");
            createRequest.AddParameter("email", "newuser5@example.com");
            createRequest.AddParameter("telefone", "123456789");
            createRequest.AddParameter("sexo", "masculino");
            createRequest.AddParameter("cor", "branco");
            createRequest.AddParameter("senha", "password");

            // Adicionando endereço como subcampos
            createRequest.AddParameter("enderecos[0][logradouro]", "Rua B");
            createRequest.AddParameter("enderecos[0][complemento]", "Apto 202");
            createRequest.AddParameter("enderecos[0][cep]", "12345001");

            // Adicionando arquivo de foto (opcional)
            var filePath = @"C:\Users\Gabriel\Source\Repos\Back-End\backend\wwwroot\imagens_produtos\IMG_3963.jpg";
            createRequest.AddFile("foto", filePath, "image/jpeg");

            // Enviando a requisição
            var createResponse = await client.ExecuteAsync(createRequest);

            // LOG Detalhado da Resposta
            global::System.Console.WriteLine($"Status Code: {createResponse.StatusCode}");
            global::System.Console.WriteLine($"Response Content: {createResponse.Content}");
            global::System.Console.WriteLine($"Response Headers: {JsonConvert.SerializeObject(createResponse.Headers)}");

            // Verificando o resultado da resposta
            if (createResponse.StatusCode != HttpStatusCode.Created)
            {
                Assert.True(false, $"Erro ao criar usuário. StatusCode: {createResponse.StatusCode}, " +
                                   $"Conteúdo: {createResponse.Content}");
            }

            // Validando o conteúdo da resposta
            Assert.False(string.IsNullOrWhiteSpace(createResponse.Content), "O conteúdo da resposta está vazio!");

            // Tentar deserializar a resposta
            dynamic responseContent;
            try
            {
                responseContent = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Erro ao deserializar o JSON: {ex.Message}, " +
                                   $"Conteúdo: {createResponse.Content}");
                return; // Retornar para evitar exceções adicionais
            }

            // Exibir o conteúdo completo da resposta
            global::System.Console.WriteLine($"Resposta da API: {JsonConvert.SerializeObject(responseContent, Formatting.Indented)}");

            // Validar o campo User e outros campos esperados
            Assert.NotNull(responseContent?.User); // Garantir que o campo User existe
            Assert.NotNull(responseContent?.User?.Id); // Garantir que o campo Id existe
            Assert.Equal("newuser", (string)responseContent.User.Username);

            // Log para depuração em caso de sucesso
            global::System.Console.WriteLine("Usuário criado com sucesso!");
        }
    }
}
