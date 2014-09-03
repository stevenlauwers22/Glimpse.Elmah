using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Elmah;

namespace Glimpse.Elmah.Plumbing.GetErrors
{
    public class GetErrorsHandler : IGetErrorsHandler
    {
        public GetErrorsResult Handle(GetErrorsRequest request)
        {
            // Get the errors from Elmah
            var errorList = new List<ErrorLogEntry>(request.PageSize);
            var totalCount = ErrorLog.GetDefault(HttpContext.Current).GetErrors(request.PageIndex, request.PageSize, errorList);
            if (totalCount == 0)
                return new GetErrorsResult();

            // Convert the Elmah errors to error info objects
            var path = VirtualPathUtility.ToAbsolute("~/", HttpContext.Current.Request.ApplicationPath);
            var elmahPath = ConfigurationManager.AppSettings["elmah.mvc.route"]
                ?? ConfigurationManager.AppSettings["elmah.route"]
                ?? "elmah.axd";
            var errors = errorList
                .Select(errorEntry =>
                        new ErrorModel
                        {
                            Id = errorEntry.Id,
                            HostName = errorEntry.Error.HostName,
                            StatusCode = errorEntry.Error.StatusCode,
                            Type = errorEntry.Error.Type,
                            Message = errorEntry.Error.Message,
                            User = errorEntry.Error.User,
                            Date = errorEntry.Error.Time,
                            DetailsUrl = string.Format("<a href=\"{0}{1}/detail?id={2}&\" target=\"_blank\">View</a>", path, elmahPath, errorEntry.Id)
                        })
                .ToList();

            var result = new GetErrorsResult { Errors = errors };
            return result;
        }
    }
}