using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.DTO.Contact;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record LeadResponse
    (
        int LeadId,
        string RazaoSocial,
        string? NomeFantasia,
        string CNPJ,
        string? Setor,
        string Faturamento,
        string? Site,
        DateTime DataInclusao,
        string StatusProspeccaoFk,
        List<SimpleAddressResponse>? Enderecos,
        List<SimpleContactResponse>? Contatos
    );
}
