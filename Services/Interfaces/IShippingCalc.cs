using Embily.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{
    public class ShippingDetail
    {
        public string Description { get; set; }

        public List<ShippingOption> ShippingOptions { get; set; }
    }
    public class ShippingOption
    {
        public string Carrier { get; set; }

        public double Price { get; set; }

        public string CurrencyCode { get; set; }

        /// <summary>
        /// "DHL Expedited Shipping" or "DHL Global Priority Shipping"
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// "averages 5-10 business days" or "Thursday, July 26"
        /// </summary>
        public string Description { get; set; }

        public bool Equals(ShippingOption other)
        {
            return this.Carrier == other.Carrier
                && this.Price == other.Price
                && this.CurrencyCode == other.CurrencyCode
                && this.Title == other.Title
                && this.Description == other.Description;
        }

        public override bool Equals(object obj)
        {
            return this.Equals((ShippingOption)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public interface IShippingCalc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyCode">USD | EUR</param>
        /// <param name="countryCode">3 characters ISO country code</param>
        /// <param name="PostalCode">postal code</param>
        /// <returns></returns>
        Task<ShippingDetail> GetShippingOptionsAsync(CurrencyCodes currencyCode, string countryCode, string postalCode);
    }
}
