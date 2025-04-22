using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using UNIVERSIDAD.Models;

namespace UNIVERSIDAD.Classes
{
    public class clsMatricula
    {
        private DBUniverdadEntities DBUniversidad = new DBUniverdadEntities();

        public Matricula matricula { get; set; } //PARA MANIPULAR LA TABLA MATRICULA


        public String Insertar()
        {
            try
            {
                // Calcula el TotalMatricula antes de agregar la matrícula a la base de datos
                if (matricula != null)
                {
                    matricula.TotalMatricula = matricula.NumeroCreditos * matricula.ValorCredito;
                }

                DBUniversidad.Matriculas.Add(matricula); // agg una nueva matricula a la tabla MATRICULA (INSERT)
                DBUniversidad.SaveChanges(); // guarda cambios en la bd
                return "matricula ingresada correctamente " + (matricula.Estudiante != null ? matricula.Estudiante.Documento : "");
            }
            catch (Exception ex)
            {
                return "error al insertar la matricula " + ex.Message;
            }
        }


        /* 
          public Matricula ConsultarXDocumento(String Documento)
          {
              //EXPRESIONES LAMBDA:funciones anonimas que permiten filtrar los datos de una tabla
              //FirstOrDefault: devuelve el primer elemento que cumpla con la condicion de la expresion lambda
              Matricula mat = DBUniversidad.Matriculas.FirstOrDefault(e => e.Estudiante.Documento == Documento);//consulta la matricula por semestre de la matricula
              return mat;
          }

          public Matricula ConsultarXSemestre(String Semestre)
          {
              //EXPRESIONES LAMBDA:funciones anonimas que permiten filtrar los datos de una tabla
              //FirstOrDefault: devuelve el primer elemento que cumpla con la condicion de la expresion lambda
              Matricula mat = DBUniversidad.Matriculas.FirstOrDefault(e => e.SemestreMatricula == Semestre);//consulta la matricula por documento del estudiante
              return mat;
          }
        */
        public Matricula ConsultarPorDocumentoYSemestre(string documento, string semestre)
        {
            // Utilizamos Where para filtrar las matrículas que cumplen AMBAS condiciones
            Matricula mat = DBUniversidad.Matriculas
                .Where(e => e.Estudiante.Documento == documento && e.SemestreMatricula == semestre)
                .FirstOrDefault(); // Devolvemos la primera matrícula que coincida o null si no hay ninguna

            return mat;
        }

        public List<Matricula> ConsultarPorDocumento(string documento)
{
    // Utilizamos Where para filtrar las matrículas que cumplen AMBAS condiciones
    var matriculas = DBUniversidad.Matriculas
        .Where(e => e.Estudiante.Documento == documento)
        .ToList(); // Devolvemos la lista de matriculas asociadas al documento

    return matriculas;
}

        public String Actualizar(Matricula matriculaActualizada)
        {
            try
            {
                if (matriculaActualizada == null || matriculaActualizada.idMatricula==0)
                {
                    return "Error: Se debe proporcionar un Id de matrícula válido para actualizar.";
                }

                Matricula existingMatricula = DBUniversidad.Matriculas.Find(matriculaActualizada.idMatricula);

                if (existingMatricula == null)
                {
                    return "No existe una matrícula con el Id: " + matriculaActualizada.idMatricula;
                }

                // Actualiza las propiedades de la matrícula existente con los valores del objeto 'matriculaActualizada'
                existingMatricula.NumeroCreditos = matriculaActualizada.NumeroCreditos;
                existingMatricula.ValorCredito = matriculaActualizada.ValorCredito;
                existingMatricula.FechaMatricula = matriculaActualizada.FechaMatricula;
                existingMatricula.SemestreMatricula = matriculaActualizada.SemestreMatricula;
                existingMatricula.MateriasMatriculadas = matriculaActualizada.MateriasMatriculadas;
                // Actualiza otras propiedades aquí si es necesario

                // Recalcula el TotalMatricula
                existingMatricula.TotalMatricula = existingMatricula.NumeroCreditos * existingMatricula.ValorCredito;

                DBUniversidad.SaveChanges();
                return "Se ha actualizado la matrícula con Id: " + existingMatricula.idMatricula + " correctamente.";
            }
            catch (Exception ex)
            {
                return "Error al actualizar la matrícula: " + ex.Message;
            }
        }




        public String Eliminar()
        {
            try
            {
                //consultamos el empleado
                Matricula mat = ConsultarPorDocumentoYSemestre(matricula.Estudiante.Documento, matricula.SemestreMatricula);
                if (mat == null)
                {
                    return "no existe matricula para el documento " + mat.Estudiante.Documento + " en el semestre: " + mat.SemestreMatricula;
                }
                DBUniversidad.Matriculas.Remove(matricula);//actualiza el empleado de la tabla empleadoes
                DBUniversidad.SaveChanges();
                return "se ha eliminado la matricula correctamente";

            }
            catch (Exception ex)
            {
                return ex.Message;// MENSAJE DE ERROR
            }
        }
    }
}
