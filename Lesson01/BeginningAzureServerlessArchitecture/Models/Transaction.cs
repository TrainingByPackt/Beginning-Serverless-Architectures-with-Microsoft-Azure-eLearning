using System;

namespace BeginningAzureServerlessArchitecture.Models
{
    class Transaction
    {
        public DateTime ExecutionTime { get; set; }
        public Decimal Amount { get; set; }
    }
}
