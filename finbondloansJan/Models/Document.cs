using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class Document
    {
        public int DocTypeID { get; set; }
        public string DocName { get; set; }
        public string DocExt { get; set; }
        public byte[] DocData { get; set; }
    }
}