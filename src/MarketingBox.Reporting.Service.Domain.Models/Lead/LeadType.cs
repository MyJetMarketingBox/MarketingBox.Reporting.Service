namespace MarketingBox.Reporting.Service.Domain.Models.Lead
{
    public enum LeadCrmStatus
    {
        New,
        FullyActivated,
        NoAnswer,
        HighPriority,
        Callback,
        AutoCall,
        FailedExpectation,
        NotValidDeletedAccount,
        NotValidWrongNumber,
        NotValidNoPhonenumber,
        NotValidDuplicateUser,
        NotValidTestLead,
        NotValidUnderage,
        NotValidNoLanguageSupport,
        NotValidNeverRegistered,
        NotValidNonEligibleCountries,
        NotInterested,
        Transfer,
        FollowUp,
        ConversionRenew,
    }
}