using UNIVERSIDAD.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static UNIVERSIDAD.Models.libLogin;

namespace Super.Controllers
{
    [RoutePrefix("api/Login")]
    //[Authorize] DIRECTIVA PARA OBLIGAR A QUE SE TENGA AUTORIZACION DE INGRESO AL SERVICIO
    //[AllowAnonymous] DIRECTIVA PARA QUE SE PUEDA USAR EL SERVICIO SIN AUTORIZACION
    [AllowAnonymous]
    public class LoginController : ApiController
    {

        [HttpPost]
        [Route("Ingresar")]
        public IQueryable<LoginRespuesta> Ingresar(Login login)
        {
            clsLogin _Login = new clsLogin();
            _Login.login = login;

            return _Login.Ingresar();
        }

    }
}