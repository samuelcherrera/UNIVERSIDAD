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
        public string Actualizar([FromBody] Matricula matriculaActualizada)
        {
            if (matriculaActualizada == null || matriculaActualizada.idMatricula == 0 || string.IsNullOrEmpty(matriculaActualizada.SemestreMatricula))
            {
                return "Error: Datos de matrícula incompletos o nulos. Se requiere Id y SemestreMatricula.";
            }

            clsMatricula Matricula = new clsMatricula();
            // Ya no necesitas asignar a Matricula.matricula, pasamos el objeto directamente
            return Matricula.Actualizar(matriculaActualizada);
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