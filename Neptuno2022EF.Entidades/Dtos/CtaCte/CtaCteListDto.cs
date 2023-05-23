using Neptuno2022EF.Entidades.Dtos.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuno2022EF.Entidades.Dtos.CtaCte
{
    public class CtaCteListDto
    {
        public int CtaCteId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Movimiento { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal Saldo { get; set; }
        public string Cliente { get; set; } 
    }
}
