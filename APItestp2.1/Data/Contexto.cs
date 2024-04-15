namespace APItestp2._1.Data
{
    //la clase contexto sera la encargada de contener mi conexión
    public class Contexto
    {
        public string Conexion {  get;}
        public Contexto(string valor)
        {
            Conexion = valor;     
        }
    }
}
