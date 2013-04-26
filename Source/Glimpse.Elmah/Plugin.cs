using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Elmah.Plumbing.GetErrors;

namespace Glimpse.Elmah
{
    public class Plugin : TabBase
    {
        private readonly IGetErrorsHandler _getErrorsHandler;

        public Plugin()
            : this(new GetErrorsHandler())
        {
        }

        public Plugin(IGetErrorsHandler getErrorsHandler)
        {
            _getErrorsHandler = getErrorsHandler;
        }

        public override string Name
        {
            get { return "Elmah"; }
        }

        public override object GetData(ITabContext context)
        {
            var getErrorsRequest = new GetErrorsRequest { PageIndex = 0, PageSize = 15 };
            var getErrorsResult = _getErrorsHandler.Handle(getErrorsRequest);
            if (getErrorsResult.Errors == null)
                return null;

            // Create the header row
            var data = new List<object[]>
			{
			    new object[]
			    {
			        "Host",
			        "Code",
			        "Type",
			        "Error",
			        "User",
			        "Date",
			        "Time",
                    "Details"
			    }
			};

            // Create the data rows
            data.AddRange(getErrorsResult.Errors
                .OrderByDescending(e => e.Date)
                .Select(errorEntry => new object[]
                {
                    errorEntry.HostName,
                    errorEntry.StatusCode,
                    errorEntry.Type,
                    errorEntry.Message,
                    errorEntry.User,
                    errorEntry.Date.ToShortDateString(),
                    errorEntry.Date.ToShortTimeString(),
                    string.Format("!{0}!", errorEntry.DetailsUrl)
                })
                .ToList());

            return data;
        }
    }
}