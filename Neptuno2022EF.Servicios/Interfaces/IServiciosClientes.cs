using Neptuno2022EF.Entidades.Dtos.Ciudad;
using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Entidades;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuno2022EF.Servicios.Interfaces
{
    public interface IServiciosClientes
    {
        void Borrar(int id);
        bool EstaRelacionado(Cliente cliente);
        bool Existe(Cliente cliente);
        Cliente GetClientePorId(int id);
        List<ClienteListDto> GetClientes();
        List<ClienteListDto> GetClientes(int paisId, int ciudadId);
        void Guardar(Cliente cliente);
        List<ClienteListDto> GetClientePorPagina(int cantidadPorPagina, int paginaActual);
        List<ClienteListDto> GetClientePorPagina(int cantidadPorPagina, int paginaActual, Pais pais, CiudadListDto ciudad);
        int GetCantidad();
        List<Venta> GetComprasDelCliente(int clienteId);
        List<ClienteListDto> Filtrar(Func<Cliente, bool> predicado, int cantidadPorPagina, int paginaActual);
        int GetCantidad(Func<Cliente, bool> predicado);
    }
}
