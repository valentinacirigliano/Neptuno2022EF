using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Dtos.DetalleVenta;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Servicios.Interfaces;
using Neptuno2022EF.Servicios.Servicios;
using Neptuno2022EF.Windows.Helpers;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmResumenCompras : Form
    {
        private readonly IServiciosVentas _servicioVentas;
        Cliente cliente;
        List<VentaResumen> lista;
        VentaResumen venta;
        public frmResumenCompras(IServiciosVentas serviciosVentas)
        {
            InitializeComponent();
            _servicioVentas= serviciosVentas;
        }
        
        internal void SetCliente(Cliente cliente)
        {
            this.cliente = cliente;
        }
        

        private void frmResumenCompras_Load(object sender, EventArgs e)
        {
            lista = _servicioVentas.GetResumenCliente(cliente.Id);
            MostrarGrilla();
        }

        private void MostrarGrilla()
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            foreach (var venta in lista)
            {
                DataGridViewRow r = GridHelper.ConstruirFila(dgvDatos);
                GridHelper.SetearFila(r, venta);
                GridHelper.AgregarFila(dgvDatos, r);
            }

        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbDetalle_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            venta = (VentaResumen)r.Tag;
            frmDetalleVenta frm = new frmDetalleVenta() { Text = "Detalles de la compra realizada" };
            VentaListDto ventaListDto =_servicioVentas.GetVentaListDtoPorId(venta.VentaId);
            var detalle = _servicioVentas.GetDetalleVenta(venta.VentaId);
            var ventaDetalleDto = new VentaDetalleDto()
            {
                venta = ventaListDto,
                detalleVenta = detalle
            };
            frm.SetVenta(ventaDetalleDto);
            frm.Show(this);
        }
    }
}
