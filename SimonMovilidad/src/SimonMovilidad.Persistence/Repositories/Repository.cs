namespace SimonMovilidad.Persistence.Repositories
{
    internal abstract class Repository<TEntity>
    {
        protected readonly ApplicationDbContext _context;

        protected Repository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
