using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominioD;
using negocioD;

namespace practica_discos
{
    public partial class frmDiscos : Form
    {
        // Lista privada para guardar los discos
        private List<Discos> listaDiscos;
        
        public frmDiscos()
        {
            InitializeComponent();
        }

        // RECORDAR PONER LO DE CBO CAMPO (ESTÁ EN EL DE POKEMON)
        private void frmDiscos_Load(object sender, EventArgs e)
        {
            cargar();

            // Acá agrego el cboCampo cuando hago lo de filtrar
            cboCampo.Items.Add("CantidadCanciones");
            cboCampo.Items.Add("Titulo");
            cboCampo.Items.Add("Estilo");

        }

        private void dgvDiscos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvDiscos.CurrentRow != null)
            {
                Discos seleccionado = (Discos)dgvDiscos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagenTapa);
            }      
        }

        private void cargar()
        {
            DiscosNegocio negocio = new DiscosNegocio();

            try
            {
                listaDiscos = negocio.listar();

                dgvDiscos.DataSource = listaDiscos;

                // LLamo para ocultar las columnas puestas en el metodo aparte
                ocultarColumnas();

                cargarImagen(listaDiscos[0].UrlImagenTapa);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvDiscos.Columns["UrlImagenTapa"].Visible = false;
            dgvDiscos.Columns["Id"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDiscos.Load(imagen);

            }
            catch (Exception ex)
            {

                pbxDiscos.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaDisco alta = new frmAltaDisco();

            alta.ShowDialog();
            cargar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Discos seleccionado;
            seleccionado = (Discos)dgvDiscos.CurrentRow.DataBoundItem;

            frmAltaDisco modificar = new frmAltaDisco(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar()
        {
            DiscosNegocio negocio = new DiscosNegocio();

            Discos seleccionado;


            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estás seguro de eliminar el Disco?", "Eliminar Disco", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Discos)dgvDiscos.CurrentRow.DataBoundItem;

                    negocio.eliminar(seleccionado.Id);

                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }

            if(cboCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }

            //if(txtFiltroAvanzado.Text == "")
            //{
                //MessageBox.Show("Por favor, escriba por lo que quiera filtrar");
                //return true;
            //}

            // ---------------------------------------------------------------------

            if(cboCampo.SelectedItem.ToString() == "CantidadCanciones")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Ingrese un número en el filtro númerico...");
                    return true;
                }

                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Ingrese un número para filtrar en el campo númerico...");
                    return true;
                }
            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }

            return true;
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Discos> listaFiltrada;

            string filtro = txtFiltro.Text;

            if(filtro.Length >= 2)
            {
                listaFiltrada = listaDiscos.FindAll(x => x.Titulo.ToUpper().Contains(filtro.ToUpper())
                || x.IdEstilo.Descripcion.ToUpper().Contains(filtro.ToUpper()) 
                    || x.IdTipoEdicion.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaDiscos;
            }

            dgvDiscos.DataSource = null;

            dgvDiscos.DataSource = listaFiltrada;

            ocultarColumnas();
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();

            try
            {
                if (validarFiltro())
                {
                    return;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvDiscos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "CantidadCanciones")
            {
                cboCriterio.Items.Clear();

                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();

                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
