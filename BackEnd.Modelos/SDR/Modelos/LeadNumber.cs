using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Modelos
{
    public class LeadNumber
    {
        public int NumberId { get; private set; }
        public string Number {  get; private set; }
        public string Type { get; private set; }
        public bool Whatsapp { get; private set; }

        public LeadNumber(string number, string type, bool whatsapp)
        {
            Number = number;
            Type = type;
            Whatsapp = whatsapp;
        }
        
        public LeadNumber(int numberId, string number, string type, bool whatsapp)
        {
            NumberId = numberId;
            Number = number;
            Type = type;
            Whatsapp = whatsapp;
        }
    }
}
