using Embily.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Embily.Services
{
    public interface ICryptoAddress
    {
        Task<string> GetNewAddressAsync(CurrencyCodes code, string accountNumber);
    }
}
