using System;

namespace MarketingBox.Reporting.Service.Domain
{
    public class RetryException : Exception
    {
        public RetryException(string message) : base (message)
        {
        }
    }
}
