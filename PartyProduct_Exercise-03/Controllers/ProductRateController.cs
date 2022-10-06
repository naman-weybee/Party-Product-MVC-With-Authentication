using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyProduct_Exercise_03.Models;
using PartyProduct_Exercise_03.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Controllers
{
    public class ProductRateController : Controller
    {
        private readonly IProductRateRepository _productRateRepository = null;

        public ProductRateController(IProductRateRepository productRateRepository)
        {
            _productRateRepository = productRateRepository;
        }

        public async Task<IActionResult> ProductRate(string SortOrder)
        {

            ViewData["SortById"] = (SortOrder == "DescById" || SortOrder == "AscByProduct" || SortOrder == "DescByProduct" || SortOrder == "AscByRate" || SortOrder == "DescByRate" || SortOrder == "AscByDateOfRate" || SortOrder == "DescByDateOfRate") ? "AscById" : "DescById";
            ViewData["SortByProduct"] = SortOrder == "AscByProduct" ? "DescByProduct" : "AscByProduct";
            ViewData["SortByRate"] = SortOrder == "AscByRate" ? "DescByRate" : "AscByRate";
            ViewData["SortByDateOfRate"] = SortOrder == "AscByDateOfRate" ? "DescByDateOfRate" : "AscByDateOfRate";

            var data = await _productRateRepository.GetAllProductRate();

            switch (SortOrder)
            {
                case "AscById":
                    ViewBag.AscId = "Asc";
                    return View(data);
                case "DescById":
                    ViewBag.AscId = "Desc";
                    return View(data.OrderByDescending(x => x.Id));
                case "AscByProduct":
                    ViewBag.AscProduct = "Asc";
                    return View(data.OrderBy(x => x.Product.ProductName));
                case "DescByProduct":
                    ViewBag.AscProduct = "Desc";
                    return View(data.OrderByDescending(x => x.Product.ProductName));
                case "AscByRate":
                    ViewBag.AscRate = "Asc";
                    return View(data.OrderBy(x => x.Rate));
                case "DescByRate":
                    ViewBag.AscRate = "Desc";
                    return View(data.OrderByDescending(x => x.Rate));
                case "AscByDateOfRate":
                    ViewBag.AscDateOfRate = "Asc";
                    return View(data.OrderBy(x => x.DateOfRate));
                case "DescByDateOfRate":
                    ViewBag.AscDateOfRate = "Desc";
                    return View(data.OrderByDescending(x => x.DateOfRate));
                default:
                    break;
            }
            return View(data);
        }

        [Authorize]
        public IActionResult ProductRateAdd(int isSuccess = 0, string productName = null, int productRate = 0, int productRateId = 0)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.productName = productName;
            ViewBag.productRate = productRate;
            ViewBag.productRateId = productRateId;
            ViewBag.IsDisabled = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductRateAdd(ProductRateModel productRateModel)
        {
            if (ModelState.IsValid)
            {
                var model = await _productRateRepository.ProductRateAddNew(productRateModel);
                if (model != null)
                {
                    int id = model.Item1;
                    if (id > 0)
                    {
                        return RedirectToAction(nameof(ProductRateAdd), new { isSuccess = 1, productName = model.Item2, productRate = model.Item3 });
                    }
                }
                return RedirectToAction(nameof(ProductRateAdd), new { isSuccess = 2 });
            }
            return View();
        }

        [Authorize]
        [HttpGet("EditProductRate/{id}/{productId}/{rate}")]
        public IActionResult ProductRateEdit(int productId, int rate, int isSuccess = 0, bool isDisabled = false)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.IsDisabled = true;
            return View("ProductRateAdd");
        }

        [HttpPost("EditProductRate/{id}/{productId}/{rate}")]
        public async Task<IActionResult> ProductRateEdit([FromRoute] int id, ProductRateModel productRateModel)
        {
            if (ModelState.IsValid)
            {
                int x = await _productRateRepository.ProductRateEditById(id, productRateModel);
                if (x == 0)
                {
                    return RedirectToAction(nameof(ProductRateEdit), new { isSuccess = 2, productRateId = productRateModel.Id });
                }
            }
            return RedirectToAction("ProductRate");
        }

        [Authorize]
        public async Task<IActionResult> ProductRateDelete(int id)
        {
            bool isDeleted = await _productRateRepository.ProductRateDeleteById(id);
            if (isDeleted)
            {
                return RedirectToAction("ProductRate");
            }
            return null;
        }
    }
}
