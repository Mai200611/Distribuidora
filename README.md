# Distribuidora

Contexto:

En una empresa de distribución de productos se necesita organizar su base de datos
dependiendo de unos parámetros, esta empresa necesita organizar sus vendedores por su ID
única a los cuáles se les asignará una zona del área metropolitana, cada área podrá tener
como máximo tres trabajadores los cuáles tendrán acceso a un vehículo cada uno, los
trabajadores deben de registrar el nombre de la tienda, dirección, nombre del vendedor,
productos vendidos y kilómetros recorridos al final de cada jornada.

Empleado
- Nombre
- ID
- ID Vehículo
- Cédula
- Teléfono
- ID zona
- Estado
Vehículo
- ID
- Tipo de vehículo
- Placa de vehículo
- Kilometraje actual
Zona
- ID zona
- Nombre zona
- Máximo empleados (3)
Tiendas
- ID tienda
- Nombre vendedor
- Dirección
- Zona
Producto
- ID producto
- Nombre producto
- Stock
- Precio

- Marca
- Categoría
Registro
- ID registro
- Fecha
- ID empleado
- ID vehículo
- Km recorridos
- Venta total
- Hora inicio
- Hora fin
Usuario
- ID usuario
- Username
- Password
- Rol
