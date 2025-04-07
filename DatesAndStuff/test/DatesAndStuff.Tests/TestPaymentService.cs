using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatesAndStuff.Tests
{
    internal class TestPaymentService : IPaymentService
    {
        uint startCallCount = 0;
        uint specifyCallCount = 0;
        uint confirmCallCount = 0;
        uint getBalanceCallCount = 0;
        uint cancelCallCount = 0;

        public void StartPayment()
        {
            if (startCallCount != 0 || specifyCallCount > 0 || confirmCallCount > 0 || cancelCallCount > 0)
                throw new Exception();

            startCallCount++;
        }

        public void SpecifyAmount(double amount)
        {
            if (startCallCount != 1 || specifyCallCount > 0 || confirmCallCount > 0 || cancelCallCount > 0)
                throw new Exception();

            specifyCallCount++;
        }

        public double GetBalance(Person person) 
        {
            if (startCallCount != 1 || specifyCallCount != 1 || confirmCallCount > 0 || cancelCallCount > 0)
                throw new Exception();

            getBalanceCallCount++;
            Console.WriteLine($"GetBalance called with {person.Name} and balance {person.Balance}");
            return person.Balance;
        }

        public void CancelPayment()
        {
            if (startCallCount != 1 || specifyCallCount != 1 || getBalanceCallCount != 1 || cancelCallCount > 0 || confirmCallCount > 0)
                throw new Exception();

            cancelCallCount++;
        }

        public void ConfirmPayment()
        {
            if (startCallCount != 1 || specifyCallCount != 1 || getBalanceCallCount != 1 || confirmCallCount > 0 || cancelCallCount > 0)
                throw new Exception();

            confirmCallCount++;
        }

        public bool SuccessFul()
        {
            bool confirmed = startCallCount == 1 && specifyCallCount == 1 && getBalanceCallCount == 1 && confirmCallCount == 1;
            bool cancelled = startCallCount == 1 && specifyCallCount == 1 && getBalanceCallCount == 1 && cancelCallCount == 1;
            
            return confirmed || cancelled;
        }
    }
}
