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
        private List<TcpClient> clientesConectados = new List<TcpClient>();

        public ServerScreen()
        {
            InitializeComponent();
        }

        private async void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listener == null && clientesConectados != null)
                {
                    int porta = int.Parse(txtBox_PortServer.Text);
                    listener = new TcpListener(IPAddress.Any, porta);

                    // Garante que a porta possa ser reutilizada após uma desconexão
                    listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    listener.Start();

                    txtBox_StatusMensagem.AppendText("Servidor aguardando conexão com cliente.\n");

                    _ = Task.Run(() => AguardarClientesAssync());

                }
                else
                {
                    MessageBox.Show("O servidor já está em execução.\n");
                }
            }

            catch (SocketException)
            {
                MessageBox.Show("A porta selecionada já está em uso.\n");
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível conectar-se." + "\n" + "Nenhum servidor encontrado com o endereço especificado");
            }
        }

        private async void btnDesconectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;

                    lock (clientesConectados)
                    {
                        foreach (var cliente in clientesConectados)
                        {
                            try
                            {
                                cliente.GetStream().Close();
                                cliente.Close();
                            }
                            catch { }
                        }
                        clientesConectados.Clear();
                    }

                    txtBox_StatusMensagem.AppendText("Servidor desconectado.\n");

                    await Task.Delay(500); // Garante liberação da porta
                }
                else 
                {
                    MessageBox.Show("Servidor não está ativo.\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao parar o servidor"+ex);
            }
        }

        private async Task ReceberMensagensAsync(TcpClient cliente)
        {
            try
            {
                using var reader = new StreamReader(cliente.GetStream());
                while (true)
                {
                    string mensagem = await reader.ReadLineAsync();

                     if (mensagem == null) break;

                    Dispatcher.Invoke(() => //Atualiza a UI
                    {
                        txtBox_StatusMensagem.AppendText("Guest: " + mensagem + "\n");
                    });

                    EnviarParaTodos(mensagem);
                }
            }
            catch { }

            finally
            {
                lock (clientesConectados)
                {
                    clientesConectados.Remove(cliente);
                }
                cliente.Close();
                Dispatcher.Invoke(() =>
                {
                    txtBox_StatusMensagem.AppendText("Cliente desconectado. \n");
                });
            }
        }

        private async Task AguardarClientesAssync() 
        {
            while (true)
            {
                TcpClient novoCliente = await listener.AcceptTcpClientAsync();
                lock (clientesConectados)
                { 
                    clientesConectados.Add(novoCliente);
                }
                Dispatcher.Invoke(() =>
                {
                    txtBox_StatusMensagem.AppendText("Cliente conectado.\n");
                });

                _ = Task.Run(() => ReceberMensagensAsync(novoCliente));
                }            
            }

        private void EnviarParaTodos(string mensagem) 
        {
            lock (clientesConectados)
            {
                foreach (var cliente in clientesConectados)
                {
                    try
                    {
                        var writer = new StreamWriter(cliente.GetStream()) { AutoFlush = true };
                        writer.WriteLine(mensagem);
                    }
                    catch { }
                }
            
            }
        }
    }
}
