using Distribuidora.Shared.Entities;

public class RegistroJornada
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public decimal KilometrosRecorridos { get; set; }

    public decimal VentaTotal { get; set; }

    public string? Observaciones { get; set; }

    // FK EMPLEADO
    public int EmpleadoId { get; set; }
    public Empleado? Empleado { get; set; }

    // FK VEHICULO
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }

    // FK TIENDA
    public int TiendaId { get; set; }
    public Tienda? Tienda { get; set; }

    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
}