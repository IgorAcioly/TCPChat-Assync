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
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;

namespace TCPChat_Assync
{
    /// Realiza a conexão com o servidor e envia mensagens para ele.

    public partial class ClientScreen : Window
    {
        private TcpListener listener;
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        public string TextToSend;
        private CancellationTokenSource cts;

        public ClientScreen()
        {
            InitializeComponent();
        }

        private async void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(txtBox_IPServer.Text), int.Parse(txtBox_PortServer.Text));

            try
            {
                txtBox_StatusMensagem.AppendText("Conectado ao servidor\n");

                await client.ConnectAsync(IpEnd.Address, IpEnd.Port);

                STR = new StreamReader(client.GetStream());
                STW = new StreamWriter(client.GetStream());
                STW.AutoFlush = true;

                cts = new CancellationTokenSource();
                _ = Task.Run(() => ReceberMensagensAsync(cts.Token));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnDesconectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cts?.Cancel();

                STR?.Close();
                STW?.Close();
                client?.Close();

                txtBox_StatusMensagem.AppendText("Desconectado do servidor.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao desconectar" + ex.Message);
            }
        }

        private async Task ReceberMensagensAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    string mensagem = await STR.ReadLineAsync();

                    if (mensagem != null)
                    {
                        Dispatcher.Invoke(() => //Atualiza a UI
                        {
                            txtBox_Mensagem.AppendText("Guest: " + mensagem + "\n");
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    txtBox_Mensagem.AppendText("Erro ao receber mensagem!" + ex.Message);
                });
            }

            }

    }
}
