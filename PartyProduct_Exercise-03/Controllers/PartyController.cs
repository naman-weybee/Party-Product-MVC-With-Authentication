using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyProduct_Exercise_03.Models;
using PartyProduct_Exercise_03.Repository;
using PartyProduct_Exercise_03.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Controllers
{
    public class PartyController : Controller
    {
        private readonly IPartyRepository _partyRepository = null;
        private readonly IUserService _userService;

        public PartyController(IPartyRepository partyRepository, IUserService userService)
        {
            _partyRepository = partyRepository;
            _userService = userService;
        }

        public async Task<IActionResult> Party(string SortOrder, bool delete = false)
        {
            var userId = _userService.GetUserId();
            var isLoggedIn = _userService.IsAuthenticated();

            ViewBag.delete = delete;

            ViewData["SortById"] = (SortOrder == "DescById" || SortOrder == "AscByParty" || SortOrder == "DescByParty") ? "AscById" : "DescById";
            ViewData["SortByParty"] = SortOrder == "AscByParty" ? "DescByParty" : "AscByParty";

            var data = await _partyRepository.GetAllParty();

            switch (SortOrder)
            {
                case "AscById":
                    ViewBag.AscId = "Asc";
                    return View(data);
                case "DescById":
                    ViewBag.AscId = "Desc";
                    return View(data.OrderByDescending(x => x.Id));
                case "AscByParty":
                    ViewBag.AscParty = "Asc";
                    return View(data.OrderBy(x => x.PartyName));
                case "DescByParty":
                    ViewBag.AscParty = "Desc";
                    return View(data.OrderByDescending(x => x.PartyName));
                default:
                    break;
            }
            return View(data);
        }

        [Authorize]
        public IActionResult PartyAdd(int isSuccess = 0, int partyId = 0, string partyName = null)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.partyId = partyId;
            ViewBag.partyName = partyName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PartyAdd(PartyModel partyModel)
        {
            if (ModelState.IsValid)
            {
                int id = await _partyRepository.PartyAdd(partyModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(PartyAdd), new { isSuccess = 1, partyName = partyModel.PartyName });
                }
                else
                {
                    return RedirectToAction(nameof(PartyAdd), new { isSuccess = 2 });
                }
            }
            return View();
        }

        [Authorize]
        [HttpGet("EditParty/{id}/{partyName}")]
        public IActionResult PartyEdit(string partyName, int isSuccess = 0, [FromRoute] int partyId = 0)
        {
            return View("PartyAdd");
        }

        [HttpPost("EditParty/{id}/{partyName}")]
        public async Task<IActionResult> PartyEdit([FromRoute] int id, PartyModel partyModel)
        {
            if (ModelState.IsValid)
            {
                int x = await _partyRepository.PartyEditById(id, partyModel);
                if (x == 0)
                {
                    return RedirectToAction(nameof(PartyAdd), new { isSuccess = 2, partyId = 1 });
                }
            }
            return RedirectToAction("Party");
        }

        [Authorize]
        public async Task<IActionResult> PartyDelete(int id)
        {
            bool isDeleted = await _partyRepository.PartyDeleteById(id);
            if (isDeleted)
            {
                return RedirectToAction("Party");
            }
            return RedirectToAction(nameof(Party), new { delete = true });
        }
    }
}
