using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors.Settings;
using Genesis.Windows;
using Genesis.Utils;
using Genesis.Variant;
using Genesis;

namespace Company.Variant.Analytics
{
    /// <summary>
    /// ExampleVariantAnalysisView.xaml 的交互逻辑
    /// </summary>
    public partial class ExampleVariantAnalysisView : UserControl, IVariantAnalysisView
    {
        private ExampleVariantAnalysisObject m_object;

        private DataTable m_data;
        private int m_latestIndex;
        //
        private bool m_autoScroll;
        private string m_statusFormat; // 状态栏格式：第 1000 行 共 1000 行
        private string m_status;
        //
        // 数据获取线程
        private Thread m_refreshWorker;
        private bool m_refreshWorkerRunning;
        private bool m_refreshing;
        private int m_refreshFactor; // 更新条目数量因子
        private bool m_refreshDataSchema; // 当为true时，更新数据架构
        private bool m_clearData; // 当为true时，清除数据

        public ExampleVariantAnalysisView()
        {
            InitializeComponent();
            //
            m_object = null;
            m_data = null;
            m_latestIndex = 0;
            m_autoScroll = true;

            m_status = string.Empty;
            m_statusFormat = "第 {0} 行  共 {1} 行";
            //
            m_refreshFactor = 1;
            m_refreshDataSchema = true;
            m_refreshWorkerRunning = false;
            m_refreshing = true;
            m_refreshWorker = null;
            m_clearData = false;
            //
            this.Settings = new ExampleVariantAnalysisViewSettings();
            this.DataContext = this;
            this.Loaded += ExampleVariantAnalysisView_Loaded;
        }

        public IVariantAnalysisObject Object
        {
            get
            {
                return m_object;
            }
            set
            {
                if (m_object != value)
                {
                    m_object = value as ExampleVariantAnalysisObject;
                }
                if (m_object != null)
                {
                    m_object.PropertyChanged += Object_PropertyChanged;
                }
            }
        }

        public IVariantAnalysisViewSettings Settings
        {
            get;
            private set;
        }
        
        public string Status
        {
            get
            {
                return m_status;
            }
        }

        public void Clear()
        {
            m_clearData = true;
        }

        #region IPersistable
        public bool LoadState(IMemento memento)
        {
            IMemento settings = memento.GetChild("Settings");
            if (settings != null)
            {
                try
                {
                    Type type = System.Type.GetType(settings.GetString("Class"));
                    this.Settings = System.Activator.CreateInstance(type) as IVariantAnalysisViewSettings;

                    this.Settings.LoadState(settings);
                }
                catch { }
            }
            return true;
        }

        public bool SaveState(IMemento memento)
        {
            if (this.Settings != null)
            {
                IMemento settings = memento.CreateChild("Settings");
                settings.PutString("Class", this.Settings.GetType().FullName); // 内部用于反序列化

                this.Settings.SaveState(settings);
            }
            return true;
        }
        #endregion

        #region IDisposable
        private bool m_disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    //  释放托管状态(托管对象)。
                    if (m_refreshWorker != null)
                    {
                        m_refreshWorkerRunning = false;
                        m_refreshWorker = null;
                    }
                    m_object.PropertyChanged -= Object_PropertyChanged;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                m_disposed = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~WorkbenchPart() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
        
