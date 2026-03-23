using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Modelos
{
    public class LeadContact
    {
        public int ContactId { get; private set; }
        public string Name { get; private set; }
        public string JobTitle { get; private set; }
        public string Email { get; private set; }
        public List<LeadNumber> LeadNumbers { get; set; } = new List<LeadNumber>();

        public LeadContact(string name, string jobTitle, string email)
        {
            Name = name;
            JobTitle = jobTitle;
            Email = email;
        }

        public LeadContact(string name, string jobTitle, string email, string number, string type, bool whatsapp)
        {
            Name = name;
            JobTitle = jobTitle;
            Email = email;
            LeadNumbers.Add(new LeadNumber(number, type, whatsapp));
        }
    }
}
