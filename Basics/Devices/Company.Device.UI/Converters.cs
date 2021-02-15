using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.IO.Ports;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace Company.Device.UI
{
    //
    // StopBits
    internal class StopBitsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StopBits)
            {
                switch ((StopBits)value)
                {
                    case StopBits.One:
                        return "1";
                    case StopBits.OnePointFive:
                        return "1.5";
                    case StopBits.Two:
                        return "2";
                    default:
                        break;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stopBits = value as string;
            if (stopBits == "1")
                return StopBits.One;
            else if (stopBits == "1.5")
                return StopBits.OnePointFive;
            else if (stopBits == "2")
                return StopBits.Two;
            return StopBits.One;
        }
    }

    internal class StopBitsSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<StopBits>)
            {
                string[] stopBits = new string[] { "1", "1.5", "2" };
                return new ObservableCollection<string>(stopBits);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    // Parity
    internal class ParityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Parity)
            {
                switch ((Parity)value)
                {
                    case Parity.None:
                        return "None";
                    case Parity.Odd:
                        return "Odd";
                    case Parity.Even:
                        return "Even";
                    case Parity.Mark:
                        return "Mark";
                    case Parity.Space:
                        return "Space";
                    default:
                        break;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            if (val == "None")
                return Parity.None;
            else if (val == "Odd")
                return Parity.Odd;
            else if (val == "Even")
                return Parity.Even;
            else if (val == "Mark")
                return Parity.Mark;
            else if (val == "Space")
                return Parity.Space;
            return Parity.None;
        }
    }

    internal class ParitySetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Parity>)
            {
                string[] vals = new string[] {
                    "None",
                    "Odd",
                    "Even",
                    "Mark",
                    "Space" };
                return new ObservableCollection<string>(vals);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    // Handshake
    internal class HandshakeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Handshake)
            {
                switch ((Handshake)value)
                {
                    case Handshake.None:
                        return "None";
                    case Handshake.RequestToSend:
                        return "RTS";
                    case Handshake.XOnXOff:
                        return "XonXOff";
                    case Handshake.RequestToSendXOnXOff:
                        return "RTS/XonXOff";
                    default:
                        break;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            if (val == "RTS")
                return Handshake.RequestToSend;
            else if (val == "XonXOff")
                return Handshake.XOnXOff;
            else if (val == "RTS/XonXOff")
                return Handshake.RequestToSendXOnXOff;
            return Handshake.None;
        }
    }

    internal class HandshakeSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Handshake>)
            {
                string[] vals = new string[] { "None", "RTS", "XonXOff", "RTS/XonXOff" };
                return new ObservableCollection<string>(vals);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
