using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p=>p.AddConsole()); //Inst�ncia do log utilizado

        public DbSet<Pedido> Pedidos { get; set; } //Tabelas criadas no contexto
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Configura a string de conex�o
        {
            optionsBuilder
                .UseLoggerFactory(_logger) //Qual o log que ser� utilizado
                .EnableSensitiveDataLogging() //Ver o valor de cada par�metro que esta sendo passado no console
                .UseSqlServer("Data source=DESKTOP-3VQHJTB;Initial Catalog=CursoEFCore;Integrated Security=true;TrustServerCertificate=True", //Precisei incluir o TrustServerCertificate=True
                 p => p.EnableRetryOnFailure( //Em caso de falha na conex�o, ele vai fazer alguma coisa como por exemplo conectar daqui tanto segundos tantas vezes
                     maxRetryCount: 2, //Quantidade de tentativas
                     maxRetryDelay: TimeSpan.FromSeconds(5), //Aguarda tantos segundos na tentativas
                     errorNumbersToAdd: null) //C�digo dos erros adicionais. Passa o padr�o dos c�digos passando null no par�metros
                 .MigrationsHistoryTable("curso_ef_core")); //Muda o nome da tabela de migra��es
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Forma 1 - Fazer para cada classe Configuration
            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            //Forma 2 - Busca todas as classe que tem implementado IEntityTypeConfiguration dentro de um Assembly (ApplicationContext � a classe da aplica��o)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder); //Configura as propriedades que n�o foi definida o modelo de dados
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes()) //Carrega a lista de entidades da aplica��o
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string)); //Carrega as propriedades da entidade

                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()) //Tipo da coluna vazio
                        && !property.GetMaxLength().HasValue) //N�o foi informado nada para essa propriedade
                    {
                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}