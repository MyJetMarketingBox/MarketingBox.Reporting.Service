using System.Threading.Tasks;
using MarketingBox.Registration.Service.Messages.Registrations;

namespace MarketingBox.Reporting.Service.Engines.Interfaces
{
    public interface IReportingEngine
    {
        Task ProcessMessageAsync(RegistrationUpdateMessage message);
    }
}