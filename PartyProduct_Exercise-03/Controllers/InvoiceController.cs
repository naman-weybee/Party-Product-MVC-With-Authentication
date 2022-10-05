﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyProduct_Exercise_03.Models;
using PartyProduct_Exercise_03.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository = null;
        private readonly IAssignPartyRepository _assignPartyRepository = null;

        public InvoiceController(IInvoiceRepository invoiceRepository, IAssignPartyRepository assignPartyRepository)
        {
            _invoiceRepository = invoiceRepository;
            _assignPartyRepository = assignPartyRepository;
        }

        [Authorize]
        public async Task<IActionResult> Invoice(int id, bool isAdded = false, int grandTotal = 0, int isSuccess = 0)
        {
            if (isAdded)
            {
                ViewBag.Invoice = await _invoiceRepository.GetAllInvoice();
                ViewBag.display = true;
                ViewBag.grandTotal = grandTotal;
                ViewBag.IsSuccess = isSuccess;
                var invoicemodel = new InvoiceModel()
                {
                    PartyId = id
                };
                ViewBag.IsDisabled = true;
                return View(invoicemodel);
            }
            ViewBag.display = false;
            ViewBag.IsDisabled = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Invoice(int PartyId, InvoiceModel invoiceModel)
        {
            int grandTotal = 0;
            if (ModelState.IsValid)
            {
                grandTotal = await _invoiceRepository.InvoiceAdd(invoiceModel);
            };
            return RedirectToAction(nameof(Invoice), new { id = PartyId, isAdded = true, grandTotal = grandTotal, isSuccess = 1 });
        }

        public IActionResult InvoiceClose(bool isAdded = false)
        {
            return RedirectToAction(nameof(Invoice), new { isAdded = false });
        }

        [HttpGet]
        public async Task<JsonResult> BindProductDetails(int PartyId)
        {
            var productDetails = await _invoiceRepository.BindProduct(PartyId);

            return Json(productDetails);
        }

        [HttpGet]
        public async Task<JsonResult> BindRateDetails(int ProductId)
        {
            int rate = await _invoiceRepository.BindRate(ProductId);

            return Json(rate);
        }

        public async Task<IActionResult> ClrearInvoice()
        {
            await _invoiceRepository.ClrearInvoiceAsync();

            return RedirectToAction(nameof(Invoice), new { isAdded = false });
        }
    }
}
