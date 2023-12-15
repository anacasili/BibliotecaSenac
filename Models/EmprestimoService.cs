using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;
                emprestimo.Devolvido = e.Devolvido;

                bc.SaveChanges();
            }
        }

        public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos Filtro)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Objeto do tipo query (banco de dados)
                //Object type query (database)
                IQueryable<Emprestimo> query;
                
                    if(Filtro != null)
                {
                     switch (Filtro.TipoFiltro)
            {
                case "Usuario":
                    query = bc.Emprestimos.Include(e => e.Livro).Where(e => e.NomeUsuario.Contains(Filtro.Filtro));
                    break;
                case "Livro":
                    query = bc.Emprestimos.Include(e => e.Livro).Where(e => e.Livro.Titulo.Contains(Filtro.Filtro));
                    break;
                default:
                    query = bc.Emprestimos.Include(e => e.Livro);
                    break;
            }
        }
        else
        {
            query = bc.Emprestimos.Include(e => e.Livro);
        }
              EmprestimoService emprestimoService = new EmprestimoService();

       return query.OrderByDescending(e => e.DataDevolucao).ToList();
    }
        }          
        

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }
    }
}