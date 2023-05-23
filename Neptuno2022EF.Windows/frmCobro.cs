using Microsoft.VisualBasic;
using Neptuno2022EF.Entidades.Enums;
using Neptuno2022EF.Windows.Helpers;
using Neptuno2022EF.Windows.Helpers.Enum;
using System;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmCobro : Form
    {
        public frmCobro()
        {
            InitializeComponent();
        }
        private decimal monto;
        private decimal importe;
        private FormaPago formaPago = 0;

        public void SetMonto(decimal monto)
        {
            this.monto = monto;
        }

        private void frmCobro_Load(object sender, EventArgs e)
        {
            ImporteLabel.Text = monto.ToString("N2");
        }

        private void EfectivoButton_Click(object sender, EventArgs e)
        {
            formaPago = FormaPago.Efectivo;
            var importeText = Interaction.InputBox("Ingrese el importe", "Pago en Efectivo", "0", 800, 400);
            decimal importeRecibido;
            if (!decimal.TryParse(importeText, out importeRecibido))
            {
                return;
            }
            else if (importeRecibido <= 0 || importeRecibido<monto)
            {
                HelperMessage.Mensaje(TipoMensaje.Error, "Importe inferior a lo que se debe pagar", "Error");
                return;
            }

            ImporteRecibidoLabel.Text = importeRecibido.ToString("N2");
            if (importeRecibido >= monto)
            {
                importe = monto;
                VueltoLabel.Text = (importeRecibido - monto).ToString("N2");

            }
            else
            {
                importe = importeRecibido;
            }
        }


        private void VisaButton_Click(object sender, EventArgs e)
        {
            formaPago = FormaPago.Visa;
            importe = decimal.Parse(ImporteLabel.Text);
            ImporteRecibidoLabel.Text = importe.ToString("N2");
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            formaPago = FormaPago.Mastercard;
            importe = decimal.Parse(ImporteLabel.Text);
            ImporteRecibidoLabel.Text = importe.ToString("N2");
        }

        private void btnAmex_Click(object sender, EventArgs e)
        {
            formaPago = FormaPago.Amex;
            importe = decimal.Parse(ImporteLabel.Text);
        }

        private void btnDiners_Click(object sender, EventArgs e)
        {
            formaPago = FormaPago.Diners;
            importe = decimal.Parse(ImporteLabel.Text);
            ImporteRecibidoLabel.Text = importe.ToString("N2");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                DialogResult = DialogResult.OK;
            }
        }
        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (formaPago == 0)
            {
                valido = false;
                errorProvider1.SetError(ImporteLabel, "Debe seleccionar una forma de pago");
            }

            return valido;
        }

        public FormaPago GetFormaDePago()
        {
            return formaPago;
        }

        public decimal GetImportePagado()
        {
            return importe;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
