using System.Collections.Generic;

namespace Biblioteca.Models
{
    public class CadEmprestimoViewModel
    {
        public ICollection<Livro> Livros { get; set; }
        public Emprestimo Emprestimo { get; set; }
        public LivroService LivroService { get; set; }
        public EmprestimoService emprestimoService { get; set; }
    }
}