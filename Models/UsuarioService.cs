using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Models
{
    public class UsuarioService
    {   
        //Função que retorna uma lista de todos os usuarios do banco de dados
        //Function that returns a list of all users from the database
        public List<Usuario> Listar()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Acessa o banco de dados, recebe todos os valores de 'Usuarios' e transforma em Lista
                //Accesses the database, receives all values of 'Users' and transforms into a list
                return bc.Usuarios.ToList();
            }
        }

        //Função que busca um usuario do banco de dados pelo id
        //Functi

        public Usuario Buscar(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Acessa o banco de dados, busca pelo id em 'Usuarios'
                //Accesses the database, searches for id in 'Users'
                return bc.Usuarios.Find(id);
            }
        }

        //Função que adiciona um usuario no banco de dados
        //Function that adds a user to the database
        public void adicionarUsuario(Usuario usuario)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //Adiciona o usuario no banco de dados
                //Adds the user to the database
                bc.Add(usuario); //Vai para a tabela Usuarios automaticamente pelo tipo do objeto //Adds the user to the database
                bc.SaveChanges();
            }
        }

        //Função que edita as informações de um usuario no banco de dados
        //Function that edits the information of a user in the database
           public void editarUsuario(Usuario usuario)
{
        using(BibliotecaContext bc = new BibliotecaContext())
        {
            // Busca o usuário antigo no banco de dados
            // Searches the old user in the database
            Usuario usuarioAntigo = bc.Usuarios.Find(usuario.Id);

            // Atualiza outras informações no banco de dados (Login, Nome, Tipo)
            // Updates other information in the database (Login, Name, Type)
            usuarioAntigo.Login = usuario.Login;
            usuarioAntigo.Nome = usuario.Nome;
            usuarioAntigo.Tipo = usuario.Tipo;

            // Se a senha foi alterada
            // If the password was changed
            if (!string.IsNullOrEmpty(usuario.Senha) && usuario.Senha != usuarioAntigo.Senha)
            {
                // Re-hash a nova senha usando o método atual
                // Re-hash a new password using the current method
                usuarioAntigo.Senha = Criptografo.Criptografar(usuario.Senha);
            }
            bc.SaveChanges();
        }
    }

        //Função que exclui um usuário no banco de dados
        //Function that deletes a user from the database
        public void ExcluirUsuario(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Remove(bc.Usuarios.Find(id));
                bc.SaveChanges();
            }
        }
    }
}