using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any() && !context.Wallets.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Name = "Bob",
                        Surname = "Brown",
                        INN = 038889022,
                        Email = "Bob-Brown@mail.com"
                    },

                    new AppUser
                    {
                        Name = "Jane",
                        Surname = "Yellow",
                        INN = 037779122,
                        Email = "Jane_Yellow@list.com"
                    },

                    new AppUser
                    {
                        Name = "Tom",
                        Surname = "White",
                        INN = 036669021,
                        Email = "Tom_White@gmail.com"
                    }
                };
                
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }

                var listWallet = new List<Wallet>
                {
                    new Wallet
                    {
                        AppUser = users[0],
                        Value = "TJS",
                        Balance = 10000,
                        Identified = false
                    },

                    new Wallet
                    {
                        AppUser = users[0],
                        Value = "TJS",
                        Balance = 100000,
                        Identified = true
                    },

                    new Wallet
                    {
                        AppUser = users[0],
                        Value = "USD",
                        Balance = 10000,
                        Identified = false
                    },



                    new Wallet
                    {
                        AppUser = users[1],
                        Value = "TJS",
                        Balance = 10000,
                        Identified = true
                    },

                    new Wallet
                    {
                        AppUser = users[1],
                        Value = "TJS",
                        Balance = 100000,
                        Identified = true
                    },

                    new Wallet
                    {
                        AppUser = users[1],
                        Value = "RUB",
                        Balance = 10000,
                        Identified = false
                    },



                    new Wallet
                    {
                        AppUser = users[2],
                        Value = "TJS",
                        Balance = 9000,
                        Identified = false
                    },

                    new Wallet
                    {
                        AppUser = users[2],
                        Value = "TJS",
                        Balance = 100000,
                        Identified = true
                    },

                    new Wallet
                    {
                        AppUser = users[2],
                        Value = "AED",
                        Balance = 10000,
                        Identified = false
                    },
                };

                await context.Wallets.AddRangeAsync(listWallet);
                await context.SaveChangesAsync();
            }
        }
    }
}