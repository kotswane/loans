using System.Web;

namespace StaffLoans.Base
{
    public class BaseHtmlControl<TParams> : IHtmlString where TParams : BaseHtmlControlParam
    {
        public TParams Params { get; set; } = default(TParams);

        public string ToHtmlString()
        {
            return ToString();
        }
    }
}