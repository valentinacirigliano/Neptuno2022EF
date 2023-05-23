using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Datos;
using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptuno2022EF.Datos.Repositorios;
using NuevaAppComercial2022.Entidades.Entidades;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using System.Transactions;
using Microsoft.SqlServer.Server;
using System.Runtime.Remoting.Contexts;

namespace Neptuno2022EF.Servicios.Servicios
{
    public class ServiciosCtasCtes : IServicioCtasCtes
    {
        private readonly RepositorioCtasCtes _repositorio;
        private readonly IRepositorioVentas _repoVentas;
        private readonly IUnitOfWork _unitOfWork;
        public ServiciosCtasCtes(IRepositorioCtasCtes repositorio, IUnitOfWork unitOfWork,
            IRepositorioVentas repoVentas)
        {
            _repositorio = (RepositorioCtasCtes)repositorio;
            _unitOfWork = unitOfWork;
            _repoVentas = repoVentas;
        }

        public List<CtaCteListDto> Filtrar(Func<Cliente, bool> predicado)
        {
            throw new NotImplementedException();
        }


        public CtaCte GetCuentaPorCliente(string cliente)
        {
            try
            {
                return _repositorio.GetCuentaPorCliente(cliente);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CtaCteListDto> GetMovimientos(int ClienteId)
        {
            try
            {
                return _repositorio.GetMovimientos(ClienteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CtaCteListDto> GetMovimientos(string cliente)
        {
            try
            {
                return _repositorio.GetMovimientos(cliente);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public decimal GetSaldo(string cliente)
        {
            try
            {
                return _repositorio.GetSaldo(cliente);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CtaCteResumen> GetSaldos()
        {
            try
            {
                return _repositorio.GetSaldos();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Cliente GetClientePorNombre(string nombre)
        {
            try
            {
                return _repositorio.GetClientePorNombre(nombre);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void PagoTotalCuenta(Cliente cliente, FormaPago forma, decimal importeRecibido)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var listaVentas = _repoVentas.GetVentasFromCliente(cliente.Id);
                    _repoVentas.ModificarEstadoVenta(listaVentas);
                    _unitOfWork.SaveChanges();

                    foreach (var venta in listaVentas)
                    {
                        CtaCte ctaCte = new CtaCte
                        {
                                FechaMovimiento = DateTime.Now,
                                Movimiento = $"PAGO {forma} {venta.VentaId}",
                                Debe = 0,
                                Haber = importeRecibido,
                                Saldo = 0,
                                ClienteId = cliente.Id,

                        };
                        GuardarCtaCte(ctaCte);
                        _unitOfWork.SaveChanges();
                    }

                    transaction.Complete();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GuardarCtaCte(CtaCte ctaCte)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {


                    var ctaCteGuardar = new CtaCte()
                    {
                        FechaMovimiento = ctaCte.FechaMovimiento,
                        Movimiento = ctaCte.Movimiento,
                        Debe = ctaCte.Debe,
                        Haber = ctaCte.Haber,
                        Saldo = ctaCte.Saldo,
                        ClienteId = ctaCte.ClienteId,
                    };
                    _repositorio.Agregar(ctaCteGuardar);
                    _unitOfWork.SaveChanges();



                    transaction.Complete();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void PagoFactura(int nroFactura, Cliente cliente, FormaPago forma, decimal importeRecibido)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    _repoVentas.ModificarEstadoVenta(nroFactura);
                    _unitOfWork.SaveChanges();

                    CtaCte ctaCte = new CtaCte
                    {
                        FechaMovimiento = DateTime.Now,
                        Movimiento = $"PAGO {forma} {nroFactura}",
                        Debe = 0,
                        Haber = importeRecibido,
                        Saldo = GetSaldo(cliente.Nombre) - importeRecibido,
                        ClienteId = cliente.Id,

                    };
                    GuardarCtaCte(ctaCte);
                    _unitOfWork.SaveChanges();


                    transaction.Complete();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
