using DeliveryService.Domain.ViewModels.LoginAndRegistration;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryService.Service.Interfaces;
using DeliveryService.Domain.Enum;


namespace DeliveryService_Belko.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IAccountService accountService)
        {
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
        }

        public IActionResult SiteInformation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var response = await _accountService.Login(user);
                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {
                    return Ok(model);
                }

                ModelState.AddModelError("", response.Description);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(errors);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var response = await _accountService    .Register(user);
                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {
                    return Ok(model);
                }
                ModelState.AddModelError("", response.Description);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(errors);
        }
    }
}
