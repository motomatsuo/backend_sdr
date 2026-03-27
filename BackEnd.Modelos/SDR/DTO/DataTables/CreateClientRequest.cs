using BackEnd.Modelos.SDR.DTO.LoginPortal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.DataTables
{
    public record CreateClientRequest(ContactsDataRequest ContactsData, ComplementaryDataRequest ComplementaryData, DadosVendedorRequest DadosVendedorRequest, string clientId);
}
