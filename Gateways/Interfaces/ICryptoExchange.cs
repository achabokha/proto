using System;
using System.Threading.Tasks;

namespace Embily.Gateways
{
    public interface ICryptoExchange
    {
        Task<double> GetTransfer(string cryptoTxnId, CryptoCurrencyCodes code, DateTimeOffset aprxTxnDatetime);

        /// <summary>
        /// Placing an achange order, async... trade might come later  
        /// </summary>
        /// <param name="amount">crypto currency original amount</param>
        /// <param name="orderSymbol">pair from what currency to which, for example btcusd</param>
        /// <returns>echanged amount</returns>
        Task <long> ExchangeCryptoOrderAsync(double amount, CryptoOrderSymbols orderSymbol);

        Task <ExchangeResult> GetExchangeCryptoTradeAsync(long orderId, CryptoOrderSymbols orderSymbol);
    }

    public class ExchangeResult
    {
        public double DestinationAmount { get; set; }

        public double ExchangeFee { get; set; }
    }

    public class CryptoTransferHasNotArrivedYetException : ApplicationException
    {
        public CryptoTransferHasNotArrivedYetException() { }

        public CryptoTransferHasNotArrivedYetException(string message) : base(message) { }
    }

    public class ExchangeCryptoTradeException : ApplicationException
    {
        public ExchangeCryptoTradeException() { }

        public ExchangeCryptoTradeException(string message) : base(message) { }
    }
}
