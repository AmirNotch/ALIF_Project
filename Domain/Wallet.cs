using System;
using System.Collections.Generic;

namespace Domain
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Balance { get; set; }
        public bool Identified { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<History> History { get; set; } = new List<History>();
    }
}