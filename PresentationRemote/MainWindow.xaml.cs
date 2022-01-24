using PresentationRemote.Core;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace PresentationRemote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //
        private static readonly Int32 Port = 1111;
        private static string Server = "";
        readonly UdpClient Client = new UdpClient(Port);
        string data = "";
        private static string allRecivedTxt = "";
        //
        private readonly System.Windows.Forms.NotifyIcon appNotificationIcon = new ();
        private readonly string password;
        public MainWindow()
        {
            InitializeComponent();
            // connect app 
            Connect();
            //
            Server = GetLocalIPAddress();
            password = RandomPasswordGenerator.RandomPassword();
            passwordTB.Text = password;
            //
            server.Text = Server;
            portTB.Text = Port.ToString();
            //
            qrImage.Source = (password + "," + Server).GenerateQRImage();
      
            //
            AppNotifyIconSetup();
        }



       
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message.ToString());
                return "";
            }
         
        }

        int i = 10;
        void Connect()
        {
            try
            {
        
                Client.BeginReceive(new AsyncCallback(Recive), null);
            }
            catch (Exception ex)
            {

                allRecivedTxt += ex.Message.ToString();
            }
        }

        void Recive(IAsyncResult result)
        {
            IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, Port);
            Byte[] recived = Client.EndReceive(result, ref RemoteIP!);
            data = Encoding.UTF8.GetString(recived);

            //to avoid cross thrading we use method invoker
            //Operations.getPassword(data) == password
            if (data.GetPassword() == password)
            {
                this.Dispatcher.Invoke(new(delegate
                {
                    allRecivedTxt += "\nRecived data: " + data;
                    //msg.Text =   "Pass: "+ Operations.getPassword(data) + " Msg: "+ Operations.getMsg(data);
                    msg.Text = data.GetMsg();
                }));
                this.Dispatcher.Invoke(new(delegate
                {
                    if (data.GetMsg().ToString() == "Next")
                    {
                        SendKeys.SendWait("{DOWN}");

                    }
                    else if (data.GetMsg().ToString() == "Previous")
                    {
                        SendKeys.SendWait("{UP}");
                    }
                    else if (data.GetMsg().ToString() == "Connected")
                    {
                        MinimizeAppToNotifyIcon();
                        //this.WindowState = WindowState.Minimized;
                    }
                }));

            }
            Client.BeginReceive(new AsyncCallback(Recive), null);
            //
            i++;
            MoveCursor(i, i);
        }


        private void AppBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            MinimizeAppToNotifyIcon();
        }

        private void MinimizeAppToNotifyIcon()
        {
            this.WindowState = WindowState.Minimized;
            appNotificationIcon.Visible = true;
            appNotificationIcon.BalloonTipText = "Presentation Remote minimized here";
            appNotificationIcon.BalloonTipClicked += new System.EventHandler(this.AppNotificationIcon_DoubleClick);
            appNotificationIcon.ShowBalloonTip(1000);

            ShowInTaskbar = false;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void AppNotifyIconSetup()
        {
            appNotificationIcon.Icon = new System.Drawing.Icon("app_icon.ico");
            appNotificationIcon.Visible = false;
            appNotificationIcon.Text = "Presentation Remote";
            appNotificationIcon.DoubleClick += new System.EventHandler(this.AppNotificationIcon_DoubleClick);

        }

        private void AppNotificationIcon_DoubleClick(object? sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
            appNotificationIcon.Visible = false;
            ShowInTaskbar = true;
            this.Activate();
        }

        void ReturnAppToNoraml()
        {

            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
            appNotificationIcon.Visible = false;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            appNotificationIcon.Visible = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                //this.Hide();
                //appNotificationIcon.Visible = true;
                //appNotificationIcon.BalloonTipText = "Presentation Remote minimized here";
                //appNotificationIcon.ShowBalloonTip(1000);
                //ShowInTaskbar = false;
            }
        }
        private void MoveCursor(int x,int y)
        {
            MouseMovement.SetCursorPos(x, y);
        }
    }
}
