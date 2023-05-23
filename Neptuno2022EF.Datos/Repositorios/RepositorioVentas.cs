using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;

namespace Neptuno2022EF.Datos.Repositorios
{
    public class RepositorioVentas : IRepositorioVentas
    {
        private readonly NeptunoDbContext _context;

        public RepositorioVentas(NeptunoDbContext context)
        {
            _context = context;
        }

        public void Agregar(Venta venta)
        {
            _context.Ventas.Add(venta);
        }

        public List<VentaListDto> Filtrar(Func<Venta, bool> predicado)
        {
            throw new NotImplementedException();
        }

        public List<VentaResumen> GetResumenCliente(int clienteId)
        {
            return _context.Ventas
                .Include(v => v.Cliente)
                .Where(v => v.ClienteId == clienteId)
                .Select(v=>new VentaResumen
                {
                    VentaId=v.VentaId,
                    Fecha=v.FechaVenta,
                    Estado=v.Estado,
                    Total=v.Total,
                })
                .ToList();
        }

        public Venta GetVentaPorId(int id)
        {
            return _context.Ventas
                .Include(v => v.Cliente)
                .SingleOrDefault(v => v.VentaId == id);
        }
        public List<Venta> GetVentasEnDetalle()
        {
            return _context.Ventas
                .Include(v => v.Cliente)
                .OrderBy(v => v.FechaVenta)
                .ToList();
        }
        public List<VentaListDto> GetVentas()
        {
            return _context.Ventas
                .Include(v=>v.Cliente)
                .OrderBy(v=>v.FechaVenta)
                .Select(v=>new VentaListDto
                {
                    VentaId=v.VentaId,
                    FechaVenta=v.FechaVenta,
                    Cliente=v.Cliente.Nombre,
                    Total=v.Total,
                    Estado=v.Estado.ToString()
                }).ToList();
        }

        public List<Venta> GetVentasFromCliente(int clienteId)
        {
            return _context.Ventas
                .Include(v => v.Cliente)
                .Where(v => v.ClienteId == clienteId)
                .ToList();
        }

        public void ModificarEstadoVenta(List<Venta> lista)
        {
            try
            {
                
                using (var transaction = new TransactionScope())
                {
                    foreach (Venta venta in lista)
                    {
                        if (venta.Estado == Estado.Impaga)
                        {
                            venta.Estado = Estado.Paga;
                            _context.Entry(venta).State = EntityState.Modified;
                        }
                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ex.Entries.ToList()
                             .ForEach(entry =>
                             {
                                 entry.Reload();

                             });

                        throw new Exception(ex.Message);


                    }

                    transaction.Complete();
                }
                foreach (var item in lista)
                {
                    bool valido = false;
                    GetVentasFromCliente(item.ClienteId);
                    if (item.Estado == Entidades.Enums.Estado.Impaga)
                    {
                        valido = false;
                    }
                    valido = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void ModificarEstadoVenta(int nroFactura)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    Venta venta = (Venta)GetVentasEnDetalle().SingleOrDefault(v => v.VentaId == nroFactura);
                    if (venta.Estado == Estado.Impaga)
                    {
                        venta.Estado = Estado.Paga;
                        _context.Entry(venta).State = EntityState.Modified;
                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ex.Entries.ToList()
                             .ForEach(entry =>
                             {
                                 entry.Reload();

                             });

                        throw new Exception(ex.Message);


                    }

                    transaction.Complete();
                }
                var ventaAhora = (Venta)GetVentasEnDetalle().SingleOrDefault(v => v.VentaId == nroFactura);

                //para revision mia
                bool valido = true;
                if (ventaAhora.Estado == Estado.Impaga)
                {
                    valido = false;
                }
            
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VentaListDto GetVentaDtoPorId(int ventaId)
        {
            try
            {
                return _context.Ventas
                    .Include(v => v.Cliente)
                    .Select(v => new VentaListDto
                    {
                        VentaId = v.VentaId,
                        FechaVenta = v.FechaVenta,
                        Cliente = v.Cliente.Nombre,
                        Total = v.Total,
                        Estado = v.Estado.ToString()
                    })
                    .SingleOrDefault(v => v.VentaId == ventaId);
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
