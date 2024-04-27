
using GestorTask.Infraestructure;

namespace GestorTask.Application.Implementation;
public class Repository<TEntity> : BaseRepository<TEntity, ModelContext> where TEntity : class, new()
{
    public Repository(ModelContext context) : base(context) { }
}
