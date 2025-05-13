using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TCPChat_Assync.Cadastro
{

    public partial class CadastroScreen : Window
    {
        public CadastroScreen()
        {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen telaLogin = new LoginScreen();
            telaLogin.Show();
            this.Close();
        }

        // Cadastra cliente no banco de dados
        private void btnConcluir_Click(object sender, RoutedEventArgs e)
        {
            var mongo = new TCPChat_Assync.Repository.MongoDB();

            string nomeCompleto = txtBox_NomeCompleto.Text;
            string nomeUsuario = txtBox_NomeUsuario.Text;
            string senha = txtBox_Senha.Text;

            mongo.InsertUser(nomeCompleto, nomeUsuario, senha);

            MessageBox.Show("Cadastro concluído com sucesso!");

        }
    }
}
