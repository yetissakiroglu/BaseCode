using Economy.Application.AdminUI.Dtos.DashboardSummaryDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.PanelUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IPanelDashboardService _svc;

        public DashboardController(IPanelDashboardService svc)
        {
            _svc = svc;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _svc.GetSummaryAsync();
            if (!res.IsSuccess)
            {
                AddMessage(res);
                return View(new DashboardSummaryDto());
            }
            return View(res.Data);
        }



        public IActionResult Editor()
        {
            return View();
        }
        public IActionResult Select()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
