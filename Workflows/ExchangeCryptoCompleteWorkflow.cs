using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Embily.Models;
using Embily.Gateways;
using System.IO;
using Embily.Messages;
using System.Collections.Specialized;

namespace Embily.Workflows
{
    public class ExchangeCryptoCompleteWorkflow : BaseWorkflow
    {
        readonly ICryptoExchange _crypto;

        public ExchangeCryptoCompleteWorkflow(ICryptoExchange crypto, NameValueCollection appSettings, EmbilyDbContext ctx, TextWriter log)
            : base(appSettings, ctx, log)
        {
            _crypto = crypto;
        }

        public async Task<LoadCard> Process(ExchangeCryptoComplete msg)
        {
            var symbolStr = msg.OriginalCurrencyCode + msg.DestinationCurrencyCode;
            var symbol = symbolStr.ParseEnum<CryptoOrderSymbols>();
            var destinationCurrencyCode = msg.DestinationCurrencyCode.ParseEnum<CurrencyCodes>();

            var fx = await _crypto.GetExchangeCryptoTradeAsync(msg.OrderId, symbol);

            await UpdateTxn(msg.TxnId, TxnStatus.Complete, fx);

            return CreateOutMessage(msg, fx.DestinationAmount, fx.ExchangeFee);
        }

        protected async Task UpdateTxn(string txnId, TxnStatus status, ExchangeResult fx)
        {
            var txn = await _ctx.Transactions.FindAsync(txnId);

            txn.DestinationAmount = fx.DestinationAmount;
            txn.ExchangeFee = fx.ExchangeFee;
            txn.Status = status;

            await _ctx.SaveChangesAsync();
        }

        private LoadCard CreateOutMessage(ExchangeCryptoComplete msg, double amount, double fees)
        {
            return new LoadCard
            {
                TxnId = msg.TxnId,
                TransactionNumber = msg.TransactionNumber,
                Amount = amount,
                ExchangeFee = fees,
                CurrencyCode = msg.DestinationCurrencyCode,
            };
        }
    }
}
