using System;
using System.Collections.Generic;
using Domain;

namespace Application.Wallets
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Balance { get; set; }
        public bool Identified { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<History> Histories { get; set; } = new List<History>();
    }
}