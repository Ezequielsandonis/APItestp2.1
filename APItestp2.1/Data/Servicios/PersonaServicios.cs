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

        //ACTUALIZAR TOKEN


        //LISTAR PERSONAS
        public List<Persona> ListarPersonas()
        {
            var personas = new List<Persona>();

            //interacción con la bd
            using (SqlConnection con = new SqlConnection(_contexto.Conexion))
            { //llamado al procedimiento
                using (SqlCommand cmd = new("ListarUsuarios"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //abrir la conexion sin parametros
                    con.Open();
                    //usar un reader para llenar la lista
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var persona = new Persona{
                            PersonaId = (int)rdr["PersonaId"],
                            Nombre = rdr["Nombre"].ToString(),
                            Dni = (int)rdr["Dni"],
                            FechaNacimiento = Convert.ToDateTime(rdr["FechaNacimiento"]),
                            Correo = rdr["Correo"].ToString(),
                            Contrasenia = rdr["Contrasenia"].ToString(),
                            Token = rdr["Token"].ToString(),
                            FechaExpiracion = Convert.ToDateTime(rdr["FechaExpiracion"]),
                            Estado = (Boolean)rdr["Estado"]
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
                using (SqlCommand cmd = new("ObtenerPersonaId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //parametro necesario
                    cmd.Parameters.AddWithValue("@PersonaId", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    //validar
                    if (rdr.Read())
                    {
                         persona = new Persona
                        {
                            PersonaId =id,
                            Nombre = rdr["Nombre"].ToString(),
                            Dni = (int)rdr["Dni"],
                            FechaNacimiento = Convert.ToDateTime(rdr["FechaNacimiento"]),
                            Correo = rdr["Correo"].ToString(),
                            Contrasenia = rdr["Contrasenia"].ToString(),
                            Token = rdr["Token"].ToString(),
                            FechaExpiracion = Convert.ToDateTime(rdr["FechaExpiracion"]),
                            Estado = (Boolean)rdr["Estado"]
                        };
                    }
                }
            }

            return persona;
        }



    }
}
