using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuno2022EF.Datos.Interfaces
{
    public interface IRepositorioCtasCtes
    {
        List<CtaCteResumen> GetSaldos();
        List<CtaCteListDto> GetMovimientos(int ClienteId);
        List<CtaCteListDto> GetMovimientos(string cliente);
        
        CtaCte GetCuentaPorCliente(string cliente);
        decimal GetSaldo(string cliente);
        Cliente GetClientePorNombre(string nombre);
        void PagoTotalCuenta(Cliente cliente, FormaPago forma, decimal importeRecibido);
    }
}
