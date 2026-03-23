using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Modelos
{
    public class LeadAddress
    {
        public int AddressId { get; private set; }
        public string Street { get; private set; }
        public int Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string UF { get; private set; }
        
        public LeadAddress(string street, int number, string neighborhood,  string city, string uf)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            UF = uf;
        }

        public LeadAddress(string street, string neighborhood,  string city, string uf)
        {
            Street = street;
            Neighborhood = neighborhood;
            City = city;
            UF = uf;
        }
    }
}
