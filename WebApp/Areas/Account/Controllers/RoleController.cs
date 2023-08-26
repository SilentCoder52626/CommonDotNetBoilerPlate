﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DomainModule.Dto;
using DomainModule.Entity;
using DomainModule.Exceptions;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NToastNotify;
using WebApp.ViewModel;

namespace WebApp.Areas.Account.Controllers
{
    [Area("Account")]
    
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleServiceInterface _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IToastNotification _notify;
        public RoleController(RoleManager<IdentityRole> roleManager,
          RoleServiceInterface roleService,
          ILogger<RoleController> logger,
          IToastNotification notify,
          UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _roleService = roleService;
            _logger = logger;
            _notify = notify;
            _userManager = userManager;
        }
        [Authorize(Policy ="Role-View")]
        public async Task<IActionResult> Index()
        {
            var roles =await  _roleManager.Roles.Where(a=>a.Name != DomainModule.Entity.User.TypeSuperAdmin).ToListAsync();
            var roleIndexViewModels = new List<RoleIndexViewModel>();
            var i = 1;
            foreach (var role in roles)
            {
                roleIndexViewModels.Add(new RoleIndexViewModel
                {
                    Sno = i,
                    Id = role.Id,
                    Name = role.Name,

                });
                i++;
              
            }
            return View(roleIndexViewModels);
        }

        [Authorize(Policy = "Role-Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            try
            {
                var dto = new RoleDto { RoleName = model.RoleName };
                await _roleService.Create(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return View(model);
        }
        [Authorize(Policy = "Role-Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(true) ?? throw new RoleNotFoundException();
            var roleEditViewModel = new RoleEditViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            try
            {
                await _roleService.Edit(new RoleEditDto { Id=model.Id,Name=model.Name});
                _notify.AddSuccessToastMessage("Role Updated Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

      
    }
}