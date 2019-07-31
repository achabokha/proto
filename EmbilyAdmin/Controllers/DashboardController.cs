using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Embily.Models;
using EmbilyAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmbilyAdmin.Controllers
{
    public class Dashboard
    {
        public string Greeting { get; internal set; }
        public string Tag { get; internal set; }

        public double dollarVolumeAll { get; internal set; }
        public double dollarVolume24h { get; internal set; }
        public double dollarVolume30d { get; internal set; }

        public double euroVolumeAll { get; internal set; }
        public double euroVolume24h { get; internal set; }
        public double euroVolume30d { get; internal set; }

        public int numNewApplications { get; internal set; }
        public int numTransactions { get; set; }
        public int numAccounts { get; set; }
        public int numUsers { get; set; }
        public string EnvironmentName { get; internal set; }
    }

    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        readonly EmbilyDbContext _ctx;
        readonly ILogger _logger;
        readonly IHostingEnvironment _env;

        public DashboardController(EmbilyDbContext ctx, ILogger<DashboardController> logger, IHostingEnvironment env)
        {
            _env = env;
            _ctx = ctx;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<Dashboard> GetAll()
        {
            var user = await _ctx.Users.FindAsync(GetUserId());

            var dash = new Dashboard
            {
                Greeting = $"Hello {user.FirstName} {user.LastName}!",
                Tag = "How are you doing today?",

                dollarVolumeAll = _ctx.Transactions.Where(tr => tr.DestinationCurrencyCode == CurrencyCodes.USD & tr.TxnCode == TxnCodes.TRANSFER).Sum(a => a.DestinationAmount),
                dollarVolume24h = _ctx.Transactions.Where(t => t.DateCreated > DateTime.UtcNow.AddDays(-1) & t.DestinationCurrencyCode == CurrencyCodes.USD & t.TxnCode == TxnCodes.TRANSFER).Sum(a => a.DestinationAmount),
                dollarVolume30d = _ctx.Transactions.Where(t => t.DateCreated > DateTime.UtcNow.AddDays(-30) & t.DestinationCurrencyCode == CurrencyCodes.USD & t.TxnCode == TxnCodes.TRANSFER).Sum(a => a.DestinationAmount),

                euroVolumeAll = _ctx.Transactions.Where(tr => tr.DestinationCurrencyCode == CurrencyCodes.EUR & tr.TxnCode == TxnCodes.TRANSFER).Sum(a => a.DestinationAmount),
                euroVolume24h = _ctx.Transactions.Where(t => t.DateCreated > DateTime.UtcNow.AddDays(-1) & t.DestinationCurrencyCode == CurrencyCodes.EUR & t.TxnCode == TxnCodes.TRANSFER).Sum(a => a.DestinationAmount),
                euroVolume30d = _ctx.Transactions.Where(t => t.DateCreated > DateTime.UtcNow.AddDays(-30) & t.DestinationCurrencyCode == CurrencyCodes.EUR & t.TxnCode == TxnCodes.TRANSFER).Sum(a => a.DestinationAmount),

                numNewApplications = _ctx.Applications.Where(a => a.Status == ApplicationStatus.Submitted || a.Status == ApplicationStatus.Paid).Count(),
                numUsers = _ctx.Users.Count(),
                numAccounts = _ctx.Accounts.Count(),
                numTransactions = _ctx.Transactions.Count(),
                EnvironmentName = _env.EnvironmentName,
            };

            _logger.LogInformation(">>> test log: getting dashboard info!");

            return dash;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTransactions(string accountNumber, int hours, string currency)
        {
            List<string> lineChartLabels = new List<string>();

            List<ChartViewModel> charts = new List<ChartViewModel>();
            ChartViewModel transactionsCount = new ChartViewModel();
            transactionsCount.Label = "Transactions Count";
            transactionsCount.yAxisID = "right-y-axis";
            ChartViewModel transactionsAmount = new ChartViewModel();
            transactionsAmount.Label = "Transactions Amount";

            var transactions = await _ctx.Transactions.ToListAsync();

            var resp = transactions
                 .Where(t => DateTime.UtcNow.Subtract(t.DateCreated).TotalHours <= hours)
                 .Where(tc => tc.DestinationCurrencyCodeString == currency)
                 .GroupBy(g => Convert.ToInt32(DateTime.UtcNow.Subtract(g.DateCreated).TotalHours))
                 .Select(a => new
                 {
                     dateCreated = new DateTime(a.First().DateCreated.Year, a.First().DateCreated.Month, a.First().DateCreated.Day, a.First().DateCreated.Hour, 0, 0),
                     hoursSinceCreated = DateTime.UtcNow.Subtract(a.First().DateCreated).TotalHours,
                     amount = a.Sum(z => z.DestinationAmount),
                     count = a.Count()
                 })
                 .OrderBy(a => a.dateCreated);

            var countPoint = hours / 24;

            switch (countPoint)
            {
                case 1:
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            lineChartLabels.Add(i.ToString());

                            int count = 0;
                            int ammount = 0;

                            if (resp.Count() != 0)
                            {
                                foreach (var txn in resp)
                                {
                                    if (txn.dateCreated.Hour == i)
                                    {
                                        count =+ txn.count;
                                        ammount =+ (int)txn.amount;
                                    }
                                }

                            }
                            transactionsCount.Data.Add(count);
                            transactionsAmount.Data.Add(ammount);
                        }
                        break;
                    }
                case 7:
                case 30:
                    {
                        for (int i = 0; i < countPoint; i++)
                        {
                            int day = -(countPoint - 1) + i;
                            DateTime dateTimeCreate = DateTime.Now.AddDays(day);
                            lineChartLabels.Add(dateTimeCreate.ToString("MMM dd"));

                            int count = 0;
                            int ammount = 0;

                            if (resp.Count() != 0)
                            {
                                foreach (var txn in resp)
                                {
                                    if (txn.dateCreated.Date == dateTimeCreate.Date)
                                    {
                                        count = +txn.count;
                                        ammount = +(int)txn.amount;
                                    }
                                }

                            }
                            transactionsCount.Data.Add(count);
                            transactionsAmount.Data.Add(ammount);
                        }
                        break;
                    }
            }

            charts.Add(transactionsCount);
            charts.Add(transactionsAmount);

            return Ok(new { resp, lineChartLabels, chartData = charts, message = "" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetApplications(int hours)
        {
            List<string> lineChartLabels = new List<string>();
            //List<ChartViewModel> charts = new List<ChartViewModel>();
            List<int> chartData = new List<int>();
            //ChartViewModel applicationsCount = new ChartViewModel();
            //applicationsCount.Label = "Applications Count";
            //applicationsCount.yAxisID = "left-y-axis";

            var applications = await _ctx.Applications.ToListAsync();

            var resp = applications
                .Where(t => DateTime.UtcNow.Subtract(t.DateCreated).TotalHours <= hours)
                .GroupBy(g => Convert.ToInt32(DateTime.UtcNow.Subtract(g.DateCreated).TotalHours))
                .Select(a => new
                {
                    dateCreated = new DateTime(a.First().DateCreated.Year, a.First().DateCreated.Month, a.First().DateCreated.Day, a.First().DateCreated.Hour, 0, 0),
                    hoursSinceCreated = DateTime.UtcNow.Subtract(a.First().DateCreated).TotalHours,
                    count = a.Count()
                })
                .OrderBy(a => a.dateCreated);

            var countPoint = hours / 24;

            switch (countPoint)
            {
                case 1:
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            lineChartLabels.Add(i.ToString());

                            int count = 0;

                            if (resp.Count() != 0)
                            {
                                foreach (var app in resp)
                                {
                                    if (app.dateCreated.Hour == i)
                                    {
                                        count = +app.count;
                                    }
                                }

                            }
                            chartData.Add(count);
                        }
                        break;
                    }
                case 7:
                case 30:
                    {
                        for (int i = 0; i < countPoint; i++)
                        {
                            int day = -(countPoint - 1) + i;
                            DateTime dateTimeCreate = DateTime.Now.AddDays(day);
                            lineChartLabels.Add(dateTimeCreate.ToString("MMM dd"));

                            int count = 0;

                            if (resp.Count() != 0)
                            {
                                foreach (var app in resp)
                                {
                                    if (app.dateCreated.Date == dateTimeCreate.Date)
                                    {
                                        count = +app.count;
                                    }
                                }

                            }
                            chartData.Add(count);
                        }
                        break;
                    }
            }

            //charts.Add(applicationsCount);
            //charts.Add(applicationsCount);

            return Ok(new { resp, lineChartLabels, chartData, message = "" });
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetTransactionDistribution(int hours = 720)
        {
            var transactions = await _ctx.Transactions.ToListAsync();

            int getGroupIndex(string label)
            {
                switch (label)
                {
                    case "<500":
                        return 0;
                    case ">=500 and <1,000":
                        return 1;
                    case ">=1,000 and <2,500":
                        return 2;
                    case ">=2,500 and <5,000":
                        return 3;
                    case ">=5,000":
                        return 4;
                    default:
                        return 5;
                }
            }

            var resp = transactions
                .Where(t => DateTime.UtcNow.Subtract(t.DateCreated).TotalHours <= hours)
                //.Where(tc => tc.DestinationCurrencyCodeString == currency)
                .GroupBy(g =>
                {
                    if (g.DestinationAmount < 500)
                    {
                        return "<500";
                    }
                    else if (g.DestinationAmount >= 500 && g.DestinationAmount < 1000)
                    {
                        return ">=500 and <1,000";
                    }
                    else if (g.DestinationAmount >= 1000 && g.DestinationAmount < 2500)
                    {
                        return ">=1,000 and <2,500";
                    }
                    else if (g.DestinationAmount >= 2500 && g.DestinationAmount < 5000)
                    {
                        return ">=2,500 and <5,000";
                    }
                    else
                    {
                        return ">=5,000";
                    }
                })
                .Select(t =>
                {
                    return new
                    {
                        //id = t.Key,
                        groupIndex = getGroupIndex(t.Key),
                        //data = new
                        //{
                        //amount24h = t.Where(d => DateTime.UtcNow.Subtract(d.DateCreated).TotalHours <= 24).Sum(z => z.DestinationAmount),
                        //amount7d = t.Where(d => DateTime.UtcNow.Subtract(d.DateCreated).TotalDays <= 7).Sum(z => z.DestinationAmount),
                        //amount30d = t.Where(d => DateTime.UtcNow.Subtract(d.DateCreated).TotalDays <= 30).Sum(z => z.DestinationAmount),
                        count = t.Where(d => DateTime.UtcNow.Subtract(d.DateCreated).TotalHours <= hours).Count(),
                        //count7d = t.Where(d => DateTime.UtcNow.Subtract(d.DateCreated).TotalDays <= 7).Count(),
                        //count30d = t.Where(d => DateTime.UtcNow.Subtract(d.DateCreated).TotalDays <= 30).Count()
                        //}
                    };
                })
                .OrderBy(t => t.groupIndex);

            List<int> pieData = new List<int>();
            foreach(var data in resp)
            {
                pieData.Add(data.count);
            }

            return Ok(new { pieData, message = "" });
        }

        private static void GenerateTransactions(DateTime endDate, DateTime startDate, List<Transaction> transactions)
        {
            for (int i = 0; i < 6000; i++)
            {
                var trans = new Transaction();
                TimeSpan timeSpan = endDate - startDate;
                var randomTest = new Random();
                TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
                DateTime newDate = startDate + newSpan;
                trans.DateCreated = newDate;
                trans.IsAmountKnown = Convert.ToBoolean(randomTest.Next(0, 2));
                trans.OriginalAmount = randomTest.NextDouble();
                trans.DestinationAmount = randomTest.NextDouble();
                Array cryptoEnum = Enum.GetValues(typeof(CryptoProviders));
                trans.CryptoProvider = (CryptoProviders)cryptoEnum.GetValue(randomTest.Next(cryptoEnum.Length));

                Array currencyEnum = Enum.GetValues(typeof(CurrencyCodes));
                trans.DestinationCurrencyCode = (CurrencyCodes)currencyEnum.GetValue(randomTest.Next(cryptoEnum.Length));
                trans.OriginalCurrencyCode = (CurrencyCodes)currencyEnum.GetValue(randomTest.Next(cryptoEnum.Length));

                transactions.Add(trans);
            }
        }

        private static void GenerateApplications(DateTime endDate, DateTime startDate, List<Application> transactions)
        {
            for (int i = 0; i < 6000; i++)
            {
                var trans = new Application();
                TimeSpan timeSpan = endDate - startDate;
                var randomTest = new Random();
                TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
                DateTime newDate = startDate + newSpan;
                trans.DateCreated = newDate;

                transactions.Add(trans);
            }
        }
    }
}
