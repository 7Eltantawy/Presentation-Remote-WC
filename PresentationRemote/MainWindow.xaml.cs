using PresentationRemote.Core;
using System;
using System.Diagnostics;
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
        int curMouseX = 0, curMouseY = 0;
        int mouseSpeed = 1;
        //
        private readonly System.Windows.Forms.NotifyIcon appNotificationIcon = new();
        private readonly string password;
        //
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
            //
            updateUI();
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

        void updateUI()
        {
            //
            passwordTB.Text = password;
            //
            mouseXTB.Text = curMouseX.ToString();
            mouseYTB.Text = curMouseY.ToString();
            //
            mouseSpeedTB.Text = mouseSpeed.ToString();
        }

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
        void HandleRecivedMessages(string data)
        {
            //
            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=windowsdesktop-6.0
            //
            //Handle Keys
            if (data.GetKey().ToString() == "Down")
            {
                SendKeys.SendWait("{DOWN}");
            }
            else if (data.GetKey().ToString() == "Up")
            {
                SendKeys.SendWait("{UP}");
            }
            else if (data.GetKey().ToString() == "Right")
            {
                SendKeys.SendWait("{RIGHT}");
            }
            else if(data.GetKey().ToString() == "Left")
            {
                SendKeys.SendWait("{LEFT}");
            }
            else if (data.GetKey().ToString() == "Connected")
            {
                msg.Text = "Connected";
                MinimizeAppToNotifyIcon();
                //this.WindowState = WindowState.Minimized;
            }
            else if (data.GetKey().ToString() == "Disconnected")
            {
                msg.Text = "Disconnected";
                ReturnAppToNoraml();
            }

            //Handle Mouse
            int mouseX = 0, mouseY = 0;
            int.TryParse(data.GetMouseX().ToString(), out mouseX);
            int.TryParse(data.GetMouseY().ToString(), out mouseY);
            if(mouseX != 0 || mouseY != 0)
            {
                curMouseX += mouseX * mouseSpeed;
                curMouseY += mouseY * mouseSpeed;
                MoveCursor(curMouseX, curMouseY);
            }
       
           
            if (data.GetMouseClick().ToString() == "L")
            {
                MouseMovement.LeftMouseClick();
            }
            else if (data.GetMouseClick().ToString() == "R")
            {
                MouseMovement.RightMouseClick();
            }
            //
            updateUI();
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
                    //msg.Text = data.GetKey();
                    //msg.Text = data;
                }));

                this.Dispatcher.Invoke(new(delegate
                {
                    HandleRecivedMessages(data);
                }));

                //this.Dispatcher.Invoke(new(delegate
                //{
                //    if (data.GetKey().ToString() == "Next")
                //    {
                //        SendKeys.SendWait("{DOWN}");

                //    }
                //    else if (data.GetKey().ToString() == "Previous")
                //    {
                //        SendKeys.SendWait("{UP}");
                //    }

                //    else if (data.GetKey().ToString() == "Connected")
                //    {
                //        MinimizeAppToNotifyIcon();
                //        //this.WindowState = WindowState.Minimized;
                //    }
                //}));

            }
            Client.BeginReceive(new AsyncCallback(Recive), null);
            //

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
            appNotificationIcon.Click += new System.EventHandler(this.AppNotificationIcon_DoubleClick);

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

        private void mouseSpeedTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
             int.TryParse( mouseSpeedTB.Text,out mouseSpeed);
        }

    

        private void MoveCursor(int x, int y)
        {
            MouseMovement.SetCursorPos(x, y);
        }
    }
}
