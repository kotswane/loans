using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class Loans
    {
        public string Terms { get; set; }
        public string Description { get; set; }

        public string Capital { get; set; }
        public string InitiationFee { get; set; }

        public string ServiceFee { get; set; }

        public string PrincipalDebt { get; set; }

        public string FirstPaymentDate { get; set; }

        public string LastPaymentDate { get; set; }

        public string InstallationAmount { get; set; }
    }
}