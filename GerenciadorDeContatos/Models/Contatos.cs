using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeContatos.Models
{
    public class Contatos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string Email { get; set; }

        
        [Phone(ErrorMessage = "O telefone informado não é válido.")]
        [StringLength(maximumLength: 15, MinimumLength =15)]
        [Required (ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(400)]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [StringLength(100, ErrorMessage = "A categoria não pode exceder 100 caracteres.")]
        public string Categoria { get; set; }
        // Construtor padrão
        public Contatos() { }
        // Construtor com parâmetros
        public Contatos(int id, string nome, string email, string telefone, string endereco, DateTime dataNascimento, string categoria)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Endereco = endereco;
            DataNascimento = dataNascimento;
            Categoria = categoria;
        }

    
      
    }
}
