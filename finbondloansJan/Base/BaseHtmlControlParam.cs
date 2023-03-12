using System.Collections.Generic;

namespace StaffLoans.Base
{
    public class BaseHtmlControlParam
    {
        public string Id { get; set; }

        public object Value { get; set; }

        public string ValueString => Value?.ToString();

        public Dictionary<string, string> Attributes { get; set; }
    }
}