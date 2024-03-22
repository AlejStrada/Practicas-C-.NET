using Dominio;
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
    public partial class frmAgregarpokemon : Form
    {
        private Pokemon pokemon = null;
        public frmAgregarpokemon()
        {
            InitializeComponent();
        }
        public frmAgregarpokemon(Pokemon pokemon) //formulario para modificar
        {
            InitializeComponent();
            this.pokemon = pokemon; //lo utilizo para validar si es agregar o modiicvar un pokemoncargo la clase pokemon con un pokemon; asi pasas de ser null a contener un objeto
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
          
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (pokemon == null)
                    pokemon = new Pokemon();

                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;

                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificacion de datos exitosa");
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("Carga de datos exitosa");
                }
                Close();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAgregarpokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio negocio = new ElementoNegocio();
            try
            {
                cboTipo.DataSource = negocio.listar();
                cboTipo.ValueMember = "Id";          //se utiliza para presetear un valor
                cboTipo.DisplayMember = "Descripcion";  //
                cboDebilidad.DataSource = negocio.listar();
                cboDebilidad.ValueMember = "Id";
                cboDebilidad.DisplayMember = "Descripcion";

                //validacion para saber si modifuca o agrega nuevo pokemon:

                 if (pokemon != null) // valido si esta seleccionado o no una linea para precargar los formularios para modificar o para agregar uno nuevo
                {
                     txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                     txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id; //se setea el valor que tiene el objeto seleccionado en el desplegable
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);

        }
        private void cargarImagen(string imagen) //funcion para utilizar las exepciones si no hay imagenes  en la url
        {
            try
            {
                pboxUrlImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pboxUrlImagen.Load("https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg");
            }

        }

    }
}
