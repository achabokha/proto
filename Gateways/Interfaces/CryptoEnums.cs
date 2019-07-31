using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Embily.Gateways
{
    public enum CryptoCurrencyCodes
    {
        BTC,
        LTC,
        ETH,
    }

    public enum CryptoOrderSymbols
    {
        [EnumMember(Value = "btcusd")]
        BTCUSD,

        [EnumMember(Value = "btceur")]
        BTCEUR,

        [EnumMember(Value = "ltcusd")]
        LTCUSD,

        [EnumMember(Value = "ltceur")]
        LTCEUR,

        [EnumMember(Value = "ethusd")]
        ETHUSD,

        [EnumMember(Value = "etheur")]
        ETHEUR

    }
}
