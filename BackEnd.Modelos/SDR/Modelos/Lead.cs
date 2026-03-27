using BackEnd.Modelos.SDR.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Modelos
{
    public class Lead
    {
        public int LeadId { get; set; }
        public string RazaoSocial { get; private set; }
        public string NomeFantasia { get; private set; }
        public string CNPJ { get; private set; }
        public string Setor { get; private set; }
        public string Faturamento { get; private set; }
        public string Site { get; private set; }
        public DateTime DataInclusao { get; private set; }
        public ProspectionStatus StatusProspeccao { get; private set; }
        public List<LeadAddress> EnderecosLead { get; set; } = new List<LeadAddress>();
        public List<LeadContact> ContatosLead { get; set; } = new List<LeadContact>();
        public List<ActivityLog> Atividades { get; set; } = new List<ActivityLog>();

        public string IdProtheus { get; set; }
        public string IdVendedor { get; set; }

        public Lead(int idLead, string razaoSocial, string nomeFantasia, string cnpj, string setor, string faturamento, string site, DateTime dataInclusao, ProspectionStatus prospectionStatus)
        {
            LeadId = idLead;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            CNPJ = cnpj;
            Setor = setor;
            Faturamento = faturamento;
            Site = site;
            DataInclusao = dataInclusao;
            StatusProspeccao = prospectionStatus;
        }

        public Lead(string razaoSocial, string nomeFantasia, string cnpj, string setor, string faturamento, string site)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            CNPJ = cnpj;
            Setor = setor;
            Faturamento = faturamento;
            Site = site;
        }

        public Lead(string nomeFantasia, string setor, string faturamento, string site)
        {
            NomeFantasia = nomeFantasia;
            Setor = setor;
            Faturamento = faturamento;
            Site = site;
        }
    }
}
