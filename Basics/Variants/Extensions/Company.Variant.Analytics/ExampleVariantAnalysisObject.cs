using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genesis.Variant;
using Genesis;

namespace Company.Variant.Analytics
{
    public class ExampleVariantAnalysisObject : VariantAnalysisObject
    {
        private DataValueContainer m_tableSchema;

        public ExampleVariantAnalysisObject()
        {
            Initialize();
        }

        public DataValueContainer TableSchema
        {
            get
            {
                return m_tableSchema;
            }
            set
            {
                if (m_tableSchema != value)
                {
                    m_tableSchema = value;
                    OnPropertyChanged("TableSchema");
                }
            }
        }
        
        #region IPersistable
        public override bool LoadState(IMemento memento)
        {
            bool result = base.LoadState(memento);
            //
            Initialize();
            //
            IMemento tableSchema = memento.GetChild("TableSchema");
            m_tableSchema = DataValue.Create(tableSchema) as DataValueContainer;

            return result;
        }

        public override bool SaveState(IMemento memento)
        {
            bool result = base.SaveState(memento);
            //
            IMemento tableSchema = memento.CreateChild("TableSchema");
            m_tableSchema.SaveState(tableSchema);

            return result;
        }
        #endregion

        private void Initialize()
        {
            m_tableSchema = null;
        }
    }
}
