using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Distribuidora.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tiendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTienda = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EncargadoTienda = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Barrio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Zona = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoVehiculo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KilometrajeActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstadoVehiculo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zonas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreZona = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaxEmpleados = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zonas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ZonaId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empleados_Zonas_ZonaId",
                        column: x => x.ZonaId,
                        principalTable: "Zonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosJornada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    KilometrosRecorridos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VentaTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    TiendaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosJornada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosJornada_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosJornada_Tiendas_TiendaId",
                        column: x => x.TiendaId,
                        principalTable: "Tiendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrosJornada_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadVendida = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RegistroJornadaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_RegistrosJornada_RegistroJornadaId",
                        column: x => x.RegistroJornadaId,
                        principalTable: "RegistrosJornada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_ProductoId",
                table: "DetallesVenta",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_RegistroJornadaId",
                table: "DetallesVenta",
                column: "RegistroJornadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_VehiculoId",
                table: "Empleados",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_ZonaId",
                table: "Empleados",
                column: "ZonaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosJornada_EmpleadoId",
                table: "RegistrosJornada",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosJornada_TiendaId",
                table: "RegistrosJornada",
                column: "TiendaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosJornada_VehiculoId",
                table: "RegistrosJornada",
                column: "VehiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesVenta");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "RegistrosJornada");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Tiendas");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Zonas");
        }
    }
}
