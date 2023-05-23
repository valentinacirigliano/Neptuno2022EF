using Neptuno2022EF.Entidades.Dtos.Cliente;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Servicios.Interfaces;
using Neptuno2022EF.Windows.Helpers;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmClientes : Form
    {
        private readonly IServiciosClientes _servicio;
        private readonly IServiciosVentas _serviciosVentas;
        private List<ClienteListDto> lista;


        private int cantidadPorPagina = 20;
        private int registros;
        private int paginas;
        private int paginaActual = 1;
        private bool filtroOn = false;

        Func<Cliente, bool> predicado;


        public frmClientes(IServiciosClientes servicio, IServiciosVentas serviciosVentas)
        {
            InitializeComponent();
            _servicio = servicio;
            _serviciosVentas = serviciosVentas;

        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void MostrarDatosEnGrilla()
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            foreach (ClienteListDto cliente in lista)
            {
                DataGridViewRow r = GridHelper.ConstruirFila(dgvDatos);
                GridHelper.SetearFila(r, cliente);
                GridHelper.AgregarFila(dgvDatos,r);
            }
            lblRegistros.Text = registros.ToString();
            lblPaginaActual.Text = paginaActual.ToString();
            lblPaginas.Text = paginas.ToString();
        }


        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmClienteAE frm = new frmClienteAE(_servicio) { Text = "Agregar Cliente" };
            DialogResult dr = frm.ShowDialog(this);
            RecargarGrilla();
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                var r = dgvDatos.SelectedRows[0];
                ClienteListDto clienteDto = (ClienteListDto)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Confirma la baja de {clienteDto.NombreCliente}?",
                    "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                var cliente = _servicio.GetClientePorId(clienteDto.ClienteId);
                if (cliente == null)
                {
                    MessageBox.Show("Registro dado de baja por otro usuario", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    RecargarGrilla();
                    return;
                }
                if (!_servicio.EstaRelacionado(cliente))
                {
                    _servicio.Borrar(cliente.Id);
                    GridHelper.BorrarFila(dgvDatos,r);
                    MessageBox.Show("Registro borrado satisfactoriamente!!!",
                        "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Registro relacionado...Baja denegada", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Mensaje",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void RecargarGrilla()
        {
            try
            {
                if (filtroOn)
                {
                    registros = _servicio.GetCantidad(predicado);
                }
                else
                {
                    registros = _servicio.GetCantidad();

                }
                paginas = CalculosHelper.CalcularCantidadPaginas(registros, cantidadPorPagina);
                paginaActual = 1;
                MostrarPaginado();
                
            }
            catch (Exception)
            {

                throw;
            }


        }


        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            ClienteListDto clienteDto = (ClienteListDto)r.Tag;
            var cliente = _servicio.GetClientePorId(clienteDto.ClienteId);
            if (cliente == null)
            {
                MessageBox.Show("Registro dado de baja por otro usuario");
                RecargarGrilla();
                return;

            }

            frmClienteAE frm = new frmClienteAE(_servicio) { Text = "Editar Cliente" };
            frm.SetCliente(cliente);
            DialogResult dr = frm.ShowDialog(this);
            RecargarGrilla();
        }

        private void tsbFiltrar_Click(object sender, EventArgs e)
        {
            frmSeleccionarPaisCiudad frm = new frmSeleccionarPaisCiudad() { Text = "Seleccionar País y Ciudad" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) { return; }
            try
            {
                var pais = frm.GetPais();
                var ciudad = frm.GetCiudad();
                
                if (ciudad ==null)
                {
                    predicado = c => c.PaisId == pais.PaisId;
                }
                else
                {
                    predicado=c=>c.PaisId==pais.PaisId && c.CiudadId==ciudad.CiudadId;
                }
                filtroOn = true;
                RecargarGrilla();
                tsbFiltrar.BackColor = Color.Orange;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {

            filtroOn = false;
            RecargarGrilla();
            tsbFiltrar.BackColor = Color.White;
        }
        
        private void MostrarPaginado()
        {
            if (filtroOn)
            {
                lista = _servicio.Filtrar(predicado, cantidadPorPagina, paginaActual);
            }
            else
            {
                lista = _servicio.GetClientePorPagina(cantidadPorPagina, paginaActual);

            }
            MostrarDatosEnGrilla();
        }


        private void btnPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
            MostrarPaginado();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (paginaActual == 1)
            {
                return;
            }
            paginaActual--;
            MostrarPaginado();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (paginaActual == paginas)
            {
                return;
            }
            paginaActual++;
            MostrarPaginado();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = paginas;
            MostrarPaginado();
        }

        private void tsbDetalle_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            ClienteListDto clienteDto = (ClienteListDto)r.Tag;
            var cliente = _servicio.GetClientePorId(clienteDto.ClienteId);
            frmDetalleCliente frm = new frmDetalleCliente(_servicio) { Text = "Detalles del Cliente" };
            frm.SetCliente(cliente);
            frm.Show(this);
            RecargarGrilla();
        }

        private void tsbResumenCompras_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            ClienteListDto clienteDto = (ClienteListDto)r.Tag;
            var cliente = _servicio.GetClientePorId(clienteDto.ClienteId);
            frmResumenCompras frm = new frmResumenCompras(_serviciosVentas) { Text = "Resumen de compras del cliente" };
            frm.SetCliente(cliente);
            frm.Show(this);
            RecargarGrilla();
        }
    }
}
