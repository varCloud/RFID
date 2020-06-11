DELETE FROM [dbo].[InventarioDetalle]
DBCC CHECKIDENT ('InventarioDetalle', RESEED, 0)
DELETE FROM [dbo].[Inventario]
DBCC CHECKIDENT ('Inventario', RESEED, 0)
DELETE FROM  PRODUCTOS
DBCC CHECKIDENT ('PRODUCTOS', RESEED, 0)






/**************	INSERT DE CATALOGO TIPO INV**************************/
truncate table CatTipoInventario
DBCC CHECKIDENT ('CatTipoInventario', RESEED, 0)

insert into CatTipoInventario  (descripcion) values ('No registrado')
insert into CatTipoInventario  (descripcion) values ('Registrado')
insert into CatTipoInventario  (descripcion) values ('Entrada')
insert into CatTipoInventario  (descripcion) values ('Salida')
insert into CatTipoInventario  (descripcion) values ('Re-localizado')
select * from CatTipoInventario

/**************************************************************/
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
@tag ='666482-PEPINOS',
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
 @tagId = 'QWER123-PEPINOS'
,@cantidad = 125
,@TipoInventario = 1
,@noPuerta = 1

select * from Productos
select * from Inventario
select * from InventarioDetalle


