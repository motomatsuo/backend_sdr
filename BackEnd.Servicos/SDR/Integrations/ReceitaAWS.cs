using BackEnd.Modelos.SDR.DTO.Receita;
using System.Text.Json;

public class ReceitaAWS
{
    private readonly HttpClient _httpClient;

    public ReceitaAWS(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReceitaAwsResponse> ConsultarCnpjAsync(string cnpj)
    {
        try
        {
            if (cnpj == null)
                throw new Exception("CNPJ informado é nulo.");

            // remove máscara
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length == 11)
                throw new Exception("O valor informado não é um CNPJ, e sim um CPF");

            if (cnpj.Length != 14)
                throw new Exception("CNPJ com tamanho irregular");

            var response = await _httpClient.GetAsync($"v1/cnpj/{cnpj}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao consultar a ReceitaWS. StatusCode: {(int)response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var receita = JsonSerializer.Deserialize<ReceitaAwsApiResponse>(json, options);

            if (receita is null)
                throw new Exception("Não foi possível interpretar a resposta da ReceitaWS.");

            if (!string.IsNullOrWhiteSpace(receita.status) &&
                receita.status.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(receita.message ?? "Erro retornado pela ReceitaWS.");
            }

            return new ReceitaAwsResponse
            (
                RazaoSocial: receita.nome,
                Cnpj: receita.cnpj,
                CodigoAtividade: receita.atividade_principal?.FirstOrDefault()?.code,
                Situacao: receita.situacao,
                Porte: receita.porte,
                DataAbertura: receita.abertura,
                Logradouro: receita.logradouro,
                Bairro: receita.bairro,
                Municipio: receita.municipio,
                Uf: receita.uf,
                Cep: receita.cep
            );
        }
        catch (Exception)
        {
            throw;
        }
    }

    private sealed class ReceitaAwsApiResponse
    {
        public string? nome { get; set; }
        public string? cnpj { get; set; }
        public string? situacao { get; set; }
        public string? porte { get; set; }
        public string? abertura { get; set; }
        public string? logradouro { get; set; }
        public string? bairro { get; set; }
        public string? municipio { get; set; }
        public string? uf { get; set; }
        public string? cep { get; set; }
        public string? status { get; set; }
        public string? message { get; set; }
        public List<ReceitaAwsAtividadePrincipal>? atividade_principal { get; set; }
    }

    private sealed class ReceitaAwsAtividadePrincipal
    {
        public string? code { get; set; }
        public string? text { get; set; }
    }
}