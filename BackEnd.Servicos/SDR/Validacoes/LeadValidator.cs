using BackEnd.Modelos.SDR.DTO.Lead;
using BackEnd.Modelos.SDR.Enums;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Servicos.SDR.Interfaces;

namespace BackEnd.Servicos.SDR.Validacoes
{
    public class LeadValidator : ILeadValidator
    {
        public LeadValidator() { }

        public void ValidateLead(Lead lead)
        {

            ValidarRazaoSocial(lead.RazaoSocial);
            /*
            ValidarNomeFantasia(lead.NomeFantasia);
            ValidarCnpOujCpf(lead.CNPJ);
            ValidarSetor(lead.Setor);
            ValidarSite(lead.Site);
            */
        }

        public void ValidateUpdateLead(UpdateLeadRequest updateLeadRequest)
        {
           //ValidarRazaoSocial(updateLeadRequest.RazaoSocial);
            /*
            ValidarNomeFantasia(lead.NomeFantasia);
            ValidarCnpOujCpf(lead.CNPJ);
            ValidarSetor(lead.Setor);
            ValidarSite(lead.Site);
            */
        }

        public void ValidateIdLoginPortal(int idLoginPortal)
        {
            if (idLoginPortal <= 0)
                throw new ModelException("Foi atribuido um valor inválido para o código do login do portal");
        }

        public void ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description))
                throw new ModelException("O campo descrição esta vazio ou é nulo!");
        }

        public void ValidateProspectingStatus(string prospectionStatus)
        {
            if (string.IsNullOrEmpty(prospectionStatus) || string.IsNullOrWhiteSpace(prospectionStatus))
                throw new ModelException("Foi atribuido um valor inválido para o código do status da prospecção");
        }

        public int ConvertProspectingStatusToInt(string prospectionStatus)
        {
            int idProspectionStatus = 0;
            foreach (string status in Enum.GetNames(typeof(ProspectionStatus)))
            {
                if (Enum.TryParse<ProspectionStatus>(prospectionStatus, out var statusString))
                {
                    idProspectionStatus = (int)statusString;
                    return idProspectionStatus;
                }
            }
            throw new ModelException("O status da prospecção não condiz com nenhum status possivel!");
        }

        public void ValidateLeadId(int idLead)
        {
            if (idLead <= 0)
                throw new ModelException("Foi atribuido um valor inválido para o código do lead"); // Deu erro quando eu passei 0 como argumento na requisição
        }

        private void ValidarRazaoSocial(string razaoSocial)
        {
            if (string.IsNullOrEmpty(razaoSocial) || string.IsNullOrWhiteSpace(razaoSocial))
                throw new ModelException("Foi atribuido um valor inválido ou vazio para a razão social!");
        }

       

        /*
        private void ValidarRazaoSocial(string razaoSocial)
        {
            if (string.IsNullOrEmpty(razaoSocial) || string.IsNullOrWhiteSpace(razaoSocial))
                throw new ModeloException("Foi atribuido um valor inválido ou vazio para a razão social!");
        }
        private void ValidarNomeFantasia(string nomeFantasia)
        {
            if (string.IsNullOrEmpty(nomeFantasia) || string.IsNullOrWhiteSpace(nomeFantasia))
                throw new ModeloException("Foi atribuido um valor inválido ou vazio para a o nome fantasia!");
        }
        private void ValidarCnpOujCpf(string cnpjOuCpf)
        {
            if (string.IsNullOrEmpty(cnpjOuCpf) || string.IsNullOrWhiteSpace(cnpjOuCpf))
                throw new ModeloException("Foi atribuido um valor vazio para o CPF/CNPJ!");

            if (cnpjOuCpf.Length != 11 && cnpjOuCpf.Length != 14)
            {
                throw new ModeloException("Foi atribuido um valor inválido que não configura um CPF ou CNPJ!");
            }
            // Falta testar se todos são numeros
        }

        private void ValidarSetor(string setor)
        {
            if (string.IsNullOrEmpty(setor) || string.IsNullOrWhiteSpace(setor))
                throw new ModeloException("Foi atribuido um valor inválido para o valor!");
        }

        private void ValidarSite(string site)
        {
            if (string.IsNullOrEmpty(site) || string.IsNullOrWhiteSpace(site))
                throw new ModeloException("Foi atribuido um valor inválido parao o site!");
        }
        */
    }
}
