using FluentValidation;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;

namespace MarketingBox.Reporting.Service.Validators;

public class ReportSearchRequestValidator : AbstractValidator<ReportSearchRequest>
{
    public ReportSearchRequestValidator()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        RuleFor(x => x.ReportType)
            .NotNull();
        RuleFor(x => x.ToDate)
            .GreaterThan(x => x.FromDate)
            .When(x => x.ToDate.HasValue && x.FromDate.HasValue);
        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .When(c => c.CountryCodeType.HasValue);
        RuleFor(x => x.CountryCodeType)
            .NotNull()
            .When(c => !string.IsNullOrEmpty(c.CountryCode));
    }
}