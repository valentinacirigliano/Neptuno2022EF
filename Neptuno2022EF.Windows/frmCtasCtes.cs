using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Servicios.Interfaces;
using Neptuno2022EF.Windows.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmCtasCtes : Form
    {
        private readonly IServicioCtasCtes _servicio;
        private readonly IServiciosVentas _serviciosVentas;
        private readonly IServiciosClientes _serviciosClientes;
        private List<CtaCteResumen> lista;
        private CtaCteResumen cuenta;
        private List<CtaCteResumen> listaFiltrada;
        public frmCtasCtes(IServicioCtasCtes servicio, IServiciosVentas serviciosVentas, IServiciosClientes serviciosClientes)
        {
            InitializeComponent();
            _servicio = servicio;
            _serviciosVentas = serviciosVentas;
            _serviciosClientes = serviciosClientes;
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCtasCtes_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
            lblRegistros.Text=_servicio.GetSaldos().Count().ToString();

        }

        private void MostrarDatosEnGrilla()
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            foreach (CtaCteResumen ctaCte in lista)
            {
                DataGridViewRow r = GridHelper.ConstruirFila(dgvDatos);
                GridHelper.SetearFila(r, ctaCte);
                if (ctaCte.Saldo > 0)
                {
                    r.Cells[colSaldo.Index].Style.BackColor = Color.Red;
                }
                else if (ctaCte.Saldo <= 0)
                {
                    r.Cells[colSaldo.Index].Style.BackColor = Color.Green;

                }
                r.Tag = ctaCte;
                GridHelper.AgregarFila(dgvDatos, r);
            }
        }

        private void RecargarGrilla()
        {
            try
            {
                lista = _servicio.GetSaldos();
                MostrarDatosEnGrilla();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void tsbDetalle_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            cuenta = (CtaCteResumen)r.Tag;
            frmDetalleCtaCte frm = new frmDetalleCtaCte(_servicio,_serviciosVentas,_serviciosClientes) { Text = "Detalles cuenta corriente" };
            
            List<CtaCteListDto> lista = _servicio.GetMovimientos(cuenta.Cliente);
            frm.SetCtaCte(lista);
            frm.Show(this);


        }


        private void txtBuscarCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscarCliente.Text.Length > 0)
            {
                listaFiltrada = lista.Where(c => c.Cliente.StartsWith(txtBuscarCliente.Text)).ToList();

            }
            else
            {
                listaFiltrada = lista;
            }
            FormHelper.MostrarDatosEnGrilla(dgvDatos, listaFiltrada);
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
