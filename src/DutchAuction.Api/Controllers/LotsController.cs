﻿using System.Linq;
using System.Threading.Tasks;
using DutchAuction.Api.Models;
using DutchAuction.Core.Domain;
using DutchAuction.Core.Services;
using DutchAuction.Services;
using Microsoft.AspNetCore.Mvc;

namespace DutchAuction.Api.Controllers
{
    /// <summary>
    /// Controller for lots
    /// </summary>
    [Route("api/[controller]")]
    public class LotsController : Controller
    {
        private readonly ApplicationSettings _settings;
        private readonly IAuctionLotCacheService _auctionLotCacheService;
        private readonly AuctionLotManager _auctionLotManager;

        public LotsController(
            ApplicationSettings settings,
            IAuctionLotCacheService auctionLotCacheService,
            AuctionLotManager auctionLotManager)
        {
            _settings = settings;
            _auctionLotCacheService = auctionLotCacheService;
            _auctionLotManager = auctionLotManager;
        }

        /// <summary>
        /// Get order book by asset
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("orderbook")]
        public Order[] Get()
        {
            return _auctionLotCacheService.GetOrderbook();
        }

        /// <summary>
        /// Add auction lot
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ResponseModel> AddLot([FromBody]AuctionLotModel model)
        {
            if (string.IsNullOrEmpty(model.ClientId))
            {
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InvalidInputField,
                    $"{nameof(model.ClientId)} is required");
            }

            if (string.IsNullOrEmpty(model.AssetId))
            {
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InvalidInputField,
                    $"{nameof(model.AssetId)} is required");
            }

            if (!_settings.DutchAuction.Assets.Contains(model.AssetId))
            {
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InvalidInputField,
                    $"wrong {nameof(model.AssetId)}");
            }

            if (model.Price <= 0)
            {
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InvalidInputField,
                    $"wrong {nameof(model.Price)}");
            }

            if (model.Volume <= 0)
            {
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InvalidInputField,
                    $"wrong {nameof(model.Volume)}");
            }

            //TODO: validate model.ClientId
            await _auctionLotManager.AddAsync(model.ClientId, model.AssetId, model.Price, model.Volume);

            return ResponseModel.CreateOk();
        }
    }
}
