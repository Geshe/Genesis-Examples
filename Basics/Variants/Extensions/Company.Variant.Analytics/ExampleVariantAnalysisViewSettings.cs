using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genesis.Variant;
using Genesis;

namespace Company.Variant.Analytics
{
    public class ExampleVariantAnalysisViewSettings : IVariantAnalysisViewSettings
    {
        private bool m_showHorizontalLines;
        private bool m_showVerticalLines;
        private bool m_showEvenRowBackground;

        public ExampleVariantAnalysisViewSettings()
        {
            Initialize();
        }

        public bool ShowHorizontalLines
        {
            get
            {
                return m_showHorizontalLines;
            }
            set
            {
                if (m_showHorizontalLines != value)
                {
                    m_showHorizontalLines = value;
                    OnPropertyChanged("ShowHorizontalLines");
                }
            }
        }
        public bool ShowVerticalLines
        {
            get
            {
                return m_showVerticalLines;
            }
            set
            {
                if (m_showVerticalLines != value)
                {
                    m_showVerticalLines = value;
                    OnPropertyChanged("ShowVerticalLines");
                }
            }
        }
        public bool ShowEvenRowBackground
        {
            get
            {
                return m_showEvenRowBackground;
            }
            set
            {
                if (m_showEvenRowBackground != value)
                {
                    m_showEvenRowBackground = value;
                    OnPropertyChanged("ShowEvenRowBackground");
                }
            }
        }

        #region IPersistable
        public bool LoadState(IMemento memento)
        {
            Initialize();
            //
            if (memento.Contain("ShowHorizontalLines"))
            {
                m_showHorizontalLines = memento.GetBoolean("ShowHorizontalLines");
            }
            if (memento.Contain("ShowVerticalLines"))
            {
                m_showVerticalLines = memento.GetBoolean("ShowVerticalLines");
            }
            if (memento.Contain("ShowEvenRowBackground"))
            {
                m_showEvenRowBackground = memento.GetBoolean("ShowEvenRowBackground");
            }
            
            return true;
        }

        public bool SaveState(IMemento memento)
        {
            if (m_showHorizontalLines != false)
            {
                memento.PutBoolean("ShowHorizontalLines", m_showHorizontalLines);
            }
            if (m_showVerticalLines != false)
            {
                memento.PutBoolean("ShowVerticalLines", m_showVerticalLines);
            }
            if (m_showEvenRowBackground != false)
            {
                memento.PutBoolean("ShowEvenRowBackground", m_showEvenRowBackground);
            }

            return true;
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        private void Initialize()
        {
            m_showHorizontalLines = false;
            m_showVerticalLines = false;
            m_showEvenRowBackground = false;
        }
    }
}
