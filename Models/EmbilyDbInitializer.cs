using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Embily.Models
{
    public interface IEmbilyDbInitializer
    {
        Task SeedAsync();
    }

    public struct WellKnowIds
    {
        // AC = Andrei Chabokha, H = Hotmail, G = Gmail, A? = Affiliate1, etc

        public const string ACUserId_H = "1E9198F8-A351-4A1F-8C4C-E99C17B916D6";
        public const string ACUserId_G = "21478290-36D3-4C65-8FD0-C9CDD94633BA";
        public const string UserId_A1 = "A10412FB-FA61-4D48-9C96-CF7EB566F99E";
        public const string UserId_A2 = "A204CC18-1C5C-4657-9D8B-9FAE9EE2E99E";
        public const string UserId_A3 = "A304CC18-1C5C-4657-9D8B-9FAE9EE2E99E";

        public const string ACMasterCard_USD_H = "890B8023-98CE-400C-9E3E-F8B4BF139F04";
        public const string ACMasterCard_USD_G = "223EA3E3-BF4F-4B92-AF5B-51E413F200A9";
        public const string ACMasterCard_EUR_H = "E9C80600-BF87-49AF-BEFD-65B15DBEE8A9";
    }

    public class EmbilyDbInitializer : IEmbilyDbInitializer
    {
        private readonly EmbilyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManage;

        public EmbilyDbInitializer(EmbilyDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManage = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Roles.AnyAsync())
            {
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.CustomerSupport.ToString() });
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.Affiliate.ToString() });
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.Client_Registered.ToString() });
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.Client_KYC.ToString() });
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.Client_LB_Club.ToString() });

                {
                    {
                        var program = new Program
                        {
                            ProgramId = "sandbox",
                            Title = "Sandbox",
                            Domain = "sandbox.embily.com"
                        };
                        await _context.Programs.AddAsync(program);
                        await _context.SaveChangesAsync();
                    }

                    {
                        var program = new Program
                        {
                            ProgramId = "embily",
                            Title = "Embily",
                            Domain = "services.embily.com"
                        };
                        await _context.Programs.AddAsync(program);
                        await _context.SaveChangesAsync();
                    }
                }

                {
                    var email = "admin@embily.com";
                    var password = "Pa$$w0rd!";
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        UserNumber = 1000000005,
                        FirstName = "Admin",
                        LastName = "Embily",
                        EmailConfirmed = true,
                    };

                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                {
                    var email = "support@embily.com";
                    var password = "Pa$$w0rd!";
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        UserNumber = 1000000007,
                        FirstName = "Support",
                        LastName = "Embily",
                        EmailConfirmed = true,
                    };

                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, Roles.CustomerSupport.ToString());
                }

                // H Account 
                ApplicationUser user_H;

                {
                    var email = "achabokha@hotmail.com";
                    var password = "Pa$$w0rd!";
                    var user = new ApplicationUser
                    {
                        Id = WellKnowIds.ACUserId_H,
                        Email = email,
                        UserName = email,
                        UserNumber = 1000000009,
                        FirstName = "Andrei",
                        LastName = "Chabokha",
                        PhoneNumber = "+66630319969",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        ProgramId = "sandbox"
                    };

                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, Roles.Client_KYC.ToString());
                    await _userManager.AddToRoleAsync(user, Roles.Client_LB_Club.ToString());
                    await _userManager.AddToRoleAsync(user, Roles.Affiliate.ToString());

                    var application1 = new Application
                    {
                        UserId = WellKnowIds.ACUserId_H,
                        AccountType = AccountTypes.MasterCardPrepaid,
                        CurrencyCode = CurrencyCodes.USD,
                        ApplicationId = Guid.NewGuid().ToString(),
                        ApplicationNumber = "1000000009-0001",
                        ProviderName = AccountProviders.Virtual,
                        ProviderAccountNumber = "5387320602974168",
                        ProviderUserId = "8267200893",
                        Status = ApplicationStatus.Shipped,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                    };

                    var application2 = new Application
                    {
                        UserId = WellKnowIds.ACUserId_H,
                        AccountType = AccountTypes.MasterCardPrepaid,
                        CurrencyCode = CurrencyCodes.EUR,
                        ApplicationId = Guid.NewGuid().ToString(),
                        ApplicationNumber = "1000000009-0002",
                        ProviderName = AccountProviders.Virtual,
                        ProviderAccountNumber = "5387320601794310",
                        ProviderUserId = "8267200894",
                        Status = ApplicationStatus.Shipped,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                    };

                    await _context.Applications.AddRangeAsync(application1, application2);
                    await _context.SaveChangesAsync();

                    user.Accounts = new List<Account>()
                    {
                        new Account
                        {
                            AccountId = WellKnowIds.ACMasterCard_USD_H,
                            AccountType = AccountTypes.MasterCardPrepaid,
                            AccountStatus = AccountStatuses.Active,
                            AccountName = "Super Cool MasterCard",
                            AccountNumber = "1000000009-0001",
                            CurrencyCode = CurrencyCodes.USD,
                            ProviderName = AccountProviders.Virtual,
                            ProviderAccountNumber = "5387320602974168",
                            ProviderUserId = "8267200893",
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Phone = user.PhoneNumber,
                            User = user,
                            Transactions = new List<Transaction>()
                            {
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_H,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "dbbc0f22499ede6978893a32fa358d0b43dce0d33d11d40af4069ea59bb0f3b9",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.14974702,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.14974702) * 60000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               }
                            },
                            CryptoAddreses = new List<CryptoAddress>()
                            {
                                new CryptoAddress
                                {
                                    AccountId = WellKnowIds.ACMasterCard_USD_H,
                                    Address = "39QW9xdtwBMGMsvYASKYJ83jKeMZX6k676",
                                    CurrencyCode = CurrencyCodes.BTC,
                                },

                                new CryptoAddress
                                {
                                    AccountId = WellKnowIds.ACMasterCard_USD_H,
                                    Address = "LQMKWja6xwwxXQnJVRdLW3LQQZ4mZgXif5",
                                    CurrencyCode = CurrencyCodes.LTC,
                                }
                            }
                        },
                        //new Account
                        //{
                        //    AccountId = WellKnowIds.ACMasterCard_EUR_H,
                        //    AccountType = AccountTypes.MasterCardPrepaid,
                        //    AccountStatus = AccountStatuses.Active,
                        //    AccountName = "Embily MasterCard",
                        //    AccountNumber = "1000000009-0002",
                        //    CurrencyCode = CurrencyCodes.EUR,
                        //    ProviderName = AccountProviders.Test,
                        //    ProviderAccountNumber = "5387320601794310",
                        //    ProviderUserId = "8267200894",
                        //    User = user,
                        //    FirstName = user.FirstName,
                        //    LastName = user.LastName,
                        //    Email = user.Email,
                        //    Phone = user.PhoneNumber,
                        //},
                        new Account
                        {
                            AccountId = Guid.NewGuid().ToString(),
                            AccountType = AccountTypes.Affiliate,
                            AccountStatus = AccountStatuses.Active,
                            AccountName = "Super Affiliate (USD)",
                            AccountNumber = "1000000009-1001",
                            CurrencyCode = CurrencyCodes.USD,
                            ProviderName = AccountProviders.Embily,
                            ProviderAccountNumber = "",
                            ProviderUserId = "",
                            User = user,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Phone = user.PhoneNumber,
                        },
                        new Account
                        {
                            AccountId = Guid.NewGuid().ToString(),
                            AccountType = AccountTypes.Affiliate,
                            AccountStatus = AccountStatuses.Active,
                            AccountName = "Super Affiliate (EUR)",
                            AccountNumber = "1000000009-1002",
                            CurrencyCode = CurrencyCodes.EUR,
                            ProviderName = AccountProviders.Embily,
                            ProviderAccountNumber = "",
                            ProviderUserId = "",
                            User = user,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Phone = user.PhoneNumber,
                        }
                    };

                    user_H = user;
                }

                // G account 
                {
                    var email = "achabokha@gmail.com";
                    var password = "Pa$$w0rd!";
                    var user = new ApplicationUser
                    {
                        Id = WellKnowIds.ACUserId_G,
                        Email = email,
                        UserName = email,
                        UserNumber = 1000000010,
                        FirstName = "Andrei",
                        LastName = "Chabokha",
                        PhoneNumber = "+66630319969",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        ProgramId = "sandbox"
                    };

                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, Roles.Client_LB_Club.ToString());
                    await _userManager.AddToRoleAsync(user, Roles.Client_KYC.ToString());

                    var application = new Application
                    {
                        UserId = WellKnowIds.ACUserId_G,
                        AccountType = AccountTypes.MasterCardPrepaid,
                        CurrencyCode = CurrencyCodes.USD,
                        ApplicationId = Guid.NewGuid().ToString(),
                        ApplicationNumber = "1000000010-0001",
                        ProviderName = AccountProviders.Virtual,
                        ProviderAccountNumber = "5387320608618884",
                        ProviderUserId = "8267200899",
                        Status = ApplicationStatus.Approved,
                    };

                    await _context.Applications.AddAsync(application);
                    await _context.SaveChangesAsync();

                    user.Accounts = new List<Account>()
                    {
                        new Account
                        {
                            AccountId = WellKnowIds.ACMasterCard_USD_G,
                            AccountType = AccountTypes.MasterCardPrepaid,
                            AccountStatus = AccountStatuses.Active,
                            AccountName = "Embily MasterCard",
                            AccountNumber = "1000000010-0001",
                            CurrencyCode = CurrencyCodes.USD,
                            ProviderName = AccountProviders.Virtual,
                            ProviderAccountNumber = "5387320608618884",
                            ProviderUserId = "8267200101",
                            User = user,
                            Transactions = new List<Transaction>()
                            {
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                               },
                                new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-10)
                               },
                                 new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-10)
                               },
                                  new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-10)
                               },
                                   new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-10)
                               },
                                    new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-5)
                               },
                                    new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-5)
                               },
                                    new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddHours(-5)
                               },
                               new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddDays(-10)
                               },
                                new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddDays(-10)
                               }
                                ,
                                new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddDays(-5)
                               },
                                 new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddDays(-6)
                               },
                                  new Transaction
                               {
                                   TransactionId = Guid.NewGuid().ToString(),
                                   AccountId = WellKnowIds.ACMasterCard_USD_G,
                                   TxnType = TxnTypes.CREDIT,
                                   TxnCode = TxnCodes.LOAD,
                                   CryptoTxnId = "80f2b1c10bb707f072bd5c17b4bf70e0f1709239f38dd33be187d66f32c6b9f3",
                                   CryptoAddress = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                   CryptoProvider = CryptoProviders.Bitfinex,
                                   OriginalAmount = 0.5084261,
                                   OriginalCurrencyCode = CurrencyCodes.BTC,
                                   DestinationAmount = (0.5084261) * 6000,
                                   DestinationCurrencyCode = CurrencyCodes.USD,
                                   IsAmountKnown = true,
                                   Status = TxnStatus.Complete,
                                   DateCreated = DateTime.Now.AddDays(-6)
                               }
                            },
                            CryptoAddreses = new List<CryptoAddress>()
                            {
                                new CryptoAddress
                                {
                                    AccountId = WellKnowIds.ACMasterCard_USD_G,
                                    Address = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                    CurrencyCode = CurrencyCodes.BTC,
                                }
                            }
                        },
                    };
                }

                // Affiliated Accounts 

                // Account A1
                var user_A1 = await CreateUserAndAccountMin(WellKnowIds.UserId_A1, 1000000011, CurrencyCodes.USD, "affiliated1@gmail.com", "Pa$$w0rd!", "Joe", "Mac");

                // Account A2
                var user_A2 = await CreateUserAndAccountMin(WellKnowIds.UserId_A2, 1000000012, CurrencyCodes.USD, "affiliated2@gmail.com", "Pa$$w0rd!", "Lady", "Gaga");

                var user_A3 = await CreateUserAndAccountMin(WellKnowIds.UserId_A3, 1000000013, CurrencyCodes.EUR, "affiliated3@gmail.com", "Pa$$w0rd!", "Till", "Lindemann");

                user_A1.AffiliatedWithUserId = user_H.Id;
                user_A2.AffiliatedWithUserId = user_H.Id;
                user_A3.AffiliatedWithUserId = user_H.Id;

                await _context.SaveChangesAsync();

                await EmailInvitesAntdTokens(user_H);
            }
        }

        private async Task EmailInvitesAntdTokens(ApplicationUser user_H)
        {
            // add invites and tokens 
            var ae = new AffiliateEmail
            {
                AffiliateEmailId = Guid.NewGuid().ToString(),
                Email = "joe@dow.com",
                NormalizedEmail = "JOE@DOW.COM",
                UserId = user_H.Id,
            };

            var at = new AffiliateToken
            {
                AffiliateTokenId = Guid.NewGuid().ToString(),
                Token = "ToKeN",
                NormalizedToken = "TOKEN",
                Description = "The ultimate test token",
                IsActive = true,
                UserId = user_H.Id,
            };

            await _context.AffiliateEmails.AddAsync(ae);
            await _context.AffiliateTokens.AddAsync(at);

            await _context.SaveChangesAsync();
        }

        async Task<ApplicationUser> CreateUserAndAccountMin(string userId, long userNumber, CurrencyCodes currency, string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser
            {
                Id = userId,
                UserNumber = userNumber,
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = "+66630319969",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                ProgramId = "sandbox"
            };

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, Roles.Client_KYC.ToString());

            var application = new Application
            {
                User = user,
                ApplicationId = Guid.NewGuid().ToString(),
                AccountType = AccountTypes.MasterCardPrepaid,
                CurrencyCode = currency,
                ApplicationNumber = $"{userNumber}-0001",
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Phone = "+66630319969",
                ProviderName = AccountProviders.Virtual,
                ProviderAccountNumber = "5387320608618884",
                ProviderUserId = "8267200202",
                Status = ApplicationStatus.Shipped,
            };

            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            var accountId = Guid.NewGuid().ToString();
            user.Accounts = new List<Account>()
                {
                    new Account
                    {
                        User = user,
                        AccountId = accountId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        AccountType = AccountTypes.MasterCardPrepaid,
                        AccountStatus = AccountStatuses.Active,
                        AccountName = "Embily MasterCard",
                        AccountNumber = $"{userNumber}-0001",
                        CurrencyCode = currency,
                        ProviderName = AccountProviders.Virtual,
                        ProviderAccountNumber = "5387320608618884",
                        ProviderUserId = "82672008303",
                        CryptoAddreses = new List<CryptoAddress>()
                        {
                            new CryptoAddress
                            {
                                AccountId = accountId,
                                Address = "3HXVw4GAXoHfbgTFvtQbM31s2GaFq35vzb",
                                CurrencyCode = CurrencyCodes.BTC,
                            }
                        }
                    },
                };

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
