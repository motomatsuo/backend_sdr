using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Exceptions
{
    public class ModelException : ApplicationException
    {
        public ModelException(string msg) : base(msg) { }
    }
}
