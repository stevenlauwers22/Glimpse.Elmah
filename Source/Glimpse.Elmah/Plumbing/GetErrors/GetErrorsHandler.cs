using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elmah;
using Glimpse.Elmah.Contracts.GetErrors;

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
                            Date = errorEntry.Error.Time
                        })
                .ToList();

            var result = new GetErrorsResult { Errors = errors };
            return result;
        }
    }
}