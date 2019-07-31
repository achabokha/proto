using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Embily.Gateways;
using Embily.Messages;
using Embily.Models;

namespace Embily.Workflows
{
    public class DetectTransferWorkflow : BaseWorkflow
    {
        readonly ICryptoExchange _crypto;

        public DetectTransferWorkflow(ICryptoExchange crypto, NameValueCollection appSettings, EmbilyDbContext ctx, TextWriter log)
            : base(appSettings, ctx, log)
        {
            _crypto = crypto;
        }

        public async Task<ExchangeCrypto> Process(DetectTransfer msg)
        {
            var cryptoCurrencyCode = msg.OriginalCurrencyCode.ParseEnum<CryptoCurrencyCodes>();

            // this one will throw an exception if transfer not found, then a number of attempts will be made 
            // until message processed --
            double amount = await _crypto.GetTransfer(msg.CryptoTxnId, cryptoCurrencyCode, msg.AprxTxnDatetime);

            await UpdateTxnStatus(msg.TxnId, TxnStatus.Received);

            return new ExchangeCrypto
            {
                OriginalAmount = amount,
                OriginalCurrencyCode = cryptoCurrencyCode.ToString(),
                TxnId = msg.TxnId,
                TransactionNumber = msg.TransactionNumber,
                DestinationCurrencyCode = msg.DestinationCurrencyCode,
            }; 
        }
    }
}
