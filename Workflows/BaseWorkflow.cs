using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace Workflows
{
    public class BaseWorkflow
    {
        protected const bool _skipDB = false;

        protected readonly NameValueCollection _appSettigs;
        protected readonly DbContext _ctx;
        protected readonly TextWriter _log;

        

        public BaseWorkflow(NameValueCollection appSettigs, DbContext ctx, TextWriter log)
        {
            _appSettigs = appSettigs;
            _ctx = ctx;
            _log = log;
        }

        protected void LogError(string error)
        {
            _log.WriteLine(error);
            _log.Flush();
        }
    }
}
