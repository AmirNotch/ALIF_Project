﻿using System;
using System.Linq;
using System.Threading.Tasks;
using API.Filter;
using API.Helpers;
using API.Services;
using Application.Wallets;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class WalletController : BaseApiController
    {
        private readonly DataContext context;
        private readonly IUriService uriService;
        
        public WalletController(DataContext context, IUriService uriService)
        {
            this.context = context;
            this.uriService = uriService;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetWallets([FromQuery]PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await context.Wallets
                .Where(l => l.AppUser.Id == filter.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await context.Wallets.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Wallet>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        
        [HttpGet("search/{Id}")]
        public async Task<ActionResult> GetWallets([FromQuery]PaginationFilter filter, Guid Id)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await context.Wallets
                .Where(l => l.AppUser.Id == filter.Id)
                .Where(o => o.Id == Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await context.Wallets.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Wallet>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        
        
        [HttpPost("{id}")]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet, string id)
        {
            return Ok(await Mediator.Send(new CreateWallet.Command {Wallet = wallet, Id = id}));
        }
    }
}