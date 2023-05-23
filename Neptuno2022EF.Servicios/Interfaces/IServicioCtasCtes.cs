using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuno2022EF.Servicios.Interfaces
{
    public interface IServicioCtasCtes
    {
        List<CtaCteListDto> Filtrar(Func<Cliente, bool> predicado);
        List<CtaCteResumen> GetSaldos();
        List<CtaCteListDto> GetMovimientos(int ClienteId);
        List<CtaCteListDto> GetMovimientos(string cliente);
        CtaCte GetCuentaPorCliente(string cliente);
        decimal GetSaldo(string cliente);

        Cliente GetClientePorNombre(string nombre);
        void GuardarCtaCte(CtaCte ctaCte);
        void PagoTotalCuenta(Cliente cliente, FormaPago forma, decimal importeRecibido);
        void PagoFactura(int nroFactura, Cliente cliente, FormaPago forma, decimal importeRecibido);
    }
}

