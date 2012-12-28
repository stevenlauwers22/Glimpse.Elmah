using System.Collections.Generic;

namespace Glimpse.Elmah.Plumbing.GetErrors
{
    public class GetErrorsResult
    {
        public IEnumerable<ErrorModel> Errors { get; set; }
    }
}