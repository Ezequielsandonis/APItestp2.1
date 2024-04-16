create database APItestp;

use APItestp

-----------------------------------------------Persona

create table Persona(
PersonaId int identity(1,1) not null primary key,
Nombre varchar(50),
Dni int,
FechaNacimiento date ,
Correo varchar(100) unique,
Contrasenia varchar(max),
Token varchar(max),
FechaExpiracion datetime ,
Estado bit
)

drop table Persona

-- Obtener Persona pór Id
create procedure ObtenerPersonaId
@PersonaId int
as begin
select * from Persona where PersonaId=@PersonaId
end



--Registrar persona
create procedure RegistrarPersona 
@Nombre varchar(50),
@Dni int,
@FechaNacimiento date ,
@Correo varchar(100),
@Contrasenia varchar(max),
@Token varchar(max),
@FechaExpiracion datetime,
@Estado bit = 0

as begin
insert into Persona values(@Nombre,@Dni,@FechaNacimiento,@Correo,@Contrasenia,@Token,@FechaExpiracion,@Estado) 
end




--activar cuenta 
create procedure ActivarCuenta 
@Token varchar (max),
@Fecha datetime 
as begin 

declare @Correo varchar(100)
declare @FechaExpiracion datetime

set @Correo = (select Correo from Persona where Token=@Token)
set @FechaExpiracion= (select FechaExpiracion from Persona where Token=@Token)

if @FechaExpiracion < @Fecha 
begin 
update Persona set Estado=1 where Token=@Token
update Persona set Token = null where Correo=@Correo 
select 1 as Resultado 
end 
else 
begin 
select 0 as Resultado
end 
end


-- Actualizar token
create procedure ActualizarToken 
@Correo varchar(100),
@Fecha datetime,
@Token varchar(max)
as begin
update Persona set Token=@Token, FechaExpiracion=@Fecha where Correo=@Correo
end


---- Validar Persona
create procedure ValidarPersona 
@Correo varchar(100)
as begin
select * from Persona where Correo=@Correo
end




--Actualizar persona 
create procedure ActualizarPersona
@PersonaId int, 
@Nombre varchar(50),
@Dni int,
@FechaNacimiento date,
@Correo varchar(100),
@Estado bit
as begin
update Persona set Nombre=@Nombre,Dni=@Dni, FechaNacimiento=@FechaNacimiento, Estado=@Estado where PersonaId=@PersonaId
end

--eliminar usuario

create procedure EliminarUsuario
@PersonaId int
as begin
delete from Persona where  PersonaId=@PersonaId
end

--Listar usuarios


create procedure ListarUsuarios
as begin
select * from Persona
end

--


