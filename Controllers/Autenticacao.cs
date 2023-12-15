using System.Collections.Generic;
using System.Linq;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        //Função que verifica o status de login (Se está logado)
        //Function verifies login status
        public static void verificaLogin(Controller controller)
        {   
            //Verifica nos cookies se login é nulo ou vazio
            //Verifies if login is null or empty
            if(string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
            {
                //Se for nulo ou vazio, manda o usuário para a tela de login
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        //Função que verifica o login e senha
        //Function verifies login and password
        public static bool verificaLoginSenha(string login, string senha, Controller controller)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                //Verifica se o usuario Admin existe
                //Verifies if admin exists
                verificaSeAdminExiste(bc);

                //Criptrografa a senha
                //Encrypts password
                senha = Criptografo.Criptografar(senha);

                //Busca no banco de dados o usuário com o Login e Senha informados
                //Search in database the user with the informed Login and Password
                IQueryable<Usuario> UsuarioEncontrado = bc.Usuarios.Where(u => u.Login==login && u.Senha==senha);
                //Transforma o resultado da busca em uma lista
                //Transforms the result of the search in a list
                List<Usuario> ListaUsuarioEncontrado = UsuarioEncontrado.ToList();

                //Se nenhum usuário for encontrado
                //If no user is found
                if (ListaUsuarioEncontrado.Count == 0)
                {
                    //Retorna Falso
                    //Returns False
                    return false;
                }
                //Se for encontrado
                //If found
                else
                {
                    //Salva as informações nos cookies
                    //Saves the data in cookies
                    controller.HttpContext.Session.SetString("Login", ListaUsuarioEncontrado[0].Login);
                    controller.HttpContext.Session.SetString("Nome", ListaUsuarioEncontrado[0].Nome);
                    controller.HttpContext.Session.SetInt32("Tipo", ListaUsuarioEncontrado[0].Tipo);

                    //Retorna verdadeiro
                    //Returns True
                    return true;    
                }
            }
        }

        //Função que verifica se o usuario Admin existe
        //Function verifies if admin exists
        public static void verificaSeAdminExiste(BibliotecaContext bc)
        {
            //Busca usuario com login admin
            //Search user with login admin
            IQueryable<Usuario> userEncontrado = bc.Usuarios.Where(u => u.Login == "admin");

            //Se o usuario nao existir
            //If user doesn't exist
            if (userEncontrado.ToList().Count == 0)
            {   
                //Cria um objeto do tipo usuário
                //Creates an object of type user
                Usuario admin = new Usuario();

                //Define Login, Senha, Tipo e Nome
                //Defines Login, Password, Type and Name
                admin.Login = "admin";
                admin.Senha = Criptografo.Criptografar("123"); //Criptografa a senha //Encrypts password
                admin.Tipo = Usuario.admin;
                admin.Nome = "Administrador";

                //Adiciona ao banco de dados
                //Adds to database
                bc.Usuarios.Add(admin);
                //Salva as alterações
                //Saves changes
                bc.SaveChanges();
            }
        }

        //Função que verifica se o usuário é admin
        //Function verifies if user is admin
        public static void verificaSeUsuarioEAdmin(Controller controller)
        {
            //Verifica se o tipo do usuário logado atualmente é diferente do tipo Admin 
            //Verifies if the type of the currently logged in user is different from the Admin type
            if (controller.HttpContext.Session.GetInt32("Tipo") != Usuario.admin)
            {
                //Se não for admin, redireciona para a pagina "Precisa ser admin"
                //If not admin, redirects to "Precisa ser admin"
                controller.Request.HttpContext.Response.Redirect("/Usuario/NeedAdmin");
            }
        }
    }
}