using UNIVERSIDAD.Models;
using UNIVERSIDAD.Classes; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static UNIVERSIDAD.Models.libLogin;

namespace UNIVERSIDAD.Classes
{
    public class clsLogin
    {


        public clsLogin()
        {
            loginRespuesta = new LoginRespuesta();
        }
        public DBUniverdadEntities DBUniverdad = new DBUniverdadEntities();
        public Login login { get; set; }
        public LoginRespuesta loginRespuesta { get; set; }
        private bool ValidarUsuario()
        {
            try
            {

                //SE CONSULTA EL USUARIO SOLO CON EL NOMBRE, PARA OBTENER LA INFO BASICA DE USUARIO:SALT Y CLAVE ENCRIPTADA
                Estudiante estudiante = DBUniverdad.Estudiantes.FirstOrDefault(u => u.Usuario == login.Usuario);
                if (estudiante == null)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Usuario no existe";
                    return false;
                }

                

                //SE OBTIENE LA CLAVE CIFRADA
                login.Clave = estudiante.Clave;
                return true;
            }
            catch (Exception ex)
            {
                loginRespuesta.Autenticado = false;
                loginRespuesta.Mensaje = ex.Message;
                return false;
            }
        }
        private bool ValidarClave()
        {
            try
            {

                //SE CONSULTA EL USUARIO CON LA CLAVE ENCRIPTADA Y EL USUARIO PARA VALIDAR SI EXISTE 
                Estudiante estudiante = DBUniverdad.Estudiantes.FirstOrDefault(u => u.Usuario == login.Usuario && u.Clave == login.Clave);
                if (estudiante == null)
                {
                    //SI NO EXISTE LA CLAVE ES INCORRECTA

                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "La clave no coincide";
                    return false;
                }

                //LA CLAVE Y EL USUARIO SON CORRECTOS
                return true;
            }

            catch (Exception ex)
            {
                loginRespuesta.Autenticado = false;
                loginRespuesta.Mensaje = ex.Message;
                return false;
            }
        }
        public IQueryable<LoginRespuesta> Ingresar()
        {
            if (ValidarUsuario() && ValidarClave())
            {
                // Obtener el estudiante autenticado
                var estudiante = DBUniverdad.Estudiantes.FirstOrDefault(E => E.Usuario == login.Usuario && E.Clave == login.Clave);
                if (estudiante == null)
                {
                    loginRespuesta.Autenticado = false;
                    loginRespuesta.Mensaje = "Usuario no encontrado.";
                    return new List<LoginRespuesta> { loginRespuesta }.AsQueryable();
                }

                // Generar el token con el idEstudiante
                string token = TokenGenerator.GenerateTokenJwt(login.Usuario, estudiante.idEstudiante);

                return new List<LoginRespuesta>
        {
            new LoginRespuesta
            {
                Usuario = estudiante.Usuario,
                Autenticado = true,
                Perfil = estudiante.NombreCompleto,
                Documento = estudiante.Documento,
                Token = token,
                Mensaje = ""
            }
        }.AsQueryable();
            }
            else
            {
                return new List<LoginRespuesta> { loginRespuesta }.AsQueryable();
            }
        }



    }
}