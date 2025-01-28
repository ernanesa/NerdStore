using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identidade.API.Extensions;
using Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identidade.API.Controllers;

[Route("api/identidade")]
public class AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings) : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly AppSettings _appSettings = appSettings.Value;

    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        var usuario = new IdentityUser { UserName = usuarioRegistro.Email, Email = usuarioRegistro.Email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(usuario, usuarioRegistro.Senha);

        foreach (var error in result.Errors)
            AdicionarErroProcessamento(error.Description);

        return !result.Succeeded ? CustomResponse() : CustomResponse(await GerarJWT(usuarioRegistro.Email));
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);
        return !result.Succeeded ? CustomResponse("Usuário ou senha inválidos") : CustomResponse(await GerarJWT(usuarioLogin.Email));
    }

    private async Task<UsuarioRespostaLogin> GerarJWT(string email)
    {
        var usuario = await ObterUsuarioPorEmail(email);
        var claims = await ObterClaimsDoUsuario(usuario);
        var token = GerarToken(claims);

        return CriarRespostaLogin(usuario, claims, token);
    }

    private async Task<IdentityUser> ObterUsuarioPorEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email)
               ?? throw new InvalidOperationException("Usuário não encontrado.");
    }

    private async Task<List<Claim>> ObterClaimsDoUsuario(IdentityUser usuario)
    {
        var claims = new List<Claim>(await _userManager.GetClaimsAsync(usuario));
        var roles = await _userManager.GetRolesAsync(usuario);

        claims.AddRange(
        [
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
    ]);

        claims.AddRange(roles.Select(role => new Claim("role", role)));

        return claims;
    }

    private string GerarToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private UsuarioRespostaLogin CriarRespostaLogin(IdentityUser usuario, IEnumerable<Claim> claims, string encodedToken)
    {
        return new UsuarioRespostaLogin
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }
    private static long ToUnixEpochDate(DateTime date) =>
        (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
