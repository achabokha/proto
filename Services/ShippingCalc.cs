using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Embily.Gateways.DHL;
using Embily.Gateways.DHL.Model;
using Embily.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;

namespace Embily.Services
{
    public class ShippingCalc : IShippingCalc
    {
        private readonly IHostingEnvironment _env;
        private readonly IMapper _mapper;
        private readonly EmbilyDbContext _ctx;
        private readonly DHLAPI _api;

        public ShippingCalc()
        {
            _api = new DHLAPI();
        }

        public ShippingCalc(IHostingEnvironment env, IMapper mapper, EmbilyDbContext ctx)
        {
            _env = env;
            _ctx = ctx;
            _mapper = mapper;
            _api = new DHLAPI();
        }

        public async Task<ShippingDetail> GetShippingOptionsAsync(CurrencyCodes currencyCode, string countryCode, string postalCode)
        {
            if (countryCode == "TH")
            {
                return new ShippingDetail()
                {
                    Description = "averages 2-3 business days",
                    ShippingOptions = new List<ShippingOption>
                    {
                        new ShippingOption {
                            Carrier = "Kerry Express",
                            CurrencyCode = currencyCode.ToString(),
                            Price = (currencyCode == CurrencyCodes.USD)? 15 : 12, // USD 15, EUR 12
                            Title = "KerryExpress Shipping (flat rate)",
                            Description = "averages 2-3 business days"
                        }
                   }
                };
            }
            else
            {
                return await GetDHLFlatRates(currencyCode, countryCode, postalCode);
            }
        }
        
        async Task<ShippingDetail> GetDHLFlatRates(CurrencyCodes currencyCode, string countryCode, string postalCode)
        {
            return await GetDHLShippingOptions(currencyCode, countryCode, postalCode);

            //return new List<ShippingOption>
            //    {
            //        new ShippingOption {
            //            Carrier = "DHL",
            //            CurrencyCode = currencyCode.ToString(),
            //            Price = (currencyCode == CurrencyCodes.USD)? 50 : 45, // USD 50, EUR 45 GetDHLShippingOptions(currencyCode, countryCode, postalCode).Result[0].Price,//
            //            Title = "DHL Express Worldwide (flat rate)",
            //            Description = "averages 2-3 business days"
            //        },
            //        //new ShippingOption {
            //        //    Carrier = "DHL",
            //        //    CurrencyCode = currencyCode.ToString(),
            //        //    Price = (currencyCode == CurrencyCodes.USD)? 30 : 25, // USD 30, EUR 25
            //        //    Title = "DHL Priority Worldwide (flat rate)",
            //        //    Description = "averages 5-10 business days"
            //        //}
            //    };
        }

        public async Task<ShippingDetail> GetDHLShippingOptions(CurrencyCodes currencyCode, string countryCode, string postalCode)
        {
            try
            {
                dynamic response = await _api.CalculateRates(new RequestCalculateRates()
                {
                    Xml = new Embily.Gateways.DHL.Model.req.Xml()
                    {
                        Encoding = "UTF-8",
                        Version = "1.0"
                    },
                    PDctRequest = new Embily.Gateways.DHL.Model.req.PDctRequest()
                    {
                        XmlnsP = "http://www.dhl.com",
                        XmlnsP1 = "http://www.dhl.com/datatypes",
                        XmlnsP2 = "http://www.dhl.com/DCTRequestdatatypes",
                        XmlnsXsi = "http://www.w3.org/2001/XMLSchema-instance",
                        XsiSchemaLocation = "http://www.dhl.com DCT-req.xsd ",
                        GetQuote = new Embily.Gateways.DHL.Model.req.GetQuote()
                        {
                            Request = new Embily.Gateways.DHL.Model.req.Request()
                            {
                                ServiceHeader = new Embily.Gateways.DHL.Model.req.ServiceHeader()
                                {
                                    MessageReference = "1234567890123456789012345678901",
                                    MessageTime = DateTimeOffset.UtcNow,
                                }
                            },
                            From = new Embily.Gateways.DHL.Model.req.From()
                            {
                                CountryCode = "TH",
                                Postalcode = "10110"
                            },
                            To = new Embily.Gateways.DHL.Model.req.From()
                            {
                                CountryCode = countryCode,// = countryCode.Substring(0, countryCode.Length - 1),//"CO",
                                Postalcode = postalCode//"110861"
                            },
                            BkgDetails = new Embily.Gateways.DHL.Model.req.BkgDetails()
                            {
                                PaymentCountryCode = "TH",
                                Date = DateTimeOffset.UtcNow,
                                ReadyTime = "PT10H21M",
                                ReadyTimeGMTOffset = "+01:00",
                                DimensionUnit = "CM",
                                WeightUnit = "KG",
                                Pieces = new Embily.Gateways.DHL.Model.req.Pieces()
                                {
                                    Piece = new Embily.Gateways.DHL.Model.req.Piece()
                                    {
                                        PieceId = "1",
                                        Height = "11",
                                        Width = "22",
                                        Depth = "1",
                                        Weight = "0.020"

                                    }
                                },
                                IsDutiable = "N"
                            }
                        }
                    }
                });

                var QtdShp = response["res:DCTResponse"]["GetQuoteResponse"]["BkgDetails"]["QtdShp"];

                ShippingDetail shippingDetail = new ShippingDetail();
                shippingDetail.ShippingOptions = new List<ShippingOption>();
                double factorPrice = (currencyCode == CurrencyCodes.USD) ? 33 : 38;

                if (QtdShp is JArray)
                {
                    foreach (var shipping in QtdShp)
                    {
                        
                        var convertPrice = Math.Round(Convert.ToDouble(shipping["ShippingCharge"].ToString()) / factorPrice, 2);
                        var transitDays = Convert.ToInt32(shipping["TotalTransitDays"].ToString());

                        if (string.IsNullOrEmpty(shippingDetail.Description))
                        {
                            shippingDetail.Description = $"averages {transitDays} - {transitDays + 14} business days";
                        }

                        shippingDetail.ShippingOptions.Add(new ShippingOption()
                        {
                            Carrier = "DHL",
                            Price = convertPrice,
                            CurrencyCode = currencyCode.ToString(),
                            Title = shipping["ProductShortName"].ToString(),
                            Description = $"averages {transitDays} - {transitDays + 14} business days"
                            //shipping["DeliveryDate"].ToString()
                            //TotalTransitDays = shipping["TotalTransitDays"].ToString(),
                        });
                    }

                    shippingDetail.ShippingOptions = shippingDetail.ShippingOptions.OrderByDescending(s => s.Price).ToList();
                }
                else
                {
                    var convertPrice = Math.Round(Convert.ToDouble(QtdShp["ShippingCharge"].ToString()) / factorPrice, 2);
                    var transitDays = Convert.ToInt32(QtdShp["TotalTransitDays"].ToString());

                    if (string.IsNullOrEmpty(shippingDetail.Description))
                    {
                        shippingDetail.Description = $"averages {transitDays} - {transitDays + 14} business days";
                    }

                    shippingDetail.ShippingOptions.Add(new ShippingOption()
                    {
                        Carrier = "DHL",
                        Price = convertPrice,
                        CurrencyCode = currencyCode.ToString(),
                        Title = QtdShp["ProductShortName"].ToString(),
                        Description = $"averages {transitDays} - {transitDays + 14} business days"
                        //QtdShp["CurrencyCode"].ToString()
                        //QtdShp["DeliveryDate"].ToString()
                        //TotalTransitDays = QtdShp["TotalTransitDays"].ToString(),
                    });
                }

                return shippingDetail;
            }
            catch
            {
                return new ShippingDetail()
                {
                    Description = "averages 2-14 business days",
                    ShippingOptions = new List<ShippingOption>
                    {
                        new ShippingOption {
                            Carrier = "DHL",
                            CurrencyCode = currencyCode.ToString(),
                            Price = (currencyCode == CurrencyCodes.USD)? 50 : 45, // USD 50, EUR 45
                            Title = "DHL Express Worldwide (flat rate)",
                            Description = "averages 2-3 business days"
                        },
                    },
                };
                
            }
            
        }

        List<ShippingOption> GetTestOptions()
        {
            return new List<ShippingOption>
            {
                new ShippingOption { CurrencyCode = CurrencyCodes.USD.ToString(), Price = 34.95, Title = "DHL Global Priority Shipping", Description = "Thursday, July 26" },
                new ShippingOption { CurrencyCode = CurrencyCodes.USD.ToString(), Price = 18.95, Title = "DHL Expedited Shipping", Description = "averages 5-10 business days" }
            };
        }
    }
}