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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static double orcamento = 100, aposta = 0;
        static double vel1, vel2, vel3;
        static int acertos = 0, erros = 0;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timerInicio = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer fadeOut = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            string orcsalvo = "NULL";
            try
            {
                orcsalvo = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", "NULL").ToString();
            }
            catch(Exception)
            {
               
            }
            if (orcsalvo == "NULL")
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", "100");
                orcsalvo = "100";
            }
            orcamento = int.Parse(orcsalvo);

            if(Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "B/W/G")
            {
                rdbS1.IsChecked = true;
                rdbS1_Checked(rdbS1, null);
                rtgCavalo1.Fill = System.Windows.Media.Brushes.Black;
                rtgCavalo2.Fill = System.Windows.Media.Brushes.White;
                rtgCavalo3.Fill = System.Windows.Media.Brushes.Gray;
                rdbVermelho.Content = "Afonso (preto)";
                rdbVerde.Content = "Apolo (branco)";
                rdbAzul.Content = "Catatau (cinza)";
            }
            else
            if(Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "NULL")
            {
                rdbS0.IsChecked = true;
                rdbS0_Checked(rdbS0, null);
                rtgCavalo1.Fill = System.Windows.Media.Brushes.Red;
                rtgCavalo2.Fill = System.Windows.Media.Brushes.LimeGreen;
                rtgCavalo3.Fill = System.Windows.Media.Brushes.Blue;
                rdbVermelho.Content = "Afonso (vermelho)";
                rdbVerde.Content = "Apolo (verde)";
                rdbAzul.Content = "Catatau (azul)";
            }
            else
            if(Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "R/B/W")
            {
                rdbS2.IsChecked = true;
                rdbS2_Checked(rdbS0, null);
                rtgCavalo1.Fill = System.Windows.Media.Brushes.Red;
                rtgCavalo2.Fill = System.Windows.Media.Brushes.Black;
                rtgCavalo3.Fill = System.Windows.Media.Brushes.White;
                rdbVermelho.Content = "Afonso (vermelho)";
                rdbVerde.Content = "Apolo (azul)";
                rdbAzul.Content = "Catatau (branco)";
            }
            else
            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "G/Y/B")
            {
                rdbS3.IsChecked = true;
                rdbS3_Checked(rdbS0, null);
                rtgCavalo1.Fill = System.Windows.Media.Brushes.ForestGreen;
                rtgCavalo2.Fill = System.Windows.Media.Brushes.Yellow;
                rtgCavalo3.Fill = System.Windows.Media.Brushes.Blue;
                rdbVermelho.Content = "Afonso (verde)";
                rdbVerde.Content = "Apolo (amarelo)";
                rdbAzul.Content = "Catatau (azul)";
            }
            lblOrc.Content = "Orçamento: " + orcamento.ToString(format: "c") + " LGC";
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,1);
            
            timerInicio.Tick += new EventHandler(timerInicio_Tick);
            timerInicio.Interval = new TimeSpan(0,0,0,0,3000);
            timerInicio.IsEnabled = true;

            fadeOut.Tick += new EventHandler(fadeOut_Tick);
            fadeOut.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void fadeOut_Tick(object sender, EventArgs e)
        {
            if (grdInicio.Opacity > 0)
            {
                grdInicio.Opacity -= 0.1;
            }
            else
            {
                grdInicio.Visibility = Visibility.Collapsed;
            }

        }

        private void timerInicio_Tick(object sender, EventArgs e)
        {
            fadeOut.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdMenu.Visibility = Visibility.Hidden;
            grdAposta.Visibility = Visibility.Visible;
        }

        private void btnLoja_Click(object sender, RoutedEventArgs e)
        {
            grdMenu.Visibility = Visibility.Hidden;
            grdLoja.Visibility = Visibility.Visible;
        }

        static double aux1 = 0, aux2 = 0, aux3 = 0;
        static bool anunciado = false;

        Thickness t1 = new Thickness();

        private void rdbVermelho_Checked(object sender, RoutedEventArgs e)
        {
            if (textAposta.Text == "" || textAposta.Text == " ")
            {
                btnComecar.IsEnabled = false;
            }
            else
            btnComecar.IsEnabled = true;
        }

        private void rdbVerde_Checked(object sender, RoutedEventArgs e)
        {
            if (textAposta.Text == "" || textAposta.Text == " ")
            {
                btnComecar.IsEnabled = false;
            }
            else
            btnComecar.IsEnabled = true;
        }

        private void btnMudar_Click(object sender, RoutedEventArgs e)
        {
            if (rdbS0.IsChecked == true)
            {
                rtgCavalo1.Fill = System.Windows.Media.Brushes.Red;
                rtgCavalo2.Fill = System.Windows.Media.Brushes.LimeGreen;
                rtgCavalo3.Fill = System.Windows.Media.Brushes.Blue;
                rdbVermelho.Content = "Afonso (vermelho)";
                rdbVerde.Content = "Apolo (verde)";
                rdbAzul.Content = "Catatau (azul)";
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL");
                MessageBox.Show("Skin aplicada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                rdbS0_Checked(rdbS0, null);
            }
            else
            if (rdbS1.IsChecked == true)
            {
                if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "B/W/G", "NULL")) == "true")
                {
                    rtgCavalo1.Fill = System.Windows.Media.Brushes.Black;
                    rtgCavalo2.Fill = System.Windows.Media.Brushes.White;
                    rtgCavalo3.Fill = System.Windows.Media.Brushes.Gray;
                    rdbVermelho.Content = "Afonso (preto)";
                    rdbVerde.Content = "Apolo (branco)";
                    rdbAzul.Content = "Catatau (cinza)";
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "B/W/G");
                    MessageBox.Show("Skin aplicada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    rdbS1_Checked(rdbS1, null);
                }
                else
                    MessageBox.Show("Você não possui esta skin. Por favor, compre-a e tente novamente. Não aceitamos fiado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            if (rdbS2.IsChecked == true)
            {
                if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "R/B/W", "NULL")) == "true")
                {
                    rtgCavalo1.Fill = System.Windows.Media.Brushes.Red;
                    rtgCavalo2.Fill = System.Windows.Media.Brushes.Black;
                    rtgCavalo3.Fill = System.Windows.Media.Brushes.White;
                    rdbVermelho.Content = "Afonso (vermelho)";
                    rdbVerde.Content = "Apolo (preto)";
                    rdbAzul.Content = "Catatau (branco)";
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "R/B/W");
                    MessageBox.Show("Skin aplicada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    rdbS2_Checked(rdbS2, null);
                }
                else
                    MessageBox.Show("Você não possui esta skin. Por favor, compre-a e tente novamente. Não aceitamos fiado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            if (rdbS3.IsChecked == true)
            {
                if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "G/Y/B", "NULL")) == "true")
                {
                    rtgCavalo1.Fill = System.Windows.Media.Brushes.ForestGreen;
                    rtgCavalo2.Fill = System.Windows.Media.Brushes.Yellow;
                    rtgCavalo3.Fill = System.Windows.Media.Brushes.Blue;
                    rdbVermelho.Content = "Afonso (verde)";
                    rdbVerde.Content = "Apolo (amarelo)";
                    rdbAzul.Content = "Catatau (azul)";
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "G/Y/B");
                    MessageBox.Show("Skin aplicada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    rdbS3_Checked(rdbS2, null);
                }
                else
                    MessageBox.Show("Você não possui esta skin. Por favor, compre-a e tente novamente. Não aceitamos fiado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnVoltar2_Click(object sender, RoutedEventArgs e)
        {
            grdLoja.Visibility = Visibility.Collapsed;
            grdMenu.Visibility = Visibility.Visible;
        }

        private void rdbS0_Checked(object sender, RoutedEventArgs e)
        {
            rtgPreviewR.Fill = System.Windows.Media.Brushes.Red;
            rtgPreviewG.Fill = System.Windows.Media.Brushes.LimeGreen;
            rtgPreviewB.Fill = System.Windows.Media.Brushes.Blue;
            btnComprar.IsEnabled = false;
            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "NULL")
            {
                btnMudar.IsEnabled = false;
            }
            else
            {
                btnMudar.IsEnabled = true;
            }
        }

        private void rdbS1_Checked(object sender, RoutedEventArgs e)
        {
            rtgPreviewR.Fill = System.Windows.Media.Brushes.Black;
            rtgPreviewG.Fill = System.Windows.Media.Brushes.White;
            rtgPreviewB.Fill = System.Windows.Media.Brushes.Gray;
            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "B/W/G", "NULL")) == "true")
            {
                btnComprar.IsEnabled = false;
            }
            else
            {
                btnComprar.IsEnabled = true;
            }

            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "B/W/G")
            {
                btnMudar.IsEnabled = false;
            }
            else
            {
                btnMudar.IsEnabled = true;
            }
        }

        private void rdbS2_Checked(object sender, RoutedEventArgs e)
        {
            rtgPreviewR.Fill = System.Windows.Media.Brushes.Red;
            rtgPreviewG.Fill = System.Windows.Media.Brushes.Black;
            rtgPreviewB.Fill = System.Windows.Media.Brushes.White;
            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "R/B/W", "NULL")) == "true")
            {
                btnComprar.IsEnabled = false;
            }
            else
            {
                btnComprar.IsEnabled = true;
            }

            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "R/B/W")
            {
                btnMudar.IsEnabled = false;
            }
            else
            {
                btnMudar.IsEnabled = true;
            }
        }

        private void rdbS3_Checked(object sender, RoutedEventArgs e)
        {
            rtgPreviewR.Fill = System.Windows.Media.Brushes.ForestGreen;
            rtgPreviewG.Fill = System.Windows.Media.Brushes.Yellow;
            rtgPreviewB.Fill = System.Windows.Media.Brushes.Blue;

            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "G/Y/B", "NULL")) == "true")
            {
                btnComprar.IsEnabled = false;
            }
            else
            {
                btnComprar.IsEnabled = true;
            }

            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "G/Y/B")
            {
                btnMudar.IsEnabled = false;
            }
            else
            {
                btnMudar.IsEnabled = true;
            }
        }

        private void rdbS4_Checked(object sender, RoutedEventArgs e)
        {
            rtgPreviewR.Fill = System.Windows.Media.Brushes.LimeGreen;
            rtgPreviewG.Fill = System.Windows.Media.Brushes.Blue;
            rtgPreviewB.Fill = System.Windows.Media.Brushes.Orange;
            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "G/B/O", "NULL")) == "true")
            {
                btnComprar.IsEnabled = false;
            }
            else
            {
                btnComprar.IsEnabled = true;
            }

            if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "G/B/O")
            {
                btnMudar.IsEnabled = false;
            }
            else
            {
                btnMudar.IsEnabled = true;
            }
        }

        private void btnVoltar3_Click(object sender, RoutedEventArgs e)
        {
            grdConfig.Visibility = Visibility.Collapsed;
            grdMenu.Visibility = Visibility.Visible;
        }

        private void btnResett_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Você esta certo disso?", "Resetar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "B/W/G", "false");
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "G/Y/B", "false");
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "R/B/W", "false");
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(100));
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL");
                orcamento = Convert.ToDouble(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", "NULL"));
                lblOrc.Content = "Orçamento: " + orcamento.ToString(format:"c") + " LGC";
                rdbS0.IsChecked = true;
                rdbS0_Checked(rdbS0, null);
                rtgCavalo1.Fill = System.Windows.Media.Brushes.Red;
                rtgCavalo2.Fill = System.Windows.Media.Brushes.LimeGreen;
                rtgCavalo3.Fill = System.Windows.Media.Brushes.Blue;
                rdbVermelho.Content = "Afonso (vermelho)";
                rdbVerde.Content = "Apolo (verde)";
                rdbAzul.Content = "Catatau (azul)";
            }
        }
       

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            if (passBS.Password == "senhum secretum")
            {
                gMode.Visibility = Visibility.Visible;
            }
        }

        private void rdbAzul_Checked(object sender, RoutedEventArgs e)
        {
            if (textAposta.Text == "" || textAposta.Text == " ")
            {
                btnComecar.IsEnabled = false;
            }
            else
            btnComecar.IsEnabled = true;
        }

        private void textAposta_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textAposta.Text == "" || textAposta.Text == " ")
            {
                btnComecar.IsEnabled = false;
            }
            else
                btnComecar.IsEnabled = true;
        }

        private void btnComprar_Click(object sender, RoutedEventArgs e)
        {
            if (rdbS0.IsChecked == true)
            {
                MessageBox.Show("Não é possível comprar a skin padrão. Nenhuma LyjegCoin foi debitada de sua conta.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            if (rdbS1.IsChecked == true)
            {
                if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "B/W/G", "NULL")) != "true")
                {
                    if (orcamento >= 50)
                    {
                        orcamento -= 50;
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "B/W/G");
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "B/W/G", "true");
                        lblOrc.Content = "Orçamento: " + orcamento.ToString(format: "c") + " LGC";
                        MessageBox.Show("Skin comprada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        rdbS1_Checked(null, null);
                    }
                    else
                        MessageBox.Show("Você não tem LyjegCoins suficientes para comprar esta skin.", "Fundos insuficientes!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Skin já comprada. Nenhuma LyjegCoin foi debitada de sua conta.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            if (rdbS2.IsChecked == true)
            {
                if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "R/B/W", "NULL")) != "true")
                {
                    if (orcamento >= 50)
                    {
                        orcamento -= 50;
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "R/B/W");
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "R/B/W", "true");
                        lblOrc.Content = "Orçamento: " + orcamento.ToString(format: "c") + " LGC";
                        MessageBox.Show("Skin comprada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        rdbS2_Checked(null, null);
                    }
                    else
                        MessageBox.Show("Você não tem LyjegCoins suficientes para comprar esta skin.", "Fundos insuficientes!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Skin já comprada. Nenhuma LyjegCoin foi debitada de sua conta.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            if (rdbS3.IsChecked == true)
            {
                if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "G/Y/B", "NULL")) != "true")
                {
                    if (orcamento >= 50)
                    {
                        orcamento -= 50;
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "G/Y/B");
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO\SkinsCompradas", "G/Y/B", "true");
                        lblOrc.Content = "Orçamento: " + orcamento.ToString(format: "c") + " LGC";
                        MessageBox.Show("Skin comprada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        rdbS2_Checked(null, null);
                    }
                    else
                        MessageBox.Show("Você não tem LyjegCoins suficientes para comprar esta skin.", "Fundos insuficientes!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Skin já comprada. Nenhuma LyjegCoin foi debitada de sua conta.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Configura_Click(object sender, RoutedEventArgs e)
        {
            grdMenu.Visibility = Visibility.Collapsed;
            grdConfig.Visibility = Visibility.Visible;
        }

        private void passBS_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passBS.Password != "")
            {
                btnEnviarr.IsEnabled = true;
            }
            else
            {
                btnEnviarr.IsEnabled = false;
            }
        }

        private void txtNOrc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNOrc.Text != "")
            {
                btnNOrc.IsEnabled = true;
            }
            else
            {
                btnNOrc.IsEnabled = false;
            }

        }

        private void txtNOrc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void btnNOrc_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", txtNOrc.Text);
            orcamento = Convert.ToDouble(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", "NULL"));
            lblOrc.Content = "Orçamento: " + orcamento.ToString(format: "c") + " LGC";
        }

        private void btnHistoria_Click(object sender, RoutedEventArgs e)
        {
            ModoHistória modHist = new ModoHistória();
            modHist.Show();
            this.Close();
        }

        private void btnMultiplayerLAN_Click(object sender, RoutedEventArgs e)
        {
            MultiLAN multiLAN = new MultiLAN();
            multiLAN.Show();
            this.Close();
        }

        private void textAposta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            grdAposta.Visibility = Visibility.Collapsed;
            grdMenu.Visibility = Visibility.Visible;
        }

        Thickness t2 = new Thickness();
        Thickness t3 = new Thickness();
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here 
          
            Random r = new Random();

            vel3 = (double) r.Next(50, r.Next(100, 1000))/100;
            vel2 = (double) r.Next(50, r.Next(100, 1000)) /100;
            vel1 = (double) r.Next(50, r.Next(100, 1000)) /100;

            aux1 = 0;
            aux2 = 0;
            aux3 = 0;

            t1.Top = 78;
            t2.Top = 136;
            t3.Top = 197;
            aux1 += vel1;
            t1.Left += aux1;
            aux2 += vel2;
            t2.Left += aux2;
            aux3 += vel3;
            t3.Left += aux3;
            rtgCavalo1.Margin = t1;
            rtgCavalo2.Margin = t2;
            rtgCavalo3.Margin = t3;

            if (!anunciado)
            {
                if (rtgCavalo1.Margin.Left >= 483)
                {
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "B/W/G")
                    {
                        MessageBox.Show("O cavalo preto ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "NULL")
                    {
                        MessageBox.Show("O cavalo vermelho ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "R/B/W")
                    {
                        MessageBox.Show("O cavalo vermelho ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "G/Y/B")
                    {
                        MessageBox.Show("O cavalo verde ganhou!");
                    }
                    anunciado = true;
                    dispatcherTimer.Stop();
                    rtgCavalo1.Visibility = Visibility.Collapsed;
                    rtgCavalo2.Visibility = Visibility.Collapsed;
                    rtgCavalo3.Visibility = Visibility.Collapsed;

                    if (rdbVermelho.IsChecked == true)
                    {
                        orcamento += 2 * aposta;
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                        acertos++;
                        lblOrc.Content = $"Orçamento: {orcamento.ToString(format: "c")} LGC";
                        lblAcertosErros.Content = $"Acertos: {acertos}  Erros: {erros}";
                    }
                    else
                    {
                        erros++;
                        lblAcertosErros.Content = $"Acertos: {acertos}  Erros: {erros}";
                    }
                    rdbAzul.IsEnabled = true;
                    rdbVerde.IsEnabled = true;
                    rdbVermelho.IsEnabled = true;
                    btnComecar.IsEnabled = true;
                }
                else if (rtgCavalo2.Margin.Left >= 483)
                {
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "B/W/G")
                    {
                        MessageBox.Show("O cavalo branco ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "NULL")
                    {
                        MessageBox.Show("O cavalo verde ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "R/B/W")
                    {
                        MessageBox.Show("O cavalo preto ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "G/Y/B")
                    {
                        MessageBox.Show("O cavalo amarelo ganhou!");
                    }
                    anunciado = true;
                    dispatcherTimer.Stop();
                    rtgCavalo1.Visibility = Visibility.Collapsed;
                    rtgCavalo2.Visibility = Visibility.Collapsed;
                    rtgCavalo3.Visibility = Visibility.Collapsed;

                    if (rdbVerde.IsChecked == true)
                    {
                        orcamento += 2 * aposta;
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                        acertos++;
                        lblOrc.Content = $"Orçamento: {orcamento.ToString(format: "c")} LGC";
                        lblAcertosErros.Content = $"Acertos: {acertos}  Erros: {erros}";
                    }
                    else
                    {
                        erros++;
                        lblAcertosErros.Content = $"Acertos: {acertos}  Erros: {erros}";
                    }
                    rdbAzul.IsEnabled = true;
                    rdbVerde.IsEnabled = true;
                    rdbVermelho.IsEnabled = true;
                    btnComecar.IsEnabled = true;
                }
                else if (rtgCavalo3.Margin.Left >= 483)
                {
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "B/W/G")
                    {
                        MessageBox.Show("O cavalo cinza ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "NULL")
                    {
                        MessageBox.Show("O cavalo azul ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "R/B/W")
                    {
                        MessageBox.Show("O cavalo branco ganhou!");
                    }
                    else
                    if (Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "skinativa", "NULL")) == "G/Y/B")
                    {
                        MessageBox.Show("O cavalo azul ganhou!");
                    }
                    anunciado = true;
                    dispatcherTimer.Stop();
                    rtgCavalo1.Visibility = Visibility.Collapsed;
                    rtgCavalo2.Visibility = Visibility.Collapsed;
                    rtgCavalo3.Visibility = Visibility.Collapsed;

                    if (rdbAzul.IsChecked == true)
                    {
                        orcamento += 2 * aposta;
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                        acertos++;
                        lblOrc.Content = $"Orçamento: {orcamento.ToString(format: "c")} LGC";
                        lblAcertosErros.Content = $"Acertos: {acertos}  Erros: {erros}";
                    }
                    else
                    {
                        erros++;
                        lblAcertosErros.Content = $"Acertos: {acertos}  Erros: {erros}";
                    }
                    rdbAzul.IsEnabled = true;
                    rdbVerde.IsEnabled = true;
                    rdbVermelho.IsEnabled = true;
                    btnComecar.IsEnabled = true;
                }
                
            }

        }
        private void btnComecar_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(textAposta.Text) <= orcamento)
            {
                aposta = Convert.ToDouble(textAposta.Text);
                orcamento -= aposta;
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\aPOSTACAVALO", "orcamento", Convert.ToString(orcamento));
                lblOrc.Content = $"Orçamento: {orcamento.ToString(format: "c")} LGC";
                rdbAzul.IsEnabled = false;
                rdbVerde.IsEnabled = false;
                rdbVermelho.IsEnabled = false;

                btnComecar.IsEnabled = false;

                rtgCavalo1.Visibility = Visibility.Visible;
                rtgCavalo2.Visibility = Visibility.Visible;
                rtgCavalo3.Visibility = Visibility.Visible;

                anunciado = false;
                //btnComecar.IsEnabled = false;

                t1.Top = 78;
                t1.Left = 10;
                t1.Bottom = 0;
                t1.Right = 0;
                rtgCavalo1.Margin = t1;


                t2.Top = 136;
                t2.Left = 10;
                t2.Bottom = 0;
                t2.Right = 0;
                rtgCavalo2.Margin = t2;


                t3.Top = 197;
                t3.Left = 10;
                t3.Bottom = 0;
                t3.Right = 0;
                rtgCavalo3.Margin = t3;



                dispatcherTimer.Start();
            }
            else
            {
                MessageBox.Show("LYJEG Coins insuficientes para confirmar a aposta. Por favor, insira uma quantidade válida ou compre mais LYJEG Coins na LOJA.");
            }
            
        }
        
    }

}
