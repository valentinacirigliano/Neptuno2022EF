using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using System;
using System.Collections.Generic;

namespace Neptuno2022EF.Datos.Interfaces
{
    public interface IRepositorioVentas
    {
        List<VentaListDto> GetVentas();
        void Agregar(Venta venta);
        List<VentaListDto> Filtrar(Func<Venta, bool> predicado);
        List<Venta> GetVentasFromCliente(int clienteId);
        void ModificarEstadoVenta(List<Venta> lista);
        List<VentaResumen> GetResumenCliente(int clienteId);
        void ModificarEstadoVenta(int nroFactura);
        Venta GetVentaPorId(int nroFactura);
        VentaListDto GetVentaDtoPorId(int ventaId);
    }
}
