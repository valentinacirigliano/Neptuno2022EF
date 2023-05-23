using Neptuno2022EF.Servicios.Interfaces;
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
    public partial class frmDetalleCliente : Form
    {
        private readonly IServiciosClientes _servicio;
        Cliente cliente;
        public frmDetalleCliente(IServiciosClientes servicio)
        {
            InitializeComponent();
            _servicio = servicio;
        }

        internal void SetCliente(Cliente cliente)
        {
            this.cliente = cliente;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDetalleCliente_Load(object sender, EventArgs e)
        {
            if (cliente != null)
            {
                lblNombre.Text = cliente.Nombre;
                lblDireccion.Text = cliente.Direccion;
                lblPais.Text = cliente.Pais.NombrePais;
                lblCiudad.Text = cliente.Ciudad.NombreCiudad;
                lblCodPostal.Text = cliente.CodPostal != null ? cliente.CodPostal : "-";
                lblTelFijo.Text = cliente.TelefonoFijo != null ? cliente.TelefonoFijo : "-";
                lblTelMovil.Text = cliente.TelefonoMovil != null ? cliente.TelefonoMovil : "-";
            }

        }
    }
}
