using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace WpfApp1
{
    /// <summary>
    /// Lógica interna para MultiLAN.xaml
    /// </summary>
    public partial class MultiLAN : Window
    {
        public MultiLAN()
        {
            InitializeComponent();
            OuvirMSG.Tick += new EventHandler(OuvirMSG_Tick);
            OuvirMSG.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            OuvirMSG.Start();
            StartListening();

        }

        private void OuvirMSG_Tick(object sender, EventArgs e)
        {
            //    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            //    using (UdpClient c = new UdpClient(11000))
            //        lblMSGrecebida.Content = c.Receive(ref RemoteIpEndPoint);

            lblMSGrecebida.Content = message;
            
        }

        System.Windows.Threading.DispatcherTimer OuvirMSG = new System.Windows.Threading.DispatcherTimer();



        //static void SendUdp(int srcPort, string dstIp, int dstPort, byte[] data)
        //{
        //    using (UdpClient c = new UdpClient(srcPort))
        //        c.Send(data, data.Length, dstIp, dstPort);
        //}

        private void btnEnviarMsgUDP_Click(object sender, RoutedEventArgs e)
        {
            //SendUdp(11000, "192.168.0.255", 11000, Encoding.ASCII.GetBytes(txtMensagemUDP.Text));
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 11000);
            byte[] bytes = Encoding.ASCII.GetBytes(txtMensagemUDP.Text);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 11000);

        private void btnOuvirMSG_Click(object sender, RoutedEventArgs e)
        {
            //    try {
            //        using (UdpClient c = new UdpClient(11000))
            //            lblMSGrecebida.Content = c.Receive(ref RemoteIpEndPoint);
            //    }
            //    catch(Exception exp)
            //    {
            //        MessageBox.Show(exp.ToString());
            //    }
            StartListening();
        }

        static string message = "Quaisquer mensagens recebidas serão exibidas aqui";
    
        private readonly UdpClient udp = new UdpClient(11000);
        private void StartListening()
        {
            this.udp.BeginReceive(Receive, new object());
        }
        public void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 11000);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            message = Encoding.ASCII.GetString(bytes);
            StartListening();
        }
   
    }
}
