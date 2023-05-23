﻿using Neptuno2022EF.Entidades.Dtos.DetalleVenta;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using System.Collections.Generic;

namespace Neptuno2022EF.Servicios.Interfaces
{
    public interface IServiciosVentas
    {
        List<DetalleVentaListDto> GetDetalleVenta(int ventaId);
        List<VentaListDto> GetVentas();
        void Guardar(Venta venta);
        List<Venta> GetVentasFromCliente(int clienteId);
        List<VentaResumen> GetResumenCliente(int clienteId);
        bool ModificarEstadoVenta(List<Venta> lista);
        Venta GetVentaPorId(int nroFactura);
        VentaListDto GetVentaListDtoPorId(int ventaId);
    }
}
