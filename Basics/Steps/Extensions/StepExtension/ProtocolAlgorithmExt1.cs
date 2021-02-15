using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Protocol;
using Genesis;

namespace StepExtensionDLL
{
    /* 本注释说明算法的接口和算法参数接口和参数类，用于扩展自定义算法。
     * 需要引用Plugins\Genesis\bin\Genesis.dll和Plugins\Genesis.Core\bin\Genesis.Core.dll
    /// <summary>
    /// 算法接口
    /// </summary>
    public interface IProtocolAlgorithm
    {
        /// <summary>
        /// 算法标识
        /// </summary>
        string Id { get; }
        /// <summary>
        /// 算法名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 算法描述
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 算法的参数，不同算法参数可能不同。
        /// </summary>
        IEnumerable<IProtocolAlgorithmParameter> Parameters { get; }
    
        /// <summary>
        /// 计算协议字段数值，只支持字段位串在原始数据中连续排列的情况
        /// </summary>
        /// <param name="data">原始数据位串</param>
        /// <param name="fieldStartBit">协议字段在数据位串的起始位置</param>
        /// <param name="fieldEndBit">协议字段在数据位串的结束位置</param>
        /// <param name="fieldEndian">协议字段的大小端</param>
        /// <param name="dataStartBit">返回待计算数据在位串的起始位置</param>
        /// <param name="dataEndBit">返回待计算数据在位串的结束位置</param>
        /// <returns>结果位串，大小端由fieldEndian决定，长度为fieldEndBit-fieldStartBit+1</returns>
        BitString Compute(BitString data, int fieldStartBit, int fieldEndBit, Endian fieldEndian, out int dataStartBit, out int dataEndBit);
    }
    /// <summary>
    /// 算法参数接口
    /// </summary>
    public interface IProtocolAlgorithmParameter
    {
        /// <summary>
        /// 算法参数名称，系统通过名称对参数进行赋值。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 算法参数的类型
        /// </summary>
        Type ValueType { get; }
        /// <summary>
        /// 算法参数值
        /// </summary>
        string Value { get; set; }
        /// <summary>
        /// 算法参数描述，一般用作Tooltip之类的帮助文本，可本地化
        /// </summary>
        string Description { get; }
    }
    /// <summary>
    /// 算法参数类
    /// </summary>
    public class ProtocolAlgorithmParameter : IProtocolAlgorithmParameter
    {
        public ProtocolAlgorithmParameter(string name, Type valueType, string value, string description)
        {
            this.Name = name;
            this.ValueType = valueType;
            this.Value = value;
            this.Description = description;
        }

        public string Name { get; }
        public Type ValueType { get; }
        public string Value { get; set; }
        public string Description { get; }
    }
    */

    internal class ProtocolAlgorithmExt1 : IProtocolAlgorithm
    {
        private List<IProtocolAlgorithmParameter> m_parameters;

        public ProtocolAlgorithmExt1()
        {
            m_parameters = new List<IProtocolAlgorithmParameter>();
            // 添加算法参数，参数显示在“属性面板”。
            m_parameters.Add(new ProtocolAlgorithmParameter("Param1", typeof(Int32), "1", "参数1"));
            m_parameters.Add(new ProtocolAlgorithmParameter("Param2", typeof(Int32), "2", "参数2")); 
        }
		/// <summary>
        /// 算法标识，在整个软件系统中必须唯一
        /// </summary>
        public string Id
        {
            get
            {
                return "ProtocolAlgorithmExt1";
            }
        }
        /// <summary>
        /// 算法名称
        /// </summary>
        public string Name
        {
            get
            {
                return "扩展协议算法1";
            }
        }
        /// <summary>
        /// 算法描述
        /// </summary>
        public string Description
        {
            get
            {
                return "扩展协议算法1，用于测试";
            }
        }
        /// <summary>
        /// 算法的参数，不同算法参数可能不同。
        /// </summary>
        public IEnumerable<IProtocolAlgorithmParameter> Parameters
        {
            get
            {
                return m_parameters;
            }
        }
        /// <summary>
        /// 计算协议字段数值，只支持字段位串在原始数据中连续排列的情况
        /// </summary>
        /// <param name="data">原始数据位串</param>
        /// <param name="fieldStartBit">协议字段在数据位串的起始位置</param>
        /// <param name="fieldEndBit">协议字段在数据位串的结束位置</param>
        /// <param name="fieldEndian">协议字段的大小端</param>
        /// <param name="dataStartBit">返回待计算数据在位串的起始位置</param>
        /// <param name="dataEndBit">返回待计算数据在位串的结束位置</param>
        /// <returns>结果位串，大小端由fieldEndian决定，长度为fieldEndBit-fieldStartBit+1</returns>
        public BitString Compute(BitString data, int fieldStartBit, int fieldEndBit, Endian fieldEndian, out int dataStartBit, out int dataEndBit)
        {
            // 如果需要参数计算，则可以获取配置的参数
            int param1 = Convert.ToInt32(m_parameters[0].Value);
            int param2 = Convert.ToInt32(m_parameters[1].Value);
            //
            dataStartBit = 0;
            dataEndBit = fieldStartBit - 1;
            //
            int sum = 0;
            // 获取计算的数据
            byte[] dataBytes = data.Bytes;
            int fieldStart = fieldStartBit / 8;
            if (dataBytes.Length < 6)
            {
                return BitConverter.GetBytes(sum);
            }
            // 计算
            for (int i=0; i< fieldStart; i++)
            {
                sum += dataBytes[i];
            }
            BitString result = BitConverter.GetBytes(sum); // 将int类型变为字节数组
            int resultLength = fieldEndBit - fieldStartBit + 1;
            result = GetComputeResult(result, resultLength, BitConverter.IsLittleEndian ? Endian.LittleEndian : Endian.BigEndian, fieldEndian);
            return result;
        }

        // 根据最终要返回的结果大小和大小端情况，处理结果数据
        private BitString GetComputeResult(BitString result, int resultLength, Endian fromEndian, Endian toEndian)
        {
            if (fromEndian != toEndian)
            {
                if (fromEndian == Endian.BigEndian)
                {
                    result.ReverseBytesBE();
                }
                else
                {
                    result.ReverseBytesLE();
                }
            }
            if (result.Length > resultLength)
            {
                if (toEndian == Endian.BigEndian)
                {
                    result.Remove(0, result.Length - resultLength);
                }
                else
                {
                    result.Remove(resultLength, result.Length - resultLength);
                }
            }
            else if (result.Length < resultLength)
            {
                if (toEndian == Endian.BigEndian)
                {
                    result.Insert(0, new BitString(resultLength - result.Length));
                }
                else
                {
                    result.Append(new BitString(resultLength - result.Length));
                }
            }
            return result;
        }
    }
}
