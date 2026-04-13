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

        public async Task<IActionResult> Index(string? name, int page = 1, int size = 10, CancellationToken ct = default)
        {
            var result = await _service.ListAsync(name, page, size, ct);
            return View(result);
        }
    }
}
