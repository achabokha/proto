using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Embily.Models;
using Embily.Gateways;
using System.IO;
using Embily.Messages;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Embily.Workflows
{
    public class ExchangeCryptoWorkflow : BaseWorkflow
    {
        readonly ICryptoExchange _crypto;

        public ExchangeCryptoWorkflow(ICryptoExchange crypto, NameValueCollection appSettings, EmbilyDbContext ctx, TextWriter log)
            : base(appSettings, ctx, log)
        {
            _crypto = crypto;
        }

        public async Task<ExchangeCryptoComplete> Process(ExchangeCrypto msg)
        {
            var symbolStr = msg.OriginalCurrencyCode + msg.DestinationCurrencyCode;
            var symbol = symbolStr.ParseEnum<CryptoOrderSymbols>();

            var orderId = await _crypto.ExchangeCryptoOrderAsync(msg.OriginalAmount, symbol);

            await UpdateTxnStatus(msg.TxnId, TxnStatus.ExchangeOrder);

            return CreateOutMessage(msg, orderId);
        }

        private ExchangeCryptoComplete CreateOutMessage(ExchangeCrypto msg, long orderId)
        {
            return new ExchangeCryptoComplete
            {
                TxnId = msg.TxnId,
                TransactionNumber = msg.TransactionNumber,
                OrderId = orderId,
                OriginalCurrencyCode = msg.OriginalCurrencyCode,
                DestinationCurrencyCode = msg.DestinationCurrencyCode,
            };
        }
    }
}
