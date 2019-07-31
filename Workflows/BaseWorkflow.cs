using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Embily.Models;

namespace Embily.Workflows
{
    public class BaseWorkflow
    {
        protected const bool _skipDB = false;

        protected readonly NameValueCollection _appSettigs;
        protected readonly EmbilyDbContext _ctx;
        protected readonly TextWriter _log;

        

        public BaseWorkflow(NameValueCollection appSettigs, EmbilyDbContext ctx, TextWriter log)
        {
            _appSettigs = appSettigs;
            _ctx = ctx;
            _log = log;
        }

        protected async Task UpdateTxnStatus(string txnId, TxnStatus status)
        {
            if (_skipDB) return;

            var txn = await _ctx.Transactions.FindAsync(txnId);
            txn.Status = status;
            await _ctx.SaveChangesAsync();
        }

        protected void LogError(string error)
        {
            _log.WriteLine(error);
            _log.Flush();
        }
    }
}
