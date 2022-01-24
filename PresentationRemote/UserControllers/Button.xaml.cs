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

namespace PresentationRemote.UserControllers
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {


        public Button()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Button), new UIPropertyMetadata(null));

        public String LabelText
        {
            get { return (String)GetValue(LabelTextProperty); }
            set
            {
                SetValue(LabelTextProperty, value);


            }
        }
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(String), typeof(Button), new UIPropertyMetadata(null));


        public Brush HoverColor
        {
            get { return (Brush)GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }
        public static readonly DependencyProperty HoverColorProperty =
            DependencyProperty.Register("HoverColor", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.Gray));

        public event  RoutedEventHandler? Click;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }


        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }
    }

}
