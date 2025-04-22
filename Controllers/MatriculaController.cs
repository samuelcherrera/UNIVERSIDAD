using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
        public Matricula ConsultarPorDocumentoYSemestre(String Documento, String Semestre)
        {
            clsMatricula Matricula = new clsMatricula();
            return Matricula.ConsultarPorDocumentoYSemestre(Documento, Semestre);
        }

        [HttpGet]
        [Route("ConsultarPorDocumento")]
        public IHttpActionResult ConsultarPorDocumento(string documento)
        {
            clsMatricula matricula = new clsMatricula();

            // Llamar al método ConsultarXDocumento
            var resultado = matricula.ConsultarPorDocumento(documento);

            if (resultado == null || !resultado.Any())
            {
                return NotFound(); // Retornar 404 si no se encuentran matrículas
            }

            return Ok(resultado); // Retornar las matrículas encontradas
        }

        [HttpPost]
        [Route("IngresarMatricula")]
        public IHttpActionResult IngresarMatricula([FromBody] Matricula matricula)
        {
            if (matricula == null || matricula.idEstudiante == 0)
            {
                return BadRequest("Error: Datos de matrícula incompletos o nulos. Se requiere idEstudiante.");
            }

            // Obtener el idEstudiante del estudiante autenticado desde el token
            var identity = (ClaimsIdentity)User.Identity;
            var idEstudianteClaim = identity.Claims.FirstOrDefault(c => c.Type == "idEstudiante");
            if (idEstudianteClaim == null)
            {
                return Unauthorized();
            }

            int idEstudianteAutenticado = int.Parse(idEstudianteClaim.Value);

            // Validar que el idEstudiante de la matrícula coincida con el del token
            if (matricula.idEstudiante != idEstudianteAutenticado)
            {
                return Unauthorized(); // El estudiante no tiene permiso para ingresar esta matrícula
            }

            clsMatricula Matricula = new clsMatricula();

            // Insertar la matrícula
            var resultado = Matricula.Insertar(matricula);

            return Ok(resultado);
        }


        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Matricula matriculaActualizada)
        {
            if (matriculaActualizada == null || matriculaActualizada.idMatricula == 0 || string.IsNullOrEmpty(matriculaActualizada.SemestreMatricula))
            {
                return "Error: Datos de matrícula incompletos o nulos. Se requiere Id y SemestreMatricula.";
            }

            // Obtener el idEstudiante del estudiante autenticado desde el token
            var identity = (ClaimsIdentity)User.Identity;
            var idEstudianteClaim = identity.Claims.FirstOrDefault(c => c.Type == "idEstudiante");
            if (idEstudianteClaim == null)
            {
                return "Error: No se pudo obtener el id del estudiante autenticado.";
            }

            int idEstudianteAutenticado = int.Parse(idEstudianteClaim.Value);

            clsMatricula Matricula = new clsMatricula();

            // Pasar los argumentos requeridos al método Actualizar
            return Matricula.Actualizar(matriculaActualizada, idEstudianteAutenticado);
        }


        [HttpDelete]
        [Route("EliminarMatricula")]
        public string Eliminar(string Documento, string Semestre)
        {
            clsMatricula Matricula = new clsMatricula();
            Matricula.matricula = Matricula.ConsultarPorDocumentoYSemestre(Documento, Semestre);

            if (Matricula.matricula == null)
            {
                return $"No existe matrícula para el documento {Documento} en el semestre {Semestre}.";
            }

            return Matricula.Eliminar();
        }
    }
}