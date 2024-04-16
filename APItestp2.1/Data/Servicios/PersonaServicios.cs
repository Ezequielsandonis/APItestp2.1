using APItestp2._1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace APItestp2._1.Data.Servicios
{
    public class PersonaServicios
    {
        // instancia de la conexion para interactuar con la bd

        private readonly Contexto _contexto;

        //constructor que inicializa la clase con una conexión
        public PersonaServicios(Contexto con)
        {
                _contexto = con;
        }

     

        //metodo para actualizar token ya existente para activar cuenta
        public void ActualizarToken(string correo)
        {
            using (SqlConnection con = new(_contexto.Conexion))
            {
                using (SqlCommand cmd = new("ActualizarToken", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //parametros
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    //calcular  la fecha + 5minutos
                    DateTime fecha = DateTime.UtcNow.AddMinutes(5);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    //generar nuevo token
                    var token = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@Token", token.ToString());
                    con.Open(); // abrir conexion
                    cmd.ExecuteNonQuery(); // ejecutar
                    con.Close();//cierre de conexion

                    //INSERTAR ENVIO DE CORREO

                    Email email = new();
                    //validar
                    if (correo != null)

                        email.Enviar(correo, token.ToString());

                }
            }
        }







        //LISTAR PERSONAS
        public List<Persona> ListarPersonas()
        {
            var personas = new List<Persona>();

            //interacción con la bd
            using (SqlConnection con = new SqlConnection(_contexto.Conexion))
            { //llamado al procedimiento
                using (SqlCommand cmd = new("ListarUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //abrir la conexion sin parametros
                    con.Open();
                    //usar un reader para llenar la lista
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DateTime fechaHora = (DateTime)rdr["FechaNacimiento"];
                        var persona = new Persona
                        {
                            PersonaId = (int)rdr["PersonaId"],
                            Nombre = rdr["Nombre"].ToString(),
                            Dni = (int)rdr["Dni"],
                            FechaNacimiento = new DateOnly(fechaHora.Year, fechaHora.Month, fechaHora.Day),
                            Correo = rdr["Correo"].ToString(),
                            Contrasenia = rdr["Contrasenia"].ToString(),
                            Token = rdr["Token"].ToString(),
                            FechaExpiracion = Convert.ToDateTime(rdr["FechaExpiracion"]),
                            Estado = (bool)rdr["Estado"]
                        };

                        personas.Add(persona);

                    }
                }
            }

            return personas;
        }



        //BUSCAR PERSONA POR ID PARA CRUD

        public Persona ObtenerPersonaId(int id)
        {
            Persona persona = new (); //instancia inicializada
            using (SqlConnection con = new SqlConnection(_contexto.Conexion))
            {
                using (SqlCommand cmd = new("ObtenerPersonaId",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //parametro necesario
                    cmd.Parameters.AddWithValue("@PersonaId", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    //validar

                     persona = null; // Declarar la variable fuera del bloque if

                    if (rdr.Read())
                    {
                        DateTime fechaHora = (DateTime)rdr["FechaNacimiento"];
                        persona = new Persona
                        {
                            PersonaId = id,
                            Nombre = rdr["Nombre"].ToString(),
                            Dni = (int)rdr["Dni"],
                            FechaNacimiento = new DateOnly(fechaHora.Year, fechaHora.Month, fechaHora.Day),
                            Correo = rdr["Correo"].ToString(),
                            Contrasenia = rdr["Contrasenia"].ToString(),
                            Token = rdr["Token"].ToString(),
                            FechaExpiracion = Convert.ToDateTime(rdr["FechaExpiracion"]),
                            Estado = (bool)rdr["Estado"]
                        };
                    }
                }
            }

            return persona;
        }



    }
}
