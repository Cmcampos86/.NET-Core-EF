using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEFCore.Domain
{
    [Table("Clientes")] //A tabela será gravada com esse nome
    public class Cliente
    {
        [Key] //Chave primária
        public int Id { get; set; }
        [Required] //Propriedade obrigatória
        public string Nome { get; set; }
        [Column("Phone")] //Vai gerar o campo como Phone
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
    }
}