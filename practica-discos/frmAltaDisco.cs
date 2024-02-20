using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocioD;
using dominioD;
using System.Configuration;
using System.IO;

namespace practica_discos
{
    public partial class frmAltaDisco : Form
    {
        private Discos discoNull = null;

        public frmAltaDisco()
        {
            InitializeComponent();
        }

        private OpenFileDialog archivo = null;

        public frmAltaDisco(Discos disco)
        {
            InitializeComponent();

            this.discoNull = disco;

            Text = "Modificar Disco";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EstilosNegocio estilosNegocio = new EstilosNegocio();
            TiposEdicionNegocio tiposEdicionNegocio = new TiposEdicionNegocio();

            try
            {
                cboEstilo.DataSource = estilosNegocio.listar();
                cboEstilo.ValueMember = "Id";
                cboEstilo.DisplayMember = "Descripcion";

                cboTipoEdicion.DataSource = tiposEdicionNegocio.listar();
                cboTipoEdicion.ValueMember = "Id";
                cboTipoEdicion.DisplayMember = "Descripcion";

                if(discoNull != null )
                {
                    txtTitulo.Text = discoNull.Titulo;
                    dtpFechaLanzamiento.Text = discoNull.FechaLanzamiento.ToString();
                    txtCantidadCanciones.Text = discoNull.CantidadCanciones.ToString();
                    txtUrlImagen.Text = discoNull.UrlImagenTapa;

                    cargarImagen(discoNull.UrlImagenTapa);

                    cboEstilo.SelectedValue = discoNull.IdEstilo.Id;
                    cboTipoEdicion.SelectedValue = discoNull.IdTipoEdicion.Id;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();

            try
            {
                if(discoNull == null)
                {
                    discoNull = new Discos();
                }

                discoNull.Titulo = txtTitulo.Text;
                discoNull.FechaLanzamiento = dtpFechaLanzamiento.Value;
                discoNull.CantidadCanciones = int.Parse(txtCantidadCanciones.Text);
                discoNull.UrlImagenTapa = txtUrlImagen.Text;

                discoNull.IdEstilo = (Estilos) cboEstilo.SelectedItem;
                discoNull.IdTipoEdicion = (TiposEdicion) cboTipoEdicion.SelectedItem;

                if(discoNull.Id != 0)
                {
                    negocio.modificar(discoNull);
                    MessageBox.Show("¡Disco modificado exitosamente!");
                }
                else
                {
                    negocio.agregar(discoNull);
                    MessageBox.Show("¡Disco agregado exitosamente!");
                }

                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();

            archivo.Filter = "jpg|*.jpg|png|*.png";

            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDisco.Load(imagen);

            }
            catch (Exception ex)
            {

                pbxDisco.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }


    }
}
