using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Repositorios.SDR.Exceptions
{
    public class RepositoriesException : ApplicationException
    {
        public RepositoriesException(string msg) : base(msg) { }

        public RepositoriesException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
