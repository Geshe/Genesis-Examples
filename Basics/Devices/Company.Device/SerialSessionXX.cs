using System;
using System.Collections.Generic;
using System.IO.Ports;
using Genesis.Device;

namespace Company.Device
{
    public class SerialSessionXX : MessageDeviceSession
    {
        public const string DEVICE_TYPE = "CompanySerialXX";

        public const string PARAM_PORTS = "Ports"; // 系统可用的串口名称
        public const string PARAM_BAUDRATE = "Baud";
        public const string PARAM_BREAKSTATE = "BreakState";
        public const string PARAM_CTSSTATE = "CtsState";
        public const string PARAM_DATABITS = "DataBits";
        public const string PARAM_DCDSTATE = "DcdState";
        public const string PARAM_DSRSTATE = "DsrState";
        public const string PARAM_DTRSTATE = "DtrState";
        public const string PARAM_ENDIN = "EndIn";
        public const string PARAM_ENDOUT = "EndOut";
        public const string PARAM_FLOWCONTROL = "FlowControl";
        public const string PARAM_PARITY = "Parity";
        public const string PARAM_REPLACECHAR = "ReplaceChar";
        public const string PARAM_RTSSTATE = "RtsState";
        public const string PARAM_STOPBITS = "StopBits";
        
        private SerialPort m_serialPort;
        private byte[] m_serialBuffer;

        public SerialSessionXX() : this(null, null, null)
        {
            
        }
        public SerialSessionXX(string name) : this(name, null, null)
        {

        }
        public SerialSessionXX(string name, string address, string parameters) : base(name, address, parameters)
        {
            this.Type = DEVICE_TYPE;
            //
            m_serialPort = new SerialPort();
            m_serialBuffer = new byte[2 * 1024];
            // 初始化串口
            m_serialPort.ReadTimeout = 5 * 1000;
            m_serialPort.WriteTimeout = 5 * 1000;
            m_serialPort.WriteBufferSize = 16 * 1024; 
            m_serialPort.ReadBufferSize = 16 * 1024;
            m_serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            m_serialPort.PinChanged += new SerialPinChangedEventHandler(SerialPort_PinChanged);
            m_serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
        }


        public override object GetParameter(string name)
        {
            switch (name)
            {
                case PARAM_PORTS:
                    return GetSystemPorts();
                case PARAM_BAUDRATE:
                    return m_serialPort.BaudRate;
                case PARAM_BREAKSTATE:
                    return m_serialPort.BreakState;
                case PARAM_CTSSTATE:
                    return m_serialPort.CtsHolding;
                case PARAM_DATABITS:
                    return m_serialPort.DataBits;
                case PARAM_DCDSTATE:
                    return m_serialPort.CDHolding;
                case PARAM_DSRSTATE:
                    return m_serialPort.DsrHolding;
                case PARAM_DTRSTATE:
                    return m_serialPort.DtrEnable;
                case PARAM_ENDIN:
                    break;
                case PARAM_ENDOUT:
                    break;
                case PARAM_FLOWCONTROL:
                    return m_serialPort.Handshake;
                case PARAM_PARITY:
                    return m_serialPort.Parity;
                case PARAM_REPLACECHAR:
                    return m_serialPort.ParityReplace;
                case PARAM_RTSSTATE:
                    return m_serialPort.RtsEnable;
                case PARAM_STOPBITS:
                    return m_serialPort.StopBits;
                default:
                    break;
            }

            //
            return base.GetParameter(name);
        }
        
        public override bool SetParameter(string name, object val)
        {
            // 设置具体设备参数
            switch(name)
            {
                case PARAM_PORTS:
                    return false;
                case PARAM_BAUDRATE:
                    m_serialPort.BaudRate = Convert.ToInt32(val);
                    break;
                case PARAM_BREAKSTATE:
                    m_serialPort.BreakState = Convert.ToBoolean(val);
                    break;
                case PARAM_CTSSTATE:
                    return false;
                case PARAM_DATABITS:
                    m_serialPort.DataBits = Convert.ToInt32(val);
                    break;
                case PARAM_DCDSTATE:
                    return false;
                case PARAM_DSRSTATE:
                    return false;
                case PARAM_DTRSTATE:
                    m_serialPort.DtrEnable = Convert.ToBoolean(val);
                    break;
                case PARAM_ENDIN:
                    return false;
                case PARAM_ENDOUT:
                    return false;
                case PARAM_FLOWCONTROL:
                    m_serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), val.ToString());
                    break;
                case PARAM_PARITY:
                    m_serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), val.ToString());
                    break;
                case PARAM_REPLACECHAR:
                    m_serialPort.ParityReplace = Convert.ToByte(val);
                    break;
                case PARAM_RTSSTATE:
                    m_serialPort.RtsEnable = Convert.ToBoolean(val);
                    break;
                case PARAM_STOPBITS:
                    m_serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), val.ToString());
                    break;
                default:
                    break;
            }
            //
            return base.SetParameter(name, val);
        }

        public override bool Open()
        {
            if (m_serialPort.IsOpen == false)
            {
                m_serialPort.PortName = this.Address;
                m_serialPort.Open();
            }
            this.State = m_serialPort.IsOpen;
            return this.State;
        }

        public override void Close()
        {
            if (m_serialPort.IsOpen)
            {
                m_serialPort.Close();
            }
            this.State = m_serialPort.IsOpen;
            //
            this.Flush();
        }

        public static string[] GetSystemPorts()
        {
            List<string> listPorts = new List<string>();

            foreach (string p in SerialPort.GetPortNames())
            {
                int splitPoint = p.IndexOf('\0');
                if (splitPoint > 0)
                {
                    listPorts.Add(p.Substring(0, splitPoint));
                }
                else
                {
                    listPorts.Add(p);
                }
            }
            return listPorts.ToArray();
        }
        //
        // Protected
        //
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_serialPort.Dispose();
            }
            //
            base.Dispose(disposing);
        }
        protected override void WriteDevice(int count)
        {
            if (m_serialPort.IsOpen)
            {
                int wLen = 0;
                wLen = m_serialPort.WriteBufferSize - m_serialPort.BytesToWrite;
                wLen = (wLen > count) ? count : wLen;
                if (wLen > 0)
                {
                    byte[] data = new byte[wLen];
                    int actualLen = GetSendData(data, 0, data.Length);
                    m_serialPort.Write(data, 0, actualLen);
                }
            }
            else
            {
                GetSendData(new byte[128], 0, 128); // 清空发送缓存，发现发送缓冲有数据，基类定时器会触发继续执行WriteDevice
            }
        }

        protected override void FlushDevice()
        {
            if (m_serialPort.IsOpen)
            {
                m_serialPort.DiscardInBuffer();
                m_serialPort.DiscardOutBuffer();
            }
        }
         
        //
        //
        //
        private void SerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            int eventType = -1;
            switch(e.EventType)
            {
                case SerialPinChange.Break:
                    eventType = (int)SerialSessionXXEventType.Break;
                    break;
                case SerialPinChange.CDChanged:
                    eventType = (int)SerialSessionXXEventType.DataCarrierDetect;
                    break;
                case SerialPinChange.CtsChanged:
                    eventType = (int)SerialSessionXXEventType.ClearToSend;
                    break;
                case SerialPinChange.DsrChanged:
                    eventType = (int)SerialSessionXXEventType.DataSetReady;
                    break;
                case SerialPinChange.Ring:
                    eventType = (int)SerialSessionXXEventType.RingIndicator;
                    break;
                default:
                    break;
            }
            OnEvent(new DeviceSessionEventArgs(eventType));
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            int code = DeviceStatus.ErrorSystemError;
            switch (e.EventType)
            {
                case SerialError.Frame:
                    code = DeviceStatus.ErrorSerialFraming;
                    break;
                case SerialError.Overrun:
                case SerialError.RXOver:
                case SerialError.TXFull:
                    code = DeviceStatus.ErrorSerialOverrun;
                    break;
                case SerialError.RXParity:
                    code = DeviceStatus.ErrorSerialParity;
                    break;
            }
            OnError(new DeviceSessionErrorEventArgs(code, e.EventType.ToString()));
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int len;
            if (e.EventType == SerialData.Chars)
            {
                try
                {
                    len = m_serialPort.Read(m_serialBuffer, 0, m_serialBuffer.Length);
                    if(SetRecvData(m_serialBuffer, 0, len) < len)
                    {
                        throw new OverflowException("SerialSessionXX's receive buffer is full.");
                    }
                    OnEvent(new DeviceSessionEventArgs((int)SerialSessionXXEventType.DataReceived));
                }
                catch (Exception ex)
                {
                    OnError(new DeviceSessionErrorEventArgs(DeviceStatus.ErrorSystemError, ex.Message));
                }
            }
        }
    }
}
