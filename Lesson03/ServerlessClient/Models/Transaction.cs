using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerlessClient.Models
{
    public class Transaction
    {
        public DateTime ExecutionTime { get; set; }
        public Decimal Amount { get; set; }
    }
}