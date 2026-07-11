using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniTrainingCenterCatalog.Mvc.Models;
using MiniTrainingCenterCatalog.Mvc.Services;
using MiniTrainingCenterCatalog.Mvc.ViewModels;

namespace MiniTrainingCenterCatalog.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IAuditLogService _audit;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IAuditLogService audit)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _audit = audit;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result =
            await _signInManager.PasswordSignInAsync(
                vm.Email,
                vm.Password,
                vm.RememberMe,
                false);

        if (result.Succeeded)
        {
            await _audit.WriteAsync(
                vm.Email,
                "Login",
                "Account",
                "",
                "Success",
                HttpContext.TraceIdentifier);

            return RedirectToAction(
                "Index",
                "Dashboard");
        }

        await _audit.WriteAsync(
            vm.Email,
            "Login",
            "Account",
            "",
            "Failed",
            HttpContext.TraceIdentifier);

        ModelState.AddModelError(
            "",
            "Invalid email or password.");

        return View(vm);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = new ApplicationUser
        {
            UserName = vm.Email,
            Email = vm.Email,
            FullName = vm.FullName
        };

        var result =
            await _userManager.CreateAsync(
                user,
                vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }

            await _audit.WriteAsync(
                vm.Email,
                "Register",
                "Account",
                "Registration failed.",
                "",
                "Failed",
                HttpContext.TraceIdentifier);

            return View(vm);
        }

        await _userManager.AddToRoleAsync(
            user,
            "User");

        await _audit.WriteAsync(
            user.Email,
            "Register",
            "Account",
            "New user registered.",
            "",
            "Success",
            HttpContext.TraceIdentifier);

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _audit.WriteAsync(
            User.Identity!.Name!,
            "Logout",
            "Account",
            "",
            "Success",
            HttpContext.TraceIdentifier);

        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public async Task<IActionResult> AccessDenied()
    {
        await _audit.WriteAsync(
            User.Identity?.Name ?? "Anonymous",
            "AccessDenied",
            "Account",
            "",
            "Failed",
            HttpContext.TraceIdentifier,
            HttpContext.Request.Path);

        return View();
    }
}
