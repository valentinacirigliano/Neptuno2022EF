using Neptuno2022EF.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuno2022EF.Entidades.Dtos.Venta
{
    public class VentaResumen
    {
        public  int VentaId { get; set; }
        public DateTime Fecha { get; set; }
        public Estado Estado { get; set; }
        public decimal Total { get; set; }
    }
}
