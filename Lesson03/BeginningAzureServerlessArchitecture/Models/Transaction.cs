using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginningAzureServerlessArchitecture.Models
{
    public class Transaction
    {
        public DateTime ExecutionTime { get; set; }
        public Decimal Amount { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }
    }
}
