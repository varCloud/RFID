 exec SP_AGREGA_ACTUALIZA_USUARIO
 @idUsuario = 7,
@nombreCompleto = 'Victor Adrian Reyes',
@correo ='var901106',
@telefono  ='4433740472',
@usuario ='vic7',
@contrasena = 'vic7'

exec SP_OBTENER_USUARIOS  7

select * from CatRol
select * from Usuarios

/************PRODUCTOS********************************/
delete from Inventario
DBCC CHECKIDENT ('Inventario', RESEED, 0)
delete from productos
DBCC CHECKIDENT ('productos', RESEED, 0)
exec SP_AGREGA_ACTUALIZA_PRODUCTO
@idProducto = 0,
@tag ='QWER123QWER-PEPINOS',
@descripcion ='PEPINOS'
select * from Productos
select * from Inventario
select * from InventarioDetalle

exec SP_ACTUALIZA_ACITVO_PRODUCTO
@idProducto =3,
@activo=1


exec SP_OBTENER_PRODUCTOS 4
/******AFECTA INVENTARIO********************/
exec SP_AFECTA_INVENTARIO
@tagId = 'PEPINO_1'
,@cantidad = 250
,@TipoInventario = 1
,@noPuerta = 1
select * from Productos
select * from Inventario
select * from InventarioDetalle


