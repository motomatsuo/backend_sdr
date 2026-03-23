using BackEnd.Modelos.SDR.DTO.Lead;
using BackEnd.Modelos.SDR.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Interfaces
{
    public interface ILeadValidator
    {
        public void ValidateLead(Lead lead);
        public void ValidateUpdateLead(UpdateLeadRequest updateLeadRequest);

        public void ValidateIdLoginPortal(int idLoginPortal);
        public void ValidateDescription(string description);
        public void ValidateProspectingStatus(string prospectionStatus);
        public int ConvertProspectingStatusToInt(string prospectionStatus);
        public void ValidateLeadId(int idLead);
    }
}
