using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Embily.Models;

namespace Embily.Services
{
    public class CryptoAddressCreatorSim : ICryptoAddress
    {
        public async Task<string> GetNewAddressAsync(CurrencyCodes code, string accountNumber)
        {
            return await Task.Run(() =>
            {
                return $"sim {code} {Guid.NewGuid().ToString()} {code} sim";
            });
        }
    }
}
