using Neptuno2022EF.Datos;
using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.Ciudad;
using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Servicios.Interfaces;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;

namespace Neptuno2022EF.Servicios.Servicios
{
    public class ServiciosClientes : IServiciosClientes
    {
        private readonly IRepositorioClientes _repositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ServiciosClientes(IRepositorioClientes repositorio, IUnitOfWork unitOfWork)
        {
            _repositorio = repositorio;
            _unitOfWork = unitOfWork;

        }

        public void Borrar(int id)
        {
            try
            {
                _repositorio.Borrar(id);
                _unitOfWork.SaveChanges();
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
                return _repositorio.EstaRelacionado(cliente);
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
                return _repositorio.Existe(cliente);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ClienteListDto> Filtrar(Func<Cliente, bool> predicado, int cantidadPorPagina, int paginaActual)
        {
            try
            {
                return _repositorio.Filtrar(predicado, cantidadPorPagina, paginaActual);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCantidad()
        {
            try
            {
                return _repositorio.GetCantidad();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCantidad(Func<Cliente, bool> predicado)
        {
            try
            {
                return _repositorio.GetCantidad(predicado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Cliente GetClientePorId(int id)
        {
            try
            {
                return _repositorio.GetClientePorId(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public List<ClienteListDto> GetClientePorPagina(int cantidad, int pagina)
        {
            try
            {
                return _repositorio.GetClientesPorPagina(cantidad, pagina);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ClienteListDto> GetClientePorPagina(int cantidad, int pagina, Pais pais, CiudadListDto ciudad)
        {
            try
            {
                return _repositorio.GetClientesPorPagina(cantidad, pagina,pais,ciudad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ClienteListDto> GetClientes()
        {
            try
            {
                return _repositorio.GetClientes();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ClienteListDto> GetClientes(int paisId, int ciudadId)
        {
            try
            {
                return _repositorio.GetClientes(paisId, ciudadId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Venta> GetComprasDelCliente(int clienteId)
        {
            throw new NotImplementedException();
        }

        public void Guardar(Cliente cliente)
        {
            try
            {
                if (cliente.Id==0)
                {
                    _repositorio.Agregar(cliente);
                }
                else
                {
                    _repositorio.Editar(cliente);
                }
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
