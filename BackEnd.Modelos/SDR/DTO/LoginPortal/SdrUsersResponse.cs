using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.LoginPortal
{
    public record SdrUsersResponse(int Id, string Nome, string Funcao, List<string> Grupo);
}
