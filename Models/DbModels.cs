using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CBT.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public string Username  { get; set; }
        public string PW  { get; set; }
        public double Wallet  { get; set; }
    }

    public class Auction : BaseEntity
    {
        [Key]
        public int AuctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Bid { get; set; }
        public string Seller { get; set; }
        public int UserId { get; set; }
        public bool Paid { get; set; }
        public List<Bid> Bids { get; set; }
    }

    public class Bid : BaseEntity
    {
        [Key]
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public User Bidder { get; set; }
    }
}