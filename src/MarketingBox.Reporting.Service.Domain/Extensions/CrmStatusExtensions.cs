using MarketingBox.Reporting.Service.Domain.Lead;

namespace MarketingBox.Reporting.Service.Domain.Extensions
{
    public static class CrmStatusExtensions
    {
        public static LeadCrmStatus ToCrmStatus(this string status)
        {
            switch (status.ToLower())
            {
                case "new":
                    return LeadCrmStatus.New;

                case "fullyactivated":
                    return LeadCrmStatus.FullyActivated;

                case "highpriority":
                    return LeadCrmStatus.HighPriority;

                case "callback":
                    return LeadCrmStatus.Callback;

                case "failedexpectation":
                    return LeadCrmStatus.FailedExpectation;

                case "notvaliddeletedaccount":
                    return LeadCrmStatus.NotValid;

                case "notinterested":
                    return LeadCrmStatus.NotInterested;

                case "transfer":
                    return LeadCrmStatus.Transfer;

                case "followup":
                    return LeadCrmStatus.FollowUp;

                case "noanswer":
                    return LeadCrmStatus.NA;

                case "conversionrenew":
                    return LeadCrmStatus.ConversionRenew;

                default:
                    return LeadCrmStatus.Unknown;
            }
        }
        public static string ToCrmStatus(this LeadCrmStatus status)
        {
            switch (status)
            {
                case LeadCrmStatus.New:
                    return "New";

                case LeadCrmStatus.FullyActivated:
                    return "FullyActivated";

                case LeadCrmStatus.HighPriority:
                    return "HighPriority";

                case LeadCrmStatus.Callback:
                    return "Callback";

                case LeadCrmStatus.FailedExpectation:
                    return "FailedExpectation";

                case LeadCrmStatus.NotValid:
                    return "NotValidDeletedAccount";

                case LeadCrmStatus.NotInterested:
                    return "NotInterested";

                case LeadCrmStatus.Transfer:
                    return "Transfer";

                case LeadCrmStatus.FollowUp:
                    return "Followup";

                case LeadCrmStatus.NA:
                    return "NoAnswer";

                case LeadCrmStatus.ConversionRenew:
                    return "ConversionRenew";

                default:
                    return "Unknown";
            }
        }
    }
}
