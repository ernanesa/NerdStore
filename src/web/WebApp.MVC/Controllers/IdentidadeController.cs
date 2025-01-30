using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApp.MVC.Models;
using WebApp.MVC.Services;

namespace WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }


        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            if (usuarioRegistro.Senha != usuarioRegistro.SenhaConfirmacao)
            {
                ModelState.AddModelError(string.Empty, "As senhas não conferem.");
                return View(usuarioRegistro);
            }

            var response = await _autenticacaoService.Registro(usuarioRegistro);

            if (response != null)
            {
                await RealizarLogin(response);
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, "Erro ao registrar usuário.");
            return View(usuarioRegistro);

        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            var response = await _autenticacaoService.Login(usuarioLogin);

            if (response != null)
            {
                await RealizarLogin(response);
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos.");
            return View(usuarioLogin);

        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);
            var claims = new List<Claim> { new Claim("JWT", resposta.AccessToken) };

            claims.AddRange(token.Claims);
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(resposta.ExpiresIn),
                IsPersistent = true
            };
            
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
        }
        
        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}