using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Biblioteca.Controllers
{
    
    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            //check login
            Autenticacao.verificaLogin(this);

            LivroService livroService = new LivroService();
            EmprestimoService emprestimoService = new EmprestimoService();
            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();

            //Lista apenas os livros disponiveis
            //List only available books
            cadModel.Livros = livroService.ListarDisponiveis();
            
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            // Cria uma instância do serviço de empréstimo para interagir com a camada de lógica de negócios
            // Creates an instance of the emprestimo service to interact with the business logic layer
            EmprestimoService emprestimoService = new EmprestimoService();
            // Verifica se o ID do empréstimo na visão é zero, indicando que é um novo empréstimo a ser inserido.
            // Verifies if the ID of the emprestimo in the view is zero, indicating that a new emprestimo should be inserted.
            if(viewModel.Emprestimo.Id == 0)
            {
            // Chama o método Inserir do serviço de empréstimo para adicionar o novo empréstimo ao sistema.
             //  Calls the Inser method of the emprestimo service to add the new emprestimo to the system.
                emprestimoService.Inserir(viewModel.Emprestimo);
            }
            else
            {
                 // Caso contrário, o ID não é zero, indicando que o empréstimo já existe e deve ser atualizado.
                 // Otherwise, the ID is not zero, indicating that the emprestimo already exists and should be updated.
                // Chama o método Atualizar do serviço de empréstimo para aplicar as alterações ao empréstimo existente.
                // Calls the Update method of the emprestimo service to apply the changes to the existing emprestimo.
                emprestimoService.Atualizar(viewModel.Emprestimo);
            }
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string TipoFiltro, string Filtro, string itensPorPagina,int paginaAtual)
        {
            Autenticacao.verificaLogin(this);

            //Filtro fica nulo por padrão
            //Filter is null by default
            FiltrosEmprestimos objFiltro = null;
            
            //Se o filtro não estiver nulo
            //If the filter is not null
            if(!string.IsNullOrEmpty(Filtro))
            {
                //Cria um objeto do tipo FiltrosEmprestimos
                //Creates an object of type FiltrosEmprestimos
                objFiltro = new FiltrosEmprestimos();
                //Define o filtro
                //Defines the filter
                objFiltro.Filtro = Filtro;
                //Define o tipo do filtro
                //Defines the type of filter
                objFiltro.TipoFiltro = TipoFiltro;
            }
                ViewData["itensPorPagina"] = (string.IsNullOrEmpty(itensPorPagina) ? 10 : Int32.Parse(itensPorPagina));
                ViewData["paginaAtual"] = (paginaAtual!=0 ? paginaAtual : 1);

            //Cria um objeto do tipo EmprestimoService
            EmprestimoService emprestimoService = new EmprestimoService();
             
            //Retorna listando apenas os emprestimos que se adequam no filtro
            return View(emprestimoService.ListarTodos(objFiltro));
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.verificaLogin(this);
            
            LivroService livroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            cadModel.Emprestimo = e;
            
            return View(cadModel);
        }
    }
}