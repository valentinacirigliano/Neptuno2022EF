using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Neptuno2022EF.Datos.Repositorios
{
    public class RepositorioCtasCtes : IRepositorioCtasCtes
    {
        private readonly NeptunoDbContext _context;

        public RepositorioCtasCtes(NeptunoDbContext context)
        {
            _context = context;
        }

        public List<CtaCteResumen> GetSaldos()
        {
            List<CtaCteResumen> lista = new List<CtaCteResumen>();
            var listaCtas = _context.CtasCtes.Include(c => c.Cliente)
                .GroupBy(c => c.Cliente.Nombre)
                .ToList();
            foreach (var g in listaCtas)
            {
                var cta = new CtaCteResumen
                {
                    Cliente = g.Key,
                    Saldo = g.Sum(x => x.Debe - x.Haber)
                };
                lista.Add(cta);
            }
            //var clientes = _context.Clientes.ToList();
            //foreach(var cliente in clientes)
            //{
            //    var saldo = _context.CtasCtes.LastOrDefault(c => c.ClienteId == cliente.Id).Saldo;
            //    var resumen = new CtaCteResumen()
            //    {
            //        Cliente = cliente.Nombre,
            //        Saldo = saldo,
            //    };
            //    lista.Add(resumen);

            //}
            return lista;
        }


        public List<CtaCteListDto> GetMovimientos(int clienteId)
        {
            return _context.CtasCtes.Include(c => c.Cliente)
                .Where(c => c.ClienteId == clienteId)
                .Select(c => new CtaCteListDto()
                {
                    Cliente = c.Cliente.Nombre,
                    Saldo = c.Saldo,
                    CtaCteId = c.CtaCteId,
                    FechaMovimiento = c.FechaMovimiento,
                    Movimiento = c.Movimiento,
                    Debe = c.Debe,
                    Haber = c.Haber,
                })

                .ToList();
        }

        public List<CtaCteListDto> GetMovimientos(string cliente)
        {
            return _context.CtasCtes.Include(c => c.Cliente)
                .Where(c => c.Cliente.Nombre == cliente)
                .OrderBy(c=>c.FechaMovimiento)
                .Select(c => new CtaCteListDto()
                {
                    Cliente = c.Cliente.Nombre,
                    Saldo = c.Saldo,
                    CtaCteId = c.CtaCteId,
                    FechaMovimiento = c.FechaMovimiento,
                    Movimiento = c.Movimiento,
                    Debe = c.Debe,
                    Haber = c.Haber,
                })

                .ToList();
        }

        public CtaCte GetCuentaPorCliente(string cliente)
        {
            try
            {
                var conferencia = _context.CtasCtes.Include(c => c.Cliente)
                   .SingleOrDefault(c => c.Cliente.Nombre == cliente);

                return conferencia;
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
                //return _context.CtasCtes.Include(c => c.Cliente)
                //    .LastOrDefault(c => c.Cliente.Nombre == cliente).Saldo;
                List<CtaCteResumen> lista = new List<CtaCteResumen>();
                var listaCtas = _context.CtasCtes.Include(c => c.Cliente)
                    .GroupBy(c => c.Cliente.Nombre)
                    .ToList();
                foreach (var g in listaCtas)
                {
                    var cta = new CtaCteResumen
                    {
                        Cliente = g.Key,
                        Saldo = g.Sum(x => x.Debe - x.Haber)
                    };
                    lista.Add(cta);
                }
                return lista.Find(c => c.Cliente==cliente).Saldo;

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
                return _context.Clientes.Include(c => c.Pais)
                    .Include(c => c.Ciudad)
                    .SingleOrDefault(c=>c.Nombre==nombre);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Agregar(CtaCte ctaCteGuardar)
        {
            _context.CtasCtes.Add(ctaCteGuardar);
        }

        public void PagoTotalCuenta(Cliente cliente, FormaPago forma, decimal importeRecibido)
        {
            throw new NotImplementedException();
        }
    }
}
