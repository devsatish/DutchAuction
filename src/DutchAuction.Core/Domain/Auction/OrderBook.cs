﻿namespace DutchAuction.Core.Domain.Auction
{
    public class OrderBook
    {
        public Order[] Orders { get; set; }
        public double CurrentPrice { get; set; }
        public double CurrentInMoneyVolume { get; set; }
        public double CurrentOutOfTheMoneyVolume { get; set; }
    }
}