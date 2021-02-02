using Microsoft.EntityFrameworkCore;
using TarefasBackEnd.Models;

namespace TarefasBackEnd.Repositories
{
    public class DataContext : DbContext {

        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Tarefa> Tarefas { get; set; }
           
    }
}