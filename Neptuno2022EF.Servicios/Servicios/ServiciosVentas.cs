﻿using Neptuno2022EF.Datos;
using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.DetalleVenta;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Servicios.Interfaces;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Neptuno2022EF.Servicios.Servicios
{
    public class ServiciosVentas : IServiciosVentas
    {
        private readonly IRepositorioVentas _repositorio;
        private readonly IRepositorioDetalleVentas _repoDetalleVentas;
        private readonly IRepositorioProductos _repoProductos;
        private readonly IUnitOfWork _unitOfWork;
        
        public ServiciosVentas(IRepositorioVentas repositorio,
            IRepositorioDetalleVentas repoDetalleVentas,
            IRepositorioProductos repoProductos,
            IUnitOfWork unitOfWork)
        {
            _repositorio = repositorio;
            _repoDetalleVentas = repoDetalleVentas;
            _repoProductos = repoProductos;
            _unitOfWork = unitOfWork;
        }

        public List<DetalleVentaListDto> GetDetalleVenta(int ventaId)
        {
            try
            {
                return _repoDetalleVentas.GetDetalleVentas(ventaId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<VentaResumen> GetResumenCliente(int clienteId)
        {
            try
            {
                return _repositorio.GetResumenCliente(clienteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VentaListDto GetVentaListDtoPorId(int ventaId)
        {
            try
            {
                return _repositorio.GetVentaDtoPorId(ventaId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Venta GetVentaPorId(int nroFactura)
        {
            try
            {
                return _repositorio.GetVentaPorId(nroFactura);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<VentaListDto> GetVentas()
        {
            try
            {
                return _repositorio.GetVentas();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Guardar(Venta venta)
        {
            try
            {
                using (var transaction=new TransactionScope())
                {
                    var ventaGuardar = new Venta()
                    {
                        ClienteId = venta.ClienteId,
                        FechaVenta = venta.FechaVenta,
                        Total = venta.Total
                    };
                    _repositorio.Agregar(ventaGuardar);
                    _unitOfWork.SaveChanges();
                    foreach (var item in venta.Detalles)
                    {
                        item.VentaId = ventaGuardar.VentaId;
                        _repoDetalleVentas.Agregar(item);
                        _repoProductos.ActualizarStock(item.ProductoId, item.Cantidad);
                    }
                    _unitOfWork.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarEstadoVenta(List<Venta> lista)
        {
            try
            {
                bool valido = false;
                using (var transaction=new TransactionScope())
                {
                    _repositorio.ModificarEstadoVenta(lista);
                    _unitOfWork.SaveChanges();

                    transaction.Complete();
                }
                foreach (var item in lista)
                {
                    _repositorio.GetVentasFromCliente(item.ClienteId);
                    if (item.Estado == Entidades.Enums.Estado.Impaga)
                    {
                        valido = false ;
                    }
                    valido = true;
                }
                return valido;
            }
            catch (Exception)
            {

                throw;
            }
        }

        Venta IServiciosVentas.GetVentaPorId(int nroFactura)
        {
            try
            {
                return _repositorio.GetVentaPorId(nroFactura);
            }
            catch (Exception)
            {

                throw;
            }
        }

        List<Venta> IServiciosVentas.GetVentasFromCliente(int clienteId)
        {
            try
            {
                return _repositorio.GetVentasFromCliente(clienteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
