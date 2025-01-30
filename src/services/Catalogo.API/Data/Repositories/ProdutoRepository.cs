using Catalogo.API.Models;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.API.Data.Repositories
{
    public class ProdutoRepository(CatalogoContext context) : IProdutoRepository
    {
        private readonly CatalogoContext _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Remover(Produto produto)
        {
            _context.Produtos.Remove(produto);
        }
    }
}