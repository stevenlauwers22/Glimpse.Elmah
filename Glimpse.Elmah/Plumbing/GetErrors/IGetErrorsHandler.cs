namespace Glimpse.Elmah.Plumbing.GetErrors
{
    public interface IGetErrorsHandler
    {
        GetErrorsResult Handle(GetErrorsRequest request);
    }
}