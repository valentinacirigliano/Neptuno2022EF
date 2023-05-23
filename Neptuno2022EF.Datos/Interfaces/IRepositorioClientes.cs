using Neptuno2022EF.Entidades.Dtos.Ciudad;
using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Entidades;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;

namespace Neptuno2022EF.Datos.Interfaces
{
    public interface IRepositorioClientes
    {
        void Agregar(Cliente cliente);
        void Borrar(int id);
        void Editar(Cliente cliente);
        bool EstaRelacionado(Cliente cliente);
        bool Existe(Cliente cliente);
        Cliente GetClientePorId(int id);
        List<ClienteListDto> GetClientes();
        List<ClienteListDto> GetClientes(int paisId, int ciudadId);
        List<ClienteListDto> GetClientesPorPagina(int cantidad, int pagina);
        List<ClienteListDto> GetClientesPorPagina(int cantidad, int pagina, Pais pais, CiudadListDto ciudad);
        int GetCantidad();
        List<ClienteListDto> Filtrar(Func<Cliente, bool> predicado, object cantidad, object pagina);
        int GetCantidad(Func<Cliente, bool> predicado);
    }
}
