using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Interfaces
{
    public interface IAddressValidator
    {
        public void ValidateAddressId(int idAddress);
        public int ValidateAddressNumber(int? addressNumber);
        public void ValidateAddressStreet(string? addressStreet);
        public void ValidateAddressCity(string? addressCity);
        public void ValidateAddressNeighborhood(string? addressNeighborhood);
        public void ValidateAddressUF(string? addressUF);
    }
}
