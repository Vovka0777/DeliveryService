using AutoMapper;
using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private readonly IMapper _mapper;

        public AdminController(IBaseStorage<UserDb> userStorage, IMapper mapper)
        {
            _userStorage = userStorage;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var usersDb = _userStorage.GetAll().ToList();
            var users = _mapper.Map<List<User>>(usersDb);
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userStorage.Get(id);
            if (user != null) await _userStorage.Delete(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(Guid id)
        {
            var userDb = await _userStorage.Get(id);
            if (userDb == null) return RedirectToAction("Index");

            var user = _mapper.Map<User>(userDb);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User model)
        {
            var userFromDb = await _userStorage.Get(model.Id);

            if (userFromDb != null)
            {
                userFromDb.Login = model.Login;
                userFromDb.Email = model.Email;

                userFromDb.Role = (int)model.Role;

                await _userStorage.Update(userFromDb);
            }

            return RedirectToAction("Index");
        }

        public IActionResult CatalogManager()
        {
            return RedirectToAction("Index", "Catalog");
        }
    }
}