using System.Net.Mail;
using System.Net;

namespace APItestp2._1.Data
{
    public class Email
    {
        //METODO para aislar el metodo de envio de correo con el metodo que contiene la informacion  del correo
        //este metodo solo llama al metodo Correo 
        //este metodo se llama a mandar desde cualquier otra clase
        public void Enviar(string correo, string token)
        {
            Correo(correo, token);
        }


        //datos con la informacion  del correo
        //este metodo se encarga de llenar y mandar el correo
        void Correo(string correo_receptor, string token)
        {
            string correo_emisor = "ezequielsandonis@gmail.com";
            string clave_emisor = "czag wgmi tfje vnfg";

            MailAddress receptor = new(correo_receptor);
            MailAddress emisor = new(correo_emisor);

            MailMessage email = new(emisor, receptor);
            email.Subject = "Activación de cuentas";
            email.Body = @" <!DOCTYPE html>
                    <html> 
                      <head>
                        <title>Activacion de cuenta</title>
                      </head>
                      <body><h2>Para activar su cuenta, ingrese al siguiente enlace:</h2>
                      <a href='https://localhost:7214/Cuenta/Token?valor=" + token + "'>Activar Cuenta</a></body></html>";
            email.IsBodyHtml = true; //definir que el cuerpo tiene estructura html para darle un diseño

            SmtpClient smtp = new();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;  //465
            smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            //control de errores en el envio https://localhost:7022/Cuenta/Token?valor="+ token +;
            try
            {
                //enviar em correo
                smtp.Send(email);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
