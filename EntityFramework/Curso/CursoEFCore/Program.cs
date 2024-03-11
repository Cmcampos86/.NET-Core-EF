using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Outra forma de executar o Migration
            //Forma utilizada em desenvolvimento e não em produção
            //using var db = new CursoEFCore.Data.ApplicationContext();

            //db.Database.Migrate(); //Executa uma migration
            //var existe = db.Database.GetPendingMigrations().Any(); //Verifica migrações pendentes

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoverRegistro();
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            //Mesmo tendo 4 formas de adicionar um registro, o EF rastreia a instância e dessa forma somente um registro é inserido e não quatro

            ////Forma 1 de adicionar no modelo de dados
            //db.Produtos.Add(produto); //Indicado a usar

            ////Forma 2 de adicionar no modelo de dados
            //db.Set<Produto>().Add(produto); //Indicado a usar

            ////Forma 3 de adicionar no modelo de dados
            //db.Entry(produto).State = EntityState.Added; 

            //Forma 4 de adicionar no modelo de dados
            db.Add(produto);

            var registros = db.SaveChanges(); //Salva os registros no Banco
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Rafael Almeida",
                CEP = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "99000001111",
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Teste 1",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001115",
                },
                new Cliente
                {
                    Nome = "Teste 2",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001116",
                },
            };


            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente); // Passa as duas instancia inserindo um registro de cada
            db.Set<Cliente>().AddRange(listaClientes); //Passa a lista por conta do Range
            //db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                //db.Clientes.Find(cliente.Id); //Consulta os objetos que estão em memória
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                 {
                     new PedidoItem
                     {
                         ProdutoId = produto.Id,
                         Desconto = 0,
                         Quantidade = 1,
                         Valor = 10,
                     }
                 }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens) //Propriedade da lista de ItemPedido
                    .ThenInclude(p => p.Produto) //Propriedade Produto de Pedido
                .ToList();

            Console.WriteLine($"Consultando pedidos: {pedidos.Count}");
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(1);
            //cliente.Nome = "Cliente Alterado Passo 2";

            //db.Entry(cliente).State = EntityState.Modified; //Modo de rastreamento. Informa que é uma alteração

            var cliente = new Cliente
            {
                Id = 1
            };

            //Não é feita a consulta no banco de dados para busca o cliente (Forma desconectada)
            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado Passo 3",
                Telefone = "7966669999"
            };

            db.Attach(cliente); //Rastreamento (Desconectado)
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            //db.Clientes.Update(cliente); //Essa forma atualiza todos os campos mesmo que não sofreram alteração. Nâo utilizar para que somente a informação mudada seja alterada no banco de dados
            db.SaveChanges();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();

            //var cliente = db.Clientes.Find(2); //Utiliza para pesquisa a chave primária
            var cliente = new Cliente { Id = 3 }; //Forma desconectada

            //db.Clientes.Remove(cliente); //Forma 1 de delete
            //db.Remove(cliente);  //Forma 2 de delete

            db.Entry(cliente).State = EntityState.Deleted; //Forma 3 de delete

            db.SaveChanges();
        }
    }
}