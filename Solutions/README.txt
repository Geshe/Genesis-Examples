格西测控大师 - 解决方案例子简介
========================================

文件
----------------------------------------
所有例子文件命名规则：<功能名>.<语言>.gpj。默认语言为中文，即不带语言标识的为中文版。

AutomaticTestSystem                   - 自动化测试软件演示
Database                              - 数据库操作演示

DeviceSimulation                      - 设备仿真
DeviceSimulation\DeviceSimulator      - 设备仿真演示
DeviceSimulation\WaveformGenerator    - 波形发生器演示

Instrumentation                       - 仪器仪表
Instrumentation\DAQmx                 - 使用NI的DAQmx驱动库来控制仪器进行数据采集演示
Instrumentation\Multimeter            - 数字万用表程控软件演示 - 中文（TODO）
Instrumentation\Oscilloscope          - 示波器程控软件演示 - 中文（TODO）
Instrumentation\Power                 - 可编程电源程控软件演示 - 中文（TODO）
Instrumentation\PowerAnalyzer         - 功率分析仪程控软件演示 - 中文（TODO）
Instrumentation\SignalGenerator       - 信号发生器程控软件演示 - 中文（TODO）

Network                               - 网络通信、物联网
Network\MQTT                          - MQTT协议通信演示

ProtocolConformanceTest               - 协议一致性测试
ProtocolConformanceTest\DLMS          - DLMS协议一致性测试演示
ProtocolConformanceTest\Modbus        - Modbus协议一致性测试演示

ProtocolSimulation                    - 协议仿真与分析
ProtocolSimulation\MessageInteraction - 消息交互演示
ProtocolSimulation\MessageMonitor     - 消息监视与篡改演示
ProtocolSimulation\NMEA               - 卫星导航协议NMEA-0183解析演示
ProtocolSimulation\ProtocolAnalysis   - 协议解析器演示
ProtocolSimulation\XModem             - XModem协议仿真演示

Reporting                             - 报表设计器和报表浏览器控件演示

SCADA                                 - 数据采集与监控 
SCADA\BulletinBoard                   - 电子公告牌演示
SCADA\DataExtractor                   - 从协议帧中提取数据演示
SCADA\PostureSensor                   - 三维多轴姿态角度传感器演示 
SCADA\SCADA                           - 数据采集与监控演示 
SCADA\VideoPlayer                     - 基于OpenCvSharp的视频播放器演示 

运行
----------------------------------------
所有例子自带仿真器，可以脱离设备仿真运行。

串口版：需要使用串口虚拟软件，如VSPD等，虚拟出一对串口进行仿真运行。如果虚拟的串口号和例子预定义的串口号不同，可以修改例子串口号，也可以修改虚拟串口号。
网口版：统一采用本地IP地址127.0.0.1，如果端口号被本机其他软件占用，则自行修改例子网口的端口号。