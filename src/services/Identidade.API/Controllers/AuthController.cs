using Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identidade.API.Controllers;

[ApiController]
[Route("api/identidade")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    
    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return BadRequest();
        var usuario = new IdentityUser { UserName = usuarioRegistro.Email, Email = usuarioRegistro.Email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(usuario, usuarioRegistro.Senha);
        if (!result.Succeeded) return BadRequest(result.Errors);
        await _signInManager.SignInAsync(usuario, false);
        return Ok();
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid) return BadRequest();
        var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, false);
        if (!result.Succeeded) return BadRequest("Usuário ou senha inválidos");
        return Ok();
    }
    
}