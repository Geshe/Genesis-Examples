using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO.Ports;
using Genesis.Device;

namespace Company.Device.UI
{
    /// <summary>
    /// SerialSessionXXDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SerialSessionXXDialog : Window, IDeviceSessionEditor
    {
        private SerialSessionXX m_deviceSession;
        
        public SerialSessionXXDialog()
        {
            InitializeComponent();
            InitializeParameters();
            //
            buttonOK.IsEnabled = false;
            m_deviceSession = null;
            //
            this.DataContext = this;
        }

        public IDeviceSession DeviceSession
        {
            get
            {
                return m_deviceSession;
            }
            set
            {
                if (value is SerialSessionXX)
                {
                    m_deviceSession = value as SerialSessionXX;
                    UpdateControls();
                }
            }
        }
        public IDeviceSessionVerifier DeviceSessionVerifier
        {
            get;
            set;
        }
        // 串口
        public ObservableCollection<string> Ports { get; private set; }
        public ObservableCollection<int> BaudRates { get; private set; }
        public ObservableCollection<int> DataBits { get; private set; }
        public ObservableCollection<StopBits> StopBits { get; private set; }
        public ObservableCollection<Parity> Parities { get; private set; }
        public ObservableCollection<Handshake> Handshakes { get; private set; }

        private void InitializeParameters()
        {
            // 串口
            string[] ports = SerialSessionXX.GetSystemPorts();
            Array.Sort(ports);
            Ports = new ObservableCollection<string>(ports);

            int[] baudRates = new int[] { 150, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 128000, 256000 };
            BaudRates = new ObservableCollection<int>(baudRates);

            int[] dataBits = new int[] { 5, 6, 7, 8 };
            DataBits = new ObservableCollection<int>(dataBits);

            StopBits[] stopBits = new StopBits[] { System.IO.Ports.StopBits.One, System.IO.Ports.StopBits.OnePointFive, System.IO.Ports.StopBits.Two };
            StopBits = new ObservableCollection<StopBits>(stopBits);

            Parity[] parities = new Parity[] { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
            Parities = new ObservableCollection<Parity>(parities);

            Handshake[] handshakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.XOnXOff, Handshake.RequestToSendXOnXOff };
            Handshakes = new ObservableCollection<Handshake>(handshakes);
        }
        private void UpdateControls()
        {
            if (m_deviceSession != null)
            {
                textEditName.Text = m_deviceSession.Name;
                textEditDescription.Text = m_deviceSession.Description;

                if (string.IsNullOrEmpty(m_deviceSession.Address))
                {
                    if (this.Ports.Count > 0)
                    {
                        comboEditPort.SelectedItem = this.Ports[0];
                    }
                }
                else
                {
                    comboEditPort.SelectedItem = m_deviceSession.Address;
                }
                comboEditBaudRate.SelectedItem = m_deviceSession.GetParameter(SerialSessionXX.PARAM_BAUDRATE);
                comboEditDataBits.SelectedItem = m_deviceSession.GetParameter(SerialSessionXX.PARAM_DATABITS);
                comboEditStopBits.SelectedItem = new StopBitsConverter().Convert(m_deviceSession.GetParameter(SerialSessionXX.PARAM_STOPBITS), null, null, null);
                comboEditParity.SelectedItem = new ParityConverter().Convert(m_deviceSession.GetParameter(SerialSessionXX.PARAM_PARITY), null, null, null);
                comboEditFlowControl.SelectedItem = new HandshakeConverter().Convert(m_deviceSession.GetParameter(SerialSessionXX.PARAM_FLOWCONTROL), null, null, null);
            }
            //
            UpdateButtonOk();
        }

        private bool CheckData()
        {
            textBlockStatus.Text = "";

            string name = textEditName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                textBlockStatus.Text = "名称不能为空。";
                return false;
            }
            if (comboEditPort.SelectedItem == null)
            {
                textBlockStatus.Text = "串口号不能为空。";
                return false;
            }
            return true;
        }
        private void UpdateButtonOk()
        {
            buttonOK.IsEnabled = CheckData();
        }

        private bool UpdateData()
        {
            m_deviceSession.Name = textEditName.Text;
            m_deviceSession.Description = textEditDescription.Text;

            m_deviceSession.Address = comboEditPort.SelectedItem.ToString();
            m_deviceSession.SetParameter(SerialSessionXX.PARAM_BAUDRATE, comboEditBaudRate.SelectedItem);
            m_deviceSession.SetParameter(SerialSessionXX.PARAM_DATABITS, comboEditDataBits.SelectedItem);
            m_deviceSession.SetParameter(SerialSessionXX.PARAM_STOPBITS, new StopBitsConverter().ConvertBack(comboEditStopBits.SelectedItem, null, null, null));
            m_deviceSession.SetParameter(SerialSessionXX.PARAM_PARITY, new ParityConverter().ConvertBack(comboEditParity.SelectedItem, null, null, null));
            m_deviceSession.SetParameter(SerialSessionXX.PARAM_FLOWCONTROL, new HandshakeConverter().ConvertBack(comboEditFlowControl.SelectedItem, null, null, null));
            return true;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateData())
            {
                this.DialogResult = true;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
             
        private void TextEditName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonOk();
        }

        private void ComboEditPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonOk();
        }
    }
}
