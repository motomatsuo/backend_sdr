using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Modelos
{
    public class LoginPortal
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<string> Grupo { get; set; } = new();
    }
}
