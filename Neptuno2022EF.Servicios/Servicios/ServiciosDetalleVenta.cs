using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.DetalleVenta;
using Neptuno2022EF.Servicios.Interfaces;
using System.Collections.Generic;

namespace Neptuno2022EF.Servicios.Servicios
{
    public class ServiciosDetalleVenta : IServiciosDetalleVenta
    {
        private readonly IRepositorioDetalleVentas _repositorio;
        public ServiciosDetalleVenta(IRepositorioDetalleVentas repositorio)
        {
            _repositorio = repositorio;
        }

        public List<DetalleVentaListDto> GetDetallesPorVenta(int id)
        {
            return _repositorio.GetDetalleVentas(id);
        }
    }
}
