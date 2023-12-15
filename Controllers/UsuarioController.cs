using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        //Página de listagem dos usuarios
        //User list
        public IActionResult ListaUsuarios()
        {   

            Autenticacao.verificaLogin(this);
            //Verifica se usuário é admin
            //Verifies if user is admin
            Autenticacao.verificaSeUsuarioEAdmin(this);

            //Retorna a view passando a lista como parametro
            //Returns the view passing the list as parameter
            return View(new UsuarioService().Listar());
        }

        //Página que insere um usuario
        //Insert user
        public IActionResult CadastrarUsuario()
        {
            Autenticacao.verificaLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        //Função de quando o formulario de cadastro de usuarios é enviado
        //When the user registration form is submitted
        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario novoUsuario)
        {
            //Criptografa a senha do usuario
            //Encrypts the user password
            novoUsuario.Senha = Criptografo.Criptografar(novoUsuario.Senha);
            
            //Adiciona o usuario no banco de dados
            //Adds the user to the database
            new UsuarioService().adicionarUsuario(novoUsuario);

            //Retorna a página de cadastro realizado
            //Returns the registration page
            return RedirectToAction("CadastroRealizado");
        }

        //Página de cadastro realizado
        //Registration
        public IActionResult CadastroRealizado()
        {
            return View();
        }

        //Página de edição de usuario
        public IActionResult EditarUsuario(int id)
        {
            Autenticacao.verificaLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            //Busca o usuario que vai ser editado pelo id
            //Search the user to be edited by id
            Usuario Usuario = new UsuarioService().Buscar(id);
            
            //Retorna a view com o usuario como parametro
            //Returns the view with the user as parameter
            return View(Usuario);
        }

        //Função de quando o formulario de edição de usuarios é enviado
        //When the user edit form is submitted
        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuarioEditado)
        {
            //Edita o usuario no banco de dados
            //Edits the user in the database
            new UsuarioService().editarUsuario(usuarioEditado);

            //Redireciona para a página de listagem de usuarios
            //Redirects to the user list page
            return RedirectToAction("ListaUsuarios");
        }

        //Página de exclusão de usuário
        //User deletion
        public IActionResult ExcluirUsuario(int id)
        {
            Autenticacao.verificaLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            //Retorna a view passando o usuario que vai ser excluido como parametro
            //Returns the view passing the user to be deleted as parameter
            return View(new UsuarioService().Buscar(id));
        }

        //Função de quando o formulario de exclusão de usuarios é enviado
        //When the user deletion form is submitted
        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int id)
        {
            //Se o usuario confirmou que quer excluir
            //If the user confirms that he wants to delete
            if(decisao == "EXCLUIR")
            {
                //Exibe a mensagem
                //Displays the message
                ViewData["Mensagem"] = "Exclusão do usuário " + new UsuarioService().Buscar(id).Nome + " realizada";

                //Exclui o usuario do banco de dados
                //Deletes the user from the database
                new UsuarioService().ExcluirUsuario(id);

                //Retorna para a View de Listagem de usuarios, passando a lista de usuarios atualizada como parametro
                //Returns to the user list view, passing the updated user list as parameter
                return View("ListaUsuarios", new UsuarioService().Listar());
            }
        
            else
            {
                ViewData["Mensagem"] = "Exclusão Cancelada";
                //Retorna para a View de Listagem de usuarios, passando a lista de usuarios atualizada como parametro
                //Returns to the user list view, passing the updated user list as parameter
                return View("ListaUsuarios", new UsuarioService().Listar());
            }
        }

        //Função que faz logoff
        //Logout
        public IActionResult Sair()
        {
            //Limpa as informações nos cookies
            //Clears the cookies
            HttpContext.Session.Clear();
            //Retorna para a página de login
            //Returns to the login page
            return RedirectToAction("Index", "Home");
        }

        //Página NeedAdmin
        public IActionResult NeedAdmin()
        {
            Autenticacao.verificaLogin(this);
            return View();
        }
    }
}