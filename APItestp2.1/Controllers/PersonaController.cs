using APItestp2._1.Data;
using APItestp2._1.Data.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Data;
using APItestp2._1.Models;

namespace APItestp2._1.Controllers
{
    public class PersonaController : Controller
    {
        // instancia de la conexion para interactuar con la bd

        private readonly Contexto _contexto;
        private readonly PersonaServicios _personaServicios; // instancia

     
        public PersonaController(Contexto con)
        {
            _contexto = con;
            _personaServicios = new PersonaServicios(con);
        }


        //METODO INDEX- DEVUELVE TODAS LAS PERSONAS

        public IActionResult Index(string buscar, int? pagina)
        {
            //control de errores

            try
            {
               
                var personas = _personaServicios.ListarPersonas();
               
                //lista actualizada y pqaginada
                return View(personas);
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View();
            }

        }


        //METODO CREATE
        public IActionResult Create()
        {
            return View();
        }

        //post
        [HttpPost]
        public IActionResult Create(Persona persona)
        {
            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("RegistrarPersona", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //parametros 
                        cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
                        cmd.Parameters.AddWithValue("@Dni", persona.Dni);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", persona.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Correo", persona.Correo);
                        //encriptar contraseña con Bcrypt
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(persona.Contrasenia);
                        cmd.Parameters.AddWithValue("@Contrasenia", hashedPassword);                       
                        //Generar token
                        var token = Guid.NewGuid();
                        cmd.Parameters.AddWithValue("@Token ", token);
                        //expiracion
                        DateTime fechaExpiracion = DateTime.UtcNow.AddMinutes(5);
                        cmd.Parameters.AddWithValue("@FechaExpiracion", fechaExpiracion);
                        cmd.Parameters.AddWithValue("@Estado", persona.Estado);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                //cargar los datos
                return View(persona);
            }
        }





        //METODO EDITAR 
        public IActionResult Edit(int id)
        {
            try
            {
                //instancia y llamada al metodo para obtenerpersona
                var persona = _personaServicios.ObtenerPersonaId(id);
                // si es nbulo retorna not found
                if (persona == null) return NotFound();
                return View(persona);
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View();
            }

        }




        //post
        [HttpPost]
        public IActionResult Edit(int id, Persona persona)
        {
            //validar que coincidan los id
            if (id != persona.PersonaId) return NotFound();

            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("ActualizarPersona", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //parametros 
                        cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
                        cmd.Parameters.AddWithValue("@Dni", persona.Dni);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", persona.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Correo", persona.Correo);
                        cmd.Parameters.AddWithValue("@Estado", persona.Estado);
                        con.Open();
                        cmd.ExecuteNonQuery();


                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View(persona);
            }
        }



        //METODO DELETE:Get // cargar los datos y preguntar al usuario siu esta seguro
        public IActionResult Delete(int id)
        {
            try
            {
                var persona = _personaServicios.ObtenerPersonaId(id);
                if (persona == null) return NotFound();
                //si no retornar los valores de usuario
                return View(persona);
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View();
            }

        }

        //post -- confirmar eliminacion de datos
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                //eliminar persona
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("EliminarUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //parametros
                        cmd.Parameters.AddWithValue("@PersonaId", id);
                        con.Open();//abrir conexion
                        cmd.ExecuteNonQuery(); // ejecutar el procedimiento
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                ViewBag.Error = e.Message;
                //lamar al metodo del servicio
                return View(_personaServicios.ObtenerPersonaId(id));
            }
        }


    }
}
