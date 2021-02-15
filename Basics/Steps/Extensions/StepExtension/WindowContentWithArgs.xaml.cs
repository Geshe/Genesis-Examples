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
    public class WindowContentArgs
    {
        public int Arg1 { get; set; }
        public string Arg2 { get; set; }
    }

    /// <summary>
    /// WindowContentWithArgs.xaml 的交互逻辑
    /// </summary>
    public partial class WindowContentWithArgs : UserControl
    {
        WindowContentArgs m_args = null;
        public WindowContentWithArgs(object args) // 参数传入
        {
            InitializeComponent();
            //
            m_args = args as WindowContentArgs;
            if (m_args != null)
            {
                textBoxArg1.Text = m_args.Arg1.ToString();
                textBoxArg2.Text = m_args.Arg2;
                textBlock.Text = string.Format("初始值：Arg1={0} Arg2={1}", m_args.Arg1, m_args.Arg2);
            }
        }
        
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (!Int32.TryParse(textBoxArg1.Text, out int result))
            {
                MessageBox.Show("Arg1输入不正确，Arg1是整数类型。");
                return;
            }
            if (m_args != null) // 参数输出
            {
                m_args.Arg1 = Convert.ToInt32(textBoxArg1.Text);
                m_args.Arg2 = textBoxArg2.Text;
            }
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
