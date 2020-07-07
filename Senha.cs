using System;
using System.Windows.Forms;

namespace ImportDB
{
    public partial class Senha : Form
    {
        public bool senha = false;

        public Senha()
        {
            InitializeComponent();
            GerarSenha();
        }

        private string GerarSenha()
        {
            Random random = new Random();
            return contraSenha.Text = random.Next(0,9).ToString() + " " + random.Next(0, 9).ToString() + " " + random.Next(0, 9).ToString() + " " + random.Next(0, 9).ToString();
        }

        private bool Confirmar() 
        {
            return (senhaDigitada.Text == contraSenha.Text.Replace(" ","")) ? true : false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConfirma_Click(object sender, EventArgs e)
        {
            senha = Confirmar();

            if (senha)
            {
                Close();
            }
            else 
            {
                MessageBox.Show("Senha não confere");
            }
        }
    }
}
