using MemberApi.Models;
using MemberApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    public class ViewWithfController : Controller
    {
        private readonly AuthCodeService _service;

        public ViewWithfController(AuthCodeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? name, int page = 1, int size = 10)
        {
            var result = await _service.PagedList(name, page, size);
            return View(result);
        }
    }
}