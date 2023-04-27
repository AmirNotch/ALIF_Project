using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public long INN { get; set; }
        public ICollection<Wallet> Wallet { get; set; } = new List<Wallet>(); 
    }
}