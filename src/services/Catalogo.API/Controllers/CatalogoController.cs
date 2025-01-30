using Catalogo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController(IProdutoRepository produtoRepository) : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            return await _produtoRepository.ObterPorId(id);
        }
    }
}
