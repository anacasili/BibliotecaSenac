using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Biblioteca.Models
{
    public class LivroService
    {
        //Função que insere o livro no banco de dados
        //Function that inserts the book in the database
        public void Inserir(Livro livro)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Adiciona o livro no banco de dados
                //Adds the book to the database
                bc.Livros.Add(livro);
                //Salva as alterações
                //Saves the changes
                bc.SaveChanges();
            }
        }

        //Função que atualiza informações de um livro
        //Function that updates information about a book
        public void Atualizar(Livro livro)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {   
                //Busca o livro pelo id
                //Search for the book by id
                Livro livroAntigo = bc.Livros.Find(livro.Id);
                //Atualiza o Autor
                //Updates the author
                livroAntigo.Autor = livro.Autor;
                //Atualiza o titulo
                //Updates the title
                livroAntigo.Titulo = livro.Titulo;
                //Atualiza o ano
                //Updates the year
                livroAntigo.Ano = livro.Ano;
                
                bc.SaveChanges();
            }
        }

        //Função que lista todos os livros (filtro padrão é nulo, mas pode ser alterado)
        //Function that lists all books (default filter is null, but can be changed)
        public ICollection<Livro> ListarTodos(FiltrosLivros Filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Livro> query;
                
                if(Filtro != null)
                {
                    //Define dinamicamente a filtragem
                    //Defines dynamic filtering
                    switch(Filtro.TipoFiltro)
                    {
                        //Se for por autor 
                        //If it's by author
                        case "Autor":
                            //Recebe os livros que se encaixam no filtro de autor
                            //Receives the books that match the filter of author
                            query = bc.Livros.Where(l => l.Autor.Contains(Filtro.Filtro));
                        break;

                        //Se for titulo
                        //If it's by title
                        case "Titulo":
                            //Recebe os livros que se encaixam no filtro de titulo
                            //Receives the books that match the filter of title
                            query = bc.Livros.Where(l => l.Titulo.Contains(Filtro.Filtro));
                        break;

                        //Padrão lista todos sem filtro
                        //Default list all without filter
                        default:
                            query = bc.Livros;
                        break;
                    }
                }
                //Caso filtro não tenha sido informado
                //If the filter has not been informed
                else
                {
                    //Lista todos sem filtro
                    //List all without filter
                    query = bc.Livros;
                }
                
                //Ordenação padrão (Ordem alfabetica por titulo)
                //Default sorting (alphabetical order by title)
                return query.OrderBy(l => l.Titulo).ToList();
            }
        }
        //Função que lista os livros disponiveis (que não estão sob emprestimo)
        //Function that lists the available books (that are not on loan)
        public ICollection<Livro> ListarDisponiveis()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Busca os livros onde o id não está entre os ids de livros em empréstimo
                //Search for the books where the id is not in the list of books on loan
                //utiliza uma subconsulta
                //uses a subquery
                return
                    bc.Livros
                    .Where(l =>  !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)))
                    .ToList();
            }
        }

        //Função que obtém um livro pelo id
        //Function that gets a book by id
        public Livro ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Retorna o livro que foi encontrado
                //Returns the book that was found
                return bc.Livros.Find(id);
            }
        }
    }
}