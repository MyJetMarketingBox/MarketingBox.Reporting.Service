using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models.Deposits;
using MarketingBox.Reporting.Service.Grpc.Models.Deposits.Requests;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface IDepositService
    {
        [OperationContract]
        Task<DepositSearchResponse> SearchAsync(DepositSearchRequest request);
    }
}