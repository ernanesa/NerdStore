using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identidade.API.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private ICollection<string> Erros = new List<string>();

    protected ActionResult CustomResponse(object result = null)
    {
        if (OperacaoValida())
        {
            return Ok(result);
        }

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            {"Messages", Erros.ToArray()}
        }));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return CustomResponse();
    }

    private bool OperacaoValida()
    {
        return Erros.Count == 0;
    }

    protected void AdicionarErroProcessamento(string erro)
    {
        Erros.Add(erro);
    }

    protected void LimparErrosProcessamento()
    {
        Erros.Clear();
    }
}