namespace Glimpse.Elmah.Contracts.GetErrors
{
    public interface IGetErrorsHandler
    {
        GetErrorsResult Handle(GetErrorsRequest request);
    }
}