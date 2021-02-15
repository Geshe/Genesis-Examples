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

namespace StepExtensionDLL
{
    /// <summary>
    /// WindowContent.xaml 的交互逻辑
    /// </summary>
    public partial class WindowContent : UserControl
    {
        public WindowContent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
            {
                textBlock.Text = ((Window)this.Parent).Title;
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
            {
                Window parent = (Window)this.Parent;
                parent.DialogResult = true;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
            {
                Window parent = (Window)this.Parent;
                parent.DialogResult = false;
            }
        }
    }
}
