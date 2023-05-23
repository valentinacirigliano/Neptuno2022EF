using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using Neptuno2022EF.Servicios.Interfaces;
using Neptuno2022EF.Windows.Helpers;
using Neptuno2022EF.Windows.Helpers.Enum;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmDetalleCtaCte : Form
    {
        private readonly IServicioCtasCtes _servicio;
        private readonly IServiciosVentas _serviciosVentas;
        private readonly IServiciosClientes _serviciosClientes;
        Cliente cliente;
        List<CtaCteListDto> lista;
        public frmDetalleCtaCte(IServicioCtasCtes servicio, IServiciosVentas serviciosVentas, IServiciosClientes serviciosClientes)
        {
            InitializeComponent();
            _servicio = servicio;
            _serviciosVentas = serviciosVentas;
            _serviciosClientes = serviciosClientes;
        }
        internal void SetCtaCte(List<CtaCteListDto> lista)
        {
            this.lista = lista;
            cliente = _servicio.GetClientePorNombre(lista[0].Cliente);
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDetalleCtaCte_Load(object sender, EventArgs e)
        {
            txtCliente.Text = cliente.Nombre;
            txtDireccion.Text = cliente.Direccion;
            txtLocalidad.Text = cliente.Ciudad.NombreCiudad;
            txtCP.Text = cliente.CodPostal;
            RecargarGrilla();
        }
        public void RecargarGrilla()
        {
            MostrarGrilla(_servicio.GetMovimientos(cliente.Id));
            txtSaldo.Text = _servicio.GetSaldo(cliente.Nombre).ToString();
        }
           
        private void MostrarGrilla(List<CtaCteListDto> lista)
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            foreach (var ctaCte in lista)
            {
                DataGridViewRow r = GridHelper.ConstruirFila(dgvDatos);
                GridHelper.SetearFila(r, ctaCte);
                GridHelper.AgregarFila(dgvDatos, r);
            }            
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIngresarPago_Click(object sender, EventArgs e)
        {
            decimal saldo = _servicio.GetSaldo(cliente.Nombre);
            frmCobro frm = new frmCobro() { Text = "Pago total de la cuenta corriente" };
            frm.SetMonto(saldo);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            if (dr == DialogResult.OK)
            {
                try
                {
                    FormaPago forma = frm.GetFormaDePago();
                    decimal importeRecibido = frm.GetImportePagado();
                    _servicio.PagoTotalCuenta(cliente, forma, importeRecibido);
                    HelperMessage.Mensaje(TipoMensaje.OK, "Pago efectuado!!!", "Operación Exitosa");
                }
                catch (Exception exception)
                {
                    HelperMessage.Mensaje(TipoMensaje.Error, exception.Message, "ERROR");
                }

            }

            RecargarGrilla();
        }

        private void btnIngresarPago_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0 ||VerificarSiSePuedeRealizarPago()==false)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            CtaCteListDto cuentaDto = (CtaCteListDto)r.Tag;
            frmCobro frm = new frmCobro() { Text = "Pago de la factura" };
            frm.SetMonto(cuentaDto.Debe);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            if (dr == DialogResult.OK)
            {
                try
                {
                    FormaPago forma = frm.GetFormaDePago();
                    decimal importeRecibido = frm.GetImportePagado();
                    _servicio.PagoFactura(nroFactura,cliente, forma, importeRecibido);
                    HelperMessage.Mensaje(TipoMensaje.OK, "Pago efectuado!!!", "Operación Exitosa");
                }
                catch (Exception exception)
                {
                    HelperMessage.Mensaje(TipoMensaje.Error, exception.Message, "ERROR");
                }

            }


            RecargarGrilla();
        }
        int nroFactura;
        Venta ventaActual;
        bool VerificarSiSePuedeRealizarPago()
        {
            List<VentaResumen> cuentasDelCliente = _serviciosVentas.GetResumenCliente(cliente.Id);
            decimal saldo = _servicio.GetSaldo(cliente.Nombre);
            DataGridViewRow r = dgvDatos.SelectedRows[0];
            var cuenta = r.Tag as CtaCteListDto;

            string movimiento = cuenta.Movimiento.ToString();
            string[] partes = movimiento.Split(' ');
            nroFactura = int.Parse(partes[1]);
            ventaActual = (Venta)_serviciosVentas.GetVentaPorId(nroFactura);
            if (saldo == 0)
            {
                btnIngresarPago.Enabled = false;
                btnPagoTotal.Enabled = false;
            }
            else if (saldo > 0)
            {
                btnPagoTotal.Enabled = true;

                if (ventaActual.Estado == Estado.Impaga)
                {
                    btnIngresarPago.Enabled = true;
                }
                else
                {
                    btnIngresarPago.Enabled = false;
                }

            }
            return btnIngresarPago.Enabled;
        }
        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (_servicio.GetSaldo(cliente.Nombre) == 0)
            {
                btnIngresarPago.Enabled = false;
            }
            else
            {
                btnIngresarPago.Enabled = true;
            }
        }
    }
}
