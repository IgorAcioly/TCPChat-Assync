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
using TCPChat_Assync.Cadastro;
using TCPChat_Assync.Repository;

namespace TCPChat_Assync
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            CadastroScreen telaCadastro = new CadastroScreen();
            telaCadastro.Show();
            this.Close();
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            var mongo = new TCPChat_Assync.Repository.MongoDB();
            string nomeUsuario = txtBox_Usuario.Text;
            string senha = txtBox_Senha.Text;
            var usuario = mongo.LoginUser(nomeUsuario, senha);

            if (usuario is Admin admin)
            {
                ServerScreen telaServer = new ServerScreen();
                telaServer.Show();
                this.Close();
            }
            else if (usuario is Client client)
            {
                ClientScreen telaClient = new ClientScreen();
                telaClient.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuário não encontrado\n"+"Insira os dados informados corretamente ou realize cadastro");
            }
        }
    }
}
