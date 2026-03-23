using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.DataTables
{
    public record CreateClientRequest(ContactsDataRequest ContactsData, ComplementaryDataRequest ComplementaryData);
}
