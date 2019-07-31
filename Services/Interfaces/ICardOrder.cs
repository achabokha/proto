using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{

    public class CardOrderDetails
    {
        public double Amount { get; set; }

        public string CryptoAddress { get; set; }

        public string  QrCodeSrc { get; set; }

        public string Comment { get; set; }
    }

    public interface ICardOrder
    {
        Task<CardOrderDetails> GetForwardingAddress(double amount, string currencyCode, string cryptoCurrencyCode);

        Task CreateCardOrder(string orderId, string cryptoAddress);
    }
}
