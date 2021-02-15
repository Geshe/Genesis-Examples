using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Native;
using Genesis.Variant;
using Genesis;

namespace Company.Variant.Analytics
{
    /// <summary>
    /// ExampleVariantAnalysisEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ExampleVariantAnalysisEditor : UserControl, IVariantAnalysisEditor, INotifyPropertyChanged
    {
        private IVariantAnalysis m_analysis;
        private ExampleVariantAnalysisObject m_object;
        private ExampleVariantAnalysisView m_view;

        private ObservableCollection<DataValue> m_allSchema;
        private ObservableCollection<DataValue> m_srcSchema;
        private ObservableCollection<DataValue> m_desSchema;
        //
        private bool m_showObjectProperties;
        private bool m_showViewProperties;

        public ExampleVariantAnalysisEditor()
        {
            InitializeComponent();
            InitializeCommands();
            //
            this.DataContext = this;
            //
            m_allSchema = new ObservableCollection<DataValue>();
            m_srcSchema = new ObservableCollection<DataValue>();
            m_desSchema = new ObservableCollection<DataValue>();

            m_showObjectProperties = true;
            m_showViewProperties = false;
        }

        #region IVariantAnalysisEditor
        public event EventHandler<VariantAnalysisErrorEventArgs> Error;

        public IVariantAnalysis Analysis
        {
            get
            {
                return m_analysis;
            }
            set
            {
                m_analysis = value;
                //
                m_object = m_analysis.Object as ExampleVariantAnalysisObject;
                m_view = m_analysis.View as ExampleVariantAnalysisView;
                //
                m_allSchema = new ObservableCollection<DataValue>();
                m_srcSchema = new ObservableCollection<DataValue>();
                m_desSchema = new ObservableCollection<DataValue>();
                if (m_object != null)
                {
                    if (m_object.DataSchema is DataValueContainer)
                    {
                        DataValueContainer srcSchema = m_object.DataSchema.Clone() as DataValueContainer;
                        foreach(DataValue v in srcSchema.Items)
                        {
                            m_allSchema.Add(v);
                            m_srcSchema.Add(v);
                        }
                    }
                    else
                    {
                        DataValue srcSchema = m_object.DataSchema.Clone() as DataValue;
                        m_allSchema.Add(srcSchema);
                        m_srcSchema.Add(srcSchema);
                    }
                    //
                    if (m_object.TableSchema != null)
                    {
                        foreach (DataValue v in m_object.TableSchema.Items)
                        {
                            AddToTableSchema(v);
                        }
                    }
                }
                //
                gridControlSrc.ItemsSource = m_srcSchema;
                gridControlDes.ItemsSource = m_desSchema;

                biShowObjectProperties.IsVisible = m_object.Settings != null;
                biShowViewProperties.IsVisible = m_view.Settings != null;

                m_showObjectProperties = m_object.Settings != null;
                m_showViewProperties = !m_showObjectProperties;
                //
                ShowProperties();
            }
        }
        
        public bool Confirm()
        {
            DataValueContainer tableSchema = new DataValueContainer();
            foreach(DataValue v in m_desSchema)
            {
                tableSchema.Add(v.Clone() as DataValue);
            }
            m_object.TableSchema = tableSchema;

            return true;
        }
        #endregion

        public bool ShowObjectProperties
        {
            get
            {
                return m_showObjectProperties;
            }
            set
            {
                m_showObjectProperties = value;
                OnPropertyChanged("ShowObjectProperties"); // 每次设置都变化一下，以便应对IsChecked的按钮重复点击问题
            }
        }
        public bool ShowViewProperties
        {
            get
            {
                return m_showViewProperties;
            }
            set
            {
                m_showViewProperties = value;
                OnPropertyChanged("ShowViewProperties"); // 每次设置都变化一下，以便应对IsChecked的按钮重复点击问题
            }
        }

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

        public ICommand SelectAllCommand { get; private set; }
        public ICommand MoveUpCommand { get; private set; }
        public ICommand MoveDownCommand { get; private set; }
        public ICommand MoveLeftCommand { get; private set; }
        public ICommand MoveRightCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public ICommand ShowObjectPropertiesCommand { get; private set; }
        public ICommand ShowViewPropertiesCommand { get; private set; }

        private void InitializeCommands()
        {
            SelectAllCommand = new DelegateCommand(SelectAll_Execute, SelectAll_CanExecute);
            MoveUpCommand = new DelegateCommand(MoveUp_Execute, MoveUp_CanExecute);
            MoveDownCommand = new DelegateCommand(MoveDown_Execute, MoveDown_CanExecute);
            MoveLeftCommand = new DelegateCommand(MoveLeft_Execute, MoveLeft_CanExecute);
            MoveRightCommand = new DelegateCommand(MoveRight_Execute, MoveRight_CanExecute);
            ClearCommand = new DelegateCommand(Clear_Execute, Clear_CanExecute);

            ShowObjectPropertiesCommand = new DelegateCommand(ShowObjectProperties_Execute, ShowObjectProperties_CanExecute);
            ShowViewPropertiesCommand = new DelegateCommand(ShowViewProperties_Execute, ShowViewProperties_CanExecute);
        }
        
        private bool SelectAll_CanExecute()
        {
            return m_srcSchema.Count > 0;
        }
        private void SelectAll_Execute()
        {
            try
            {
                m_desSchema.Clear();
                foreach (DataValue v in m_allSchema)
                {
                    m_desSchema.Add(v);
                }
                m_srcSchema.Clear();
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool MoveUp_CanExecute()
        {
            if (gridControlDes.SelectedItem == null)
            {
                return false;
            }
            DataValue v = gridControlDes.SelectedItem as DataValue;

            return m_desSchema.IndexOf(v) > 0;
        }
        private void MoveUp_Execute()
        {
            try
            {
                DataValue v = gridControlDes.SelectedItem as DataValue;

                int index = m_desSchema.IndexOf(v);
                m_desSchema.Remove(v);
                m_desSchema.Insert(index - 1, v);
                gridControlDes.SelectedItem = v;
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool MoveDown_CanExecute()
        {
            if (gridControlDes.SelectedItem == null)
            {
                return false;
            }
            DataValue v = gridControlDes.SelectedItem as DataValue;

            return m_desSchema.IndexOf(v) >= 0 && m_desSchema.IndexOf(v) < (m_desSchema.Count - 1);
        }
        private void MoveDown_Execute()
        {
            try
            {
                DataValue v = gridControlDes.SelectedItem as DataValue;
                int index = m_desSchema.IndexOf(v);
                m_desSchema.Remove(v);
                m_desSchema.Insert(index + 1, v);
                gridControlDes.SelectedItem = v;
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool MoveLeft_CanExecute()
        {
            return gridControlDes.SelectedItem != null;
        }
        private void MoveLeft_Execute()
        {
            try
            {
                DataValue v = gridControlDes.SelectedItem as DataValue;
                int index = m_desSchema.IndexOf(v);
                RemoveFromTableSchema(v);
                if (m_desSchema.Count > 0)
                {
                    if (index >= m_desSchema.Count)
                    {
                        index = m_desSchema.Count - 1;
                    }
                    gridControlDes.SelectedItem = m_desSchema[index];
                }
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool MoveRight_CanExecute()
        {
            return gridControlSrc.SelectedItem != null;
        }
        private void MoveRight_Execute()
        {
            try
            {
                DataValue v = gridControlSrc.SelectedItem as DataValue;
                int index = m_srcSchema.IndexOf(v);
                AddToTableSchema(v);
                if (m_srcSchema.Count > 0)
                {
                    if (index >= m_srcSchema.Count)
                    {
                        index = m_srcSchema.Count - 1;
                    }
                    gridControlSrc.SelectedItem = m_srcSchema[index];
                }
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool Clear_CanExecute()
        {
            return m_desSchema.Count > 0;
        }
        private void Clear_Execute()
        {
            try
            {
                ClearTableSchema();
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool ShowObjectProperties_CanExecute()
        {
            return true;
        }
        private void ShowObjectProperties_Execute()
        {
            try
            {
                this.ShowViewProperties = false;
                this.ShowObjectProperties = true;
                //
                ShowProperties();
            }
            catch (Exception e)
            {
                OnError(-1, e.Message);
            }
        }
        private bool ShowViewProperties_CanExecute()
        {
            return true;
        }
        private void ShowViewProperties_Execute()
        {
            try
            {
                this.ShowObjectProperties = false;
                this.ShowViewProperties = true;
                //
                ShowProperties();
            }
            catch(Exception e)
            {
                OnError(-1, e.Message);
            }
        }

        //
        private void AddToTableSchema(DataValue dataValue)
        {
            foreach(DataValue v in m_desSchema)
            {
                if (v.Name == dataValue.Name)
                {
                    return;
                }
            }
            foreach (DataValue v in m_allSchema)
            {
                if (v.Name == dataValue.Name)
                {
                    m_desSchema.Add(v);
                }
            }

            //
            m_srcSchema.Clear();
            foreach (DataValue v in m_allSchema)
            {
                bool existed = false;
                foreach (DataValue vv in m_desSchema)
                {
                    if (v.Name == vv.Name)
                    {
                        existed = true;
                        break;
                    }
                }
                if (!existed)
                {
                    m_srcSchema.Add(v);
                }
            }
        }
        private void RemoveFromTableSchema(DataValue dataValue)
        {
            DataValue vToRemoved = null;
            foreach (DataValue v in m_desSchema)
            {
                if (v.Name == dataValue.Name)
                {
                    vToRemoved = v;
                    break;
                }
            }
            m_desSchema.Remove(vToRemoved);
            //
            m_srcSchema.Clear();
            foreach (DataValue v in m_allSchema)
            {
                bool existed = false;
                foreach (DataValue vv in m_desSchema)
                {
                    if (v.Name == vv.Name)
                    {
                        existed = true;
                        break;
                    }
                }
                if (!existed)
                {
                    m_srcSchema.Add(v);
                }
            }
        }
        private void ClearTableSchema()
        {
            m_desSchema.Clear();
            m_srcSchema.Clear();
            foreach (DataValue v in m_allSchema)
            {
                m_srcSchema.Add(v);
            }
        }
        
        private void ShowProperties()
        {
            if (this.ShowObjectProperties)
            {
                propertyGridControl.SelectedObject = m_object.Settings;
            }
            else
            {
                propertyGridControl.SelectedObject = m_view.Settings;
            }
        }

        protected virtual void OnError(int code, string message)
        {
            if (Error != null)
            {
                Error(this, new VariantAnalysisErrorEventArgs(code, message));
            }
        }
    }
}