        private void ExportToTXT_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = m_data != null;
        }
        private void ExportToTXT_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Owner = Application.Current.MainWindow;
            dlg.CheckFileExists = true;
            dlg.Filter = "文本文件|*.txt";
            dlg.Title = "导出记录";
            if (dlg.ShowDialog() == true)
            {
                gridControlView.ExportToText(dlg.FileName);
            }
        }
        private void ExportToCSV_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = false;
        }
        private void ExportToCSV_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
        }
        private void ExportToXLSX_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = false;
        }
        private void ExportToXLSX_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
        }
        private void ExportToPDF_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = false;
        }
        private void ExportToPDF_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
        }
        private void ExportToRTF_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = false;
        }
        private void ExportToRTF_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
        }
        private void ExportToDOCX_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = false;
        }
        private void ExportToDOCX_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
        }
        private void ExportToTDM_CanExecute(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = false;
        }
        private void ExportToTDM_Execute(object sender, ExecutedRoutedEventArgs eventArgs)
        {
        }


        private void RunRefreshThread()
        {
            try
            {
                while (m_refreshWorkerRunning)
                {
                    RefreshData();
                    //
                    Thread.Sleep(100);
                }
            }
            catch { }
        }
        private void RefreshData()
        {
            if (m_object == null || m_refreshing == false)
            {
                return;
            }
            if (m_data == null || m_refreshDataSchema)
            {
                if (m_object.TableSchema == null)
                {
                    if (m_object.DataSchema is DataValueContainer)
                    {
                        m_object.TableSchema = m_object.DataSchema.Clone() as DataValueContainer;
                    }
                    else
                    {
                        m_object.TableSchema = new DataValueContainer();
                        ((DataValueContainer)m_object.TableSchema).Add(m_object.DataSchema.Clone() as DataValue);
                    }
                }
                m_data = CreateDataTable(m_object.TableSchema);
                m_latestIndex = 0;
                m_refreshDataSchema = false;
            }
            //
            // 处理新增结果条目
            this.Dispatcher.Invoke(new System.Action(delegate
            {
                gridControl.BeginDataUpdate();
            }));
            //
            const int DATA_REFRESH_COUNT = 1000;
            IEnumerable<DataValue> latestData = m_object.Take(m_latestIndex, DATA_REFRESH_COUNT * m_refreshFactor);
            if (latestData.Count() == (DATA_REFRESH_COUNT * m_refreshFactor))
            {
                if (m_refreshFactor < 64) // 最大16
                {
                    m_refreshFactor *= 2;
                }
            }
            else
            {
                if (m_refreshFactor >= 2)
                {
                    m_refreshFactor /= 2;
                }
            }
            if (latestData.Count() > 0)
            {
                FillDataTable(latestData);
                //
                m_latestIndex += latestData.Count();
            }
            if (m_clearData)
            {
                ClearDataTable();
                //
                m_clearData = false;
            }
            //
            this.Dispatcher.Invoke(new System.Action(delegate
            {
                gridControl.EndDataUpdate();
                if (m_autoScroll && latestData.Count() > 0)
                {
                    gridControlView.ScrollIntoView(gridControl.GetRowHandleByListIndex(m_data.Rows.Count - 1));
                    RefreshStatus();
                }
            }));
        }
        
        private DataTable CreateDataTable(DataValue schema)
        {
            DataTable dt = new DataTable(schema.Name);

            this.Dispatcher.Invoke(new System.Action(delegate
            {
                gridControl.Columns.Clear();
                if (schema is DataValueContainer)
                {
                    foreach (DataValue v in ((DataValueContainer)schema).Items)
                    {
                        Type columnType = GetValueType(v.Value);
                        dt.Columns.Add(v.Name, columnType);
                        gridControl.Columns.Add(new GridColumn() { FieldName = v.Name, Header = v.Name, EditSettings = GetEditSettings(columnType, v.Format) });
                    }
                }
                else
                {
                    Type columnType = GetValueType(schema.Value);
                    dt.Columns.Add(schema.Name, columnType);
                    gridControl.Columns.Add(new GridColumn() { FieldName = schema.Name, Header = schema.Name, EditSettings = GetEditSettings(columnType, schema.Format) });
                }
                //
                gridControl.ItemsSource = dt;
                gridControl.InvalidateVisual();
            }));

            return dt;
        }

        private Type GetValueType(object value)
        {
            return value != null ? value.GetType() : typeof(string);
        }

        private BaseEditSettings GetEditSettings(Type type, string displayFormat)
        {
            BaseEditSettings settings = null;
            switch (type.Name)
            {
                case "Boolean":
                case "SByte":
                case "Byte":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                case "Int64":
                case "UInt64":
                case "Float":
                case "Single":
                case "Double":
                case "Decimal":
                case "String":
                case "BitString":
                    settings = new TextEditSettings();
                    break;
                case "DateTime":
                    settings = new DateEditSettings();
                    break;
                default:
                    settings = new TextEditSettings();
                    break;
            }
            settings.DisplayFormat = displayFormat;

            return settings;
        }

        private void FillDataTable(IEnumerable<DataValue> data)
        {
            m_data.BeginLoadData();
            //
            foreach (DataValue v in data)
            {
                if (v != null)
                {
                    object[] rowValues = ExtractTableData(m_object.TableSchema, m_object.DataSchema, v);
                    if (rowValues.Length > 0)
                    {
                        m_data.Rows.Add(rowValues);
                    }
                }
            }
            // 处理容量限制
            if (m_data.Rows.Count > m_object.Capacity)
            {
                int removeCount = m_data.Rows.Count - m_object.Capacity;
                while (removeCount-- > 0)
                {
                    m_data.Rows.RemoveAt(0);
                }
            }
            //
            m_data.EndLoadData();
            m_data.AcceptChanges();
        }

        private object[] ExtractTableData(DataValueContainer tableSchema, DataValue dataSchema, DataValue data)
        {
            List<object> rowValues = new List<object>();
            foreach(DataValue v in tableSchema.Items)
            {
                if(dataSchema is DataValueContainer)
                {
                    for(int i=0; i< ((DataValueContainer)dataSchema).Items.Count(); i++)
                    {
                        if (((DataValueContainer)dataSchema).Items.ElementAt(i).Name == v.Name)
                        {
                            rowValues.Add(((DataValueContainer)data).Items.ElementAt(i).Value);
                        }
                    }
                }
                else
                {
                    if (dataSchema.Name == v.Name)
                    {
                        rowValues.Add(data.Value);
                    }
                }
            }
            return rowValues.ToArray();
        }

        private void ClearDataTable()
        {
            m_data.BeginLoadData();
            m_data.Clear();
            m_data.EndLoadData();
            m_data.AcceptChanges();
            //
            m_latestIndex = 0;
        }
        
        private void RefreshStatus()
        {
            // 状态更新
            int currentIndex = 0;
            int[] selectedRows = gridControl.GetSelectedRowHandles();
            if (selectedRows.Length > 0)
            {
                currentIndex = gridControl.GetListIndexByRowHandle(selectedRows[0]) + 1;
            }
            int rowCount = m_data != null ? m_data.Rows.Count : 0;
            m_status = string.Format(m_statusFormat, currentIndex, rowCount);
        }
        
        private void ExampleVariantAnalysisView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ExampleVariantAnalysisView_Loaded;
            //
            m_refreshWorkerRunning = true;
            m_refreshWorker = new Thread(new ThreadStart(RunRefreshThread));
            m_refreshWorker.Priority = ThreadPriority.BelowNormal;
            m_refreshWorker.Start();
        }

        private void Object_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TableSchema")
            {
                m_refreshDataSchema = true;
            }
        }

        private void GridControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            RefreshStatus();
        }
    }
}
