namespace Restaurante.Models
{
    public class OrdenVM
    {
        public Orden Orden { get; set; }
        public List<Producto> Productos { get; set; }
        public List<OrdenDetalle> OrdenDetalles { get; set; }
        public int Id { get; set; }
    }
}
