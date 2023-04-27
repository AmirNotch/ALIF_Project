using System;

namespace Domain
{
    public class History
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Sum { get; set; }
        public DateTime CreatedAt { get; set; } = new DateTime();
        public Wallet Wallet { get; set; }
    }
}