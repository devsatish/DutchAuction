﻿using System;
using System.Threading.Tasks;
using Common.Log;
using DutchAuction.Core;
using DutchAuction.Core.Domain.Auction;
using DutchAuction.Core.Services.Auction;
using DutchAuction.Services.RabbitMq;
using Lykke.RabbitMqBroker.Publisher;

namespace DutchAuction.Services.Auction
{
    public class PriceHistoryService : IPriceHistoryService
    {
        private readonly ILog _log;
        private readonly ApplicationSettings.RabbitSettings _rabbitSettings;
        private RabbitMqPublisher<PriceUpdatedMessage> _publisher;

        public PriceHistoryService(ILog log, ApplicationSettings.RabbitSettings rabbitSettings)
        {
            _log = log;
            _rabbitSettings = rabbitSettings;
        }

        public void Start()
        {
            _publisher = new RabbitMqPublisher<PriceUpdatedMessage>(new RabbitMqPublisherSettings
                {
                    ConnectionString = _rabbitSettings.ConnectionString,
                    ExchangeName = _rabbitSettings.ExchangeName
                })
                .SetSerializer(new JsonMessageSerializer<PriceUpdatedMessage>())
                .SetPublishStrategy(new DefaultFnoutPublishStrategy())
                .SetLogger(_log)
                .Start();
        }

        public Task PublishAsync(double price, double inMoneyVolume, double outOfTheMoneyVolume)
        {
            // TODO: Disable publishing while there are no subscribers
            return Task.FromResult(0);
            //await _publisher.ProduceAsync(new PriceUpdatedMessage
            //{
            //    Price = price,
            //    InMoneyVolume = inMoneyVolume,
            //    OutOfTheMoneyVolume = outOfTheMoneyVolume,
            //    Timestamp = DateTime.UtcNow
            //});
        }
    }
}