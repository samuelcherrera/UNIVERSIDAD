using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UNIVERSIDAD.Classes;
using UNIVERSIDAD.Models;   

namespace UNIVERSIDAD.Controllers
{
    [RoutePrefix("api/Matriculas")]
    [Authorize]
    public class MatriculaController : ApiController
    {

        [HttpGet]
        [Route("ConsultarPorDocumentoYSemestre")]
        public Matricula ConsultarXDocumento(String Documento, String Semestre)
        {
            clsMatricula Matricula = new clsMatricula();
            return Matricula.ConsultarPorDocumentoYSemestre(Documento, Semestre);
        }

        [HttpPost]
        [Route("IngresarMatricula")]
        public string Insertar([FromBody] Matricula matricula)
        {
            clsMatricula Matricula = new clsMatricula();

            //SE LE ASIGNA EL OBJETO empleado AL OBJETO empleado DE LA CLASE clsEmpleado 
            Matricula.matricula = matricula;

            return Matricula.Insertar();
        }

        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Matricula matricula)
        {
            clsMatricula Matricula = new clsMatricula();
            Matricula.matricula = matricula;

            return Matricula.Actualizar();
        }


        [HttpDelete]
        [Route("EliminarMatricula")]
        public string Eliminar(string Documento, string Semestre)
        {
            clsMatricula Matricula = new clsMatricula();
            return Matricula.Eliminar();
        }
    }
}