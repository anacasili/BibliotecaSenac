using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {   
            Autenticacao.verificaLogin(this);
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        //Função de quando envia o formulario de login
        //Function that sends the login form
        [HttpPost]
        public IActionResult Login(string login, string senha)
        {
            //Se o login e senha estiverem corretos
            //If the login and password are correct
            if(Autenticacao.verificaLoginSenha(login, senha, this))
            {
                //Salva as informações nos cookies
                //Saves the data in cookies
                HttpContext.Session.SetString("Login", login);
                
                //Redireciona para a página inicial
                //Redirects to the initial page
                return RedirectToAction("Index");
            }
            else
            {
                //Exibe a mensagem de erro
                //Displays the error message
                ViewData["Erro"] = "Login ou Senha inválidos";

                return View();               
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
