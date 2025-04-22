using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UNIVERSIDAD.Models
{
    public class libLogin
    {
        public class Login
        {
            public string Usuario { get; set; }
            public string Clave { get; set; }
            public string PaginaSolicitud { get; set; }
        }
        public class LoginRespuesta
        {
            public string Usuario { get; set; }
            public string Perfil { get; set; }
            public string Documento { get; set; }
            public string PaginaInicio { get; set; }
            public bool Autenticado { get; set; }
            public string Token { get; set; }
            public string Mensaje { get; set; }
        }

    }
}