using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Common.Enum
{
    public class Dropdownvalues
    {
        public enum SolutionType
        {
            ClickToPay = 1
        }

        public enum IntegrationType
        {
            Integrated = 1,
            NonIntegrated = 2
        }

        public enum PaymentGateway
        {
            Stripe = 1,
            WorldPay = 2
        }

        public enum TransactionType
        {
            Auth = 1,
            Tokenization = 2,
            Capture = 3
        }
    }
}
