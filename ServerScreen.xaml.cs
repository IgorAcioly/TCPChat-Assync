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
    /// Inicia o servidor e permite conexão de clientes.
    
    public partial class ServerScreen : Window
    {
        private TcpListener listener;
        private TcpClient client = new TcpClient();
        public StreamReader STR;
        public StreamWriter STW;
        private CancellationTokenSource cts;

        public ServerScreen()
        {
            InitializeComponent();
        }

        private async void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, int.Parse(txtBox_PortServer.Text));
                listener.Start();
                client = await listener.AcceptTcpClientAsync();

                txtBox_StatusMensagem.AppendText("Servidor aguardando conexão com cliente.\n");

                STR = new StreamReader(client.GetStream());
                STW = new StreamWriter(client.GetStream());
                STW.AutoFlush = true;

                cts = new CancellationTokenSource();
                _ = Task.Run(() => ReceberMensagensAsync(cts.Token));

                txtBox_StatusMensagem.AppendText("Servidor iniciado e cliente conectado.\n");
            }
            catch (SocketException)
            {
                MessageBox.Show("A porta selecionada já está em uso.");
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível conectar-se." + "\n" + "Nenhum servidor encontrado com o endereço especificado");
            }
        }

        private void btnDesconectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    STR.Close();
                    STW.Close();
                    client.Close();

                    txtBox_StatusMensagem.AppendText("Cliente desconectado.\n");
                }
                if (listener != null)
                {
                    listener.Stop();
                    txtBox_StatusMensagem.AppendText("Servidor parado.\n");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao parar o servidor: ");
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
                            txtBox_StatusMensagem.AppendText("Guest: " + mensagem + "\n");
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    txtBox_StatusMensagem.AppendText("Erro ao receber mensagem." + ex.Message);
                });
            }

        }
    }
}
