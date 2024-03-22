using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;



namespace presentacion
{
    public partial class Form1 : Form
    {
        private List<Pokemon> listapokemons; //atributo privado de la clase
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void cargar() //metodo q encapsula la carga en el datagridview
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listapokemons = negocio.listar();
                dgvPokemons.DataSource = negocio.listar();
                dgvPokemons.Columns["UrlImagen"].Visible = false;
                dgvPokemons.Columns["Id"].Visible = false;// oculto la columna con la url de la imagen
                cargarImagen(listapokemons[0].UrlImagen);//cargar la primer imagen en la lista
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
         Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; //se hace un casteo explicito porq se sabe q el objeto q esta en esa fila es de tipo Pokemon
            cargarImagen(seleccionado.UrlImagen);
        }

        private void cargarImagen(string imagen) //funcion para utilizar las exepciones si no hay imagenes  en la url
        {
            try
            {
                pboxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {

                pboxPokemon.Load("https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg");
            }
          
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarpokemon alta = new frmAgregarpokemon();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            frmAgregarpokemon modificar = new frmAgregarpokemon(seleccionado);
            modificar.ShowDialog();
            cargar();
        }
    }
}
