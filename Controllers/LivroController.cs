using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        //Cadastro de livro
        //Book Registration
        public IActionResult Cadastro()
        {
            Autenticacao.verificaLogin(this);

            return View();
        }

        //Função chamada ao enviar o formulario de cadastro de livros
        //Function called when submitting the book registration form
        [HttpPost]
        public IActionResult Cadastro(Livro l)
        {
            //Cria um objeto do tipo LivroService
            //Create an object of the LivroService
            LivroService livroService = new LivroService();

            //Se o id do livro for 0
            //If the book id is 0
            if(l.Id == 0)
            {
                //Insere o livro no banco de dados
                //Insert the book in the database
                livroService.Inserir(l);
            }
            else
            {
                //Atualiza as informações no banco de dados
                //Update the book in the database
                livroService.Atualizar(l);
            }

            //Redireciona para a listagem de livros
            //Redirect to the book list
            return RedirectToAction("Listagem");
        }

        //Listagem de livros
        //List of books
        public IActionResult Listagem(string TipoFiltro, string Filtro, string itensPorPagina, int paginaAtual)
    
        {
            //Verifica se o usuário está logado (se não estiver redireciona para login)
            //Verify if the user is logged in (if not, redirect to login)
            Autenticacao.verificaLogin(this);
            
            //Filtro fica nulo por padrão
            //Filter is set to null by default
            FiltrosLivros objFiltro = null;

            //Se o filtro não estiver nulo
            if(!string.IsNullOrEmpty(Filtro))
            {

                objFiltro = new FiltrosLivros();
                objFiltro.Filtro = Filtro;
                objFiltro.TipoFiltro = TipoFiltro;
            }
                //Define os valores que serão enviados para a View
                //Defines the values sent to the View
                ViewData["itensPorPagina"] = (string.IsNullOrEmpty(itensPorPagina) ? 10 : Int32.Parse(itensPorPagina));
                ViewData["paginaAtual"] = (paginaAtual!=0 ? paginaAtual : 1);

            //Cria um objeto do tipo LivroService
            //Create an object of the LivroService
            LivroService livroService = new LivroService();
            
            //Retorna listando apenas os livros que se adequam no filtro
            //Returns listing only the books that match the filter
            return View(livroService.ListarTodos(objFiltro));
        }

        //Edição de livro
        //Book editing
        public IActionResult Edicao(int id)
        {
            Autenticacao.verificaLogin(this);

            //Cria um objeto do tipo LivroService
            //Create an object of the LivroService
            LivroService livroService = new LivroService();
            //Busca o livro que vai ser editado pelo Id
            //Search the book that will be edited by Id
            Livro livro = livroService.ObterPorId(id);
            //Retorna a propria View, passando o livro que vai ser editado como parâmetro
            //Returns the same View, passing the book that will be edited as a parameter
            return View(livro);
        }
    }
}