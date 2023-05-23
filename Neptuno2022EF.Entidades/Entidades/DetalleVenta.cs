﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuno2022EF.Entidades.Entidades
{
    public class DetalleVenta
    {
        public int DetalleVentaId { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public byte[] RowVersion { get; set; }

        public Producto Producto { get; set; }
    }
}
