using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.Ciudad;
using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Entidades;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Neptuno2022EF.Datos.Repositorios
{
    public class RepositorioClientes : IRepositorioClientes
    {
        private readonly NeptunoDbContext _context;

        public RepositorioClientes(NeptunoDbContext context)
        {
            _context = context;
        }

        public void Agregar(Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Borrar(int id)
        {
            try
            {
                var clienteInDb = GetClientePorId(id);
                if (clienteInDb == null)
                {
                    throw new Exception("Registro borrado por otro usuario");
                }
                _context.Entry(clienteInDb).State = EntityState.Deleted;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Editar(Cliente cliente)
        {
            try
            {
                var clienteInDb = GetClientePorId(cliente.Id);
                if (clienteInDb == null)
                {
                    throw new Exception("Registro borrado por otro usuario");
                }
                _context.Entry(cliente).State = EntityState.Modified;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EstaRelacionado(Cliente cliente)
        {
            try
            {
                return _context.Ventas.Any(v => v.ClienteId == cliente.Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Existe(Cliente cliente)
        {
            try
            {
                if (cliente.Id == 0)
                {
                    return _context.Clientes.Any(c => c.Nombre == cliente.Nombre);
                }
                return _context.Clientes.Any(c => c.Nombre == cliente.Nombre && c.Id != cliente.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ClienteListDto> Filtrar(Func<Cliente, bool> predicado, object cantidad, object pagina)
        {
            return _context.Clientes.Include(c => c.Pais)
                 .Include(c => c.Ciudad)
                 .Where(predicado)
                 .Select(c => new ClienteListDto
                 {
                      ClienteId = c.Id,
                      NombreCliente = c.Nombre,
                      Pais = c.Pais.NombrePais,
                       Ciudad = c.Ciudad.NombreCiudad
                 }).ToList();
        }


        public int GetCantidad()
        {
            return _context.Clientes.Count();
        }

        public int GetCantidad(Func<Cliente, bool> predicado)
        {
            return _context.Clientes.Count(predicado);
        }

        public Cliente GetClientePorId(int id)
        {
            try
            {
                return _context.Clientes.Include(c => c.Pais)
                    .Include(c => c.Ciudad)
                    .SingleOrDefault(c => c.Id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public List<ClienteListDto> GetClientes()
        {
            return _context.Clientes
                .Include(c => c.Pais)
                .Include(c => c.Ciudad)
                .Select(c => new ClienteListDto
                {
                    ClienteId = c.Id,
                    NombreCliente = c.Nombre,
                    Pais=c.Pais.NombrePais,
                    Ciudad=c.Ciudad.NombreCiudad
                }).ToList();
        }

        public List<ClienteListDto> GetClientes(int paisId, int ciudadId)
        {
            try
            {
                return _context.Clientes.Include(c => c.Pais)
                    .Include(c => c.Ciudad)
                    .Where(c=>c.PaisId==paisId && c.CiudadId==ciudadId)
                    .Select(c => new ClienteListDto
                    {
                        ClienteId = c.Id,
                        NombreCliente = c.Nombre,
                        Pais = c.Pais.NombrePais,
                        Ciudad = c.Ciudad.NombreCiudad
                    }).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ClienteListDto> GetClientesPorPagina(int cantidad, int pagina)
        {
            return _context.Clientes.OrderBy(c => c.Nombre)
                .Skip(cantidad * (pagina - 1))
                .Take(cantidad)
                .Select(c => new ClienteListDto
                {
                    ClienteId = c.Id,
                    NombreCliente = c.Nombre,
                    Pais = c.Pais.NombrePais,
                    Ciudad = c.Ciudad.NombreCiudad
                }).ToList();
        }
        public List<ClienteListDto> GetClientesPorPagina(int cantidad, int pagina, Pais pais=null, CiudadListDto ciudad =null)
        {
            if (pais != null && ciudad == null)
            {
                return _context.Clientes.OrderBy(c => c.Nombre)
                 .Skip(cantidad * (pagina - 1))
                 .Take(cantidad)
                 .Where(c=>c.Pais.PaisId == pais.PaisId)
                 .Select(c => new ClienteListDto
                 {
                     ClienteId = c.Id,
                     NombreCliente = c.Nombre,
                     Pais = c.Pais.NombrePais,
                     Ciudad = c.Ciudad.NombreCiudad
                 }).ToList();
            }
            if (pais != null && ciudad != null)
            {
                return _context.Clientes.OrderBy(c => c.Nombre)
                 .Skip(cantidad * (pagina - 1))
                 .Take(cantidad)
                 .Where(c => c.Pais.PaisId == pais.PaisId &&  c.Ciudad.CiudadId == ciudad.CiudadId)
                 .Select(c => new ClienteListDto
                 {
                     ClienteId = c.Id,
                     NombreCliente = c.Nombre,
                     Pais = c.Pais.NombrePais,
                     Ciudad = c.Ciudad.NombreCiudad
                 }).ToList();
            }
            if (pais == null && ciudad != null)
            {
                return _context.Clientes.OrderBy(c => c.Nombre)
                 .Skip(cantidad * (pagina - 1))
                 .Take(cantidad)
                 .Where(c => c.Ciudad.CiudadId == ciudad.CiudadId)
                 .Select(c => new ClienteListDto
                 {
                     ClienteId = c.Id,
                     NombreCliente = c.Nombre,
                     Pais = c.Pais.NombrePais,
                     Ciudad = c.Ciudad.NombreCiudad
                 }).ToList();
            }

            return _context.Clientes.OrderBy(c => c.Nombre)
                .Skip(cantidad * (pagina - 1))
                .Take(cantidad)
                .Select(c => new ClienteListDto
                {
                    ClienteId = c.Id,
                    NombreCliente = c.Nombre,
                    Pais = c.Pais.NombrePais,
                    Ciudad = c.Ciudad.NombreCiudad
                }).ToList();
        }

    }
}
