<?xml version="1.0" encoding="utf-8"?>
<StepExecutionScheme FileVersion="1.0" Project="966E14C9-3C49-4E48-965E-844910F3BAA1">
  <Name>电机运动方案</Name>
  <Author>格西科技</Author>
  <Version>1.0</Version>
  <Description>电机运动方案描述</Description>
  <Cases>
    <Case Active="False" Step="测试用例集/电机运动-定点运动" Editor="自动测试电机定点运动界面">
      <Name>电机运动-定点运动1</Name>
      <Description>三轴运动到指定的X、Y、Z坐标点。</Description>
      <Parameter>{"X":1.0,"Y":1.0,"Z":1.0}</Parameter>
    </Case>
    <Case Active="False" Step="测试用例集/电机运动-定点运动" Editor="自动测试电机定点运动界面">
      <Name>电机运动-定点运动2</Name>
      <Description>三轴运动到指定的X、Y、Z坐标点。</Description>
      <Parameter>{"X":2.0,"Y":2.0,"Z":2.0}</Parameter>
    </Case>
    <Case Active="False" Step="测试用例集/电机运动-平面扫描" Editor="自动测试电机平面扫描界面">
      <Name>电机运动-XY平面扫描</Name>
      <Description>XY步进蛇形扫描，Y优先。</Description>
      <Parameter>{"MoveSurfaceType":0,"MoveSurfaceStart1":0.0,"MoveSurfaceEnd1":30.0,"MoveSurfaceStart2":0.0,"MoveSurfaceEnd2":30.0,"MoveSurfaceLineSpace":2.0,"MoveSurfaceStepLength":2.0,"MoveSurfaceStepInterval":0.1}</Parameter>
    </Case>
    <Case Active="False" Step="测试用例集/电机运动-平面扫描" Editor="自动测试电机平面扫描界面">
      <Name>电机运动-YZ平面扫描</Name>
      <Description>YZ连续蛇形平面扫描，Z优先。</Description>
      <Parameter>{"MoveSurfaceType":10,"MoveSurfaceStart1":0.0,"MoveSurfaceEnd1":20.0,"MoveSurfaceStart2":0.0,"MoveSurfaceEnd2":20.0,"MoveSurfaceLineSpace":2.0,"MoveSurfaceStepLength":2.0,"MoveSurfaceStepInterval":0.5}</Parameter>
    </Case>
    <Case Active="False" Step="测试用例集/电机运动-曲线运动" Editor="自动测试电机曲线运动界面">
      <Name>电机运动-XY平面圆曲线运动</Name>
      <Description />
      <Parameter>{"MoveCurveExpX":"30+30*Cos(3.14*T/180)","MoveCurveExpY":"30+30*Sin(3.14*T/180)","MoveCurveExpZ":"0","MoveCurveStepInterval":0.1,"MoveCurveStartT":0.0,"MoveCurveEndT":360.0,"MoveCurveStepT":1.0}</Parameter>
    </Case>
    <Case Active="True" Step="测试用例集/电机运动-定点运动采集数据" Editor="自动测试电机定点运动采集数据界面">
      <Name>电机运动-定点运动采集数据</Name>
      <Description>三轴运动到指定的X、Y、Z坐标点，并执行采集步骤。</Description>
      <Parameter>{"X":100.0,"Y":100.0,"Z":100.0,"ActionStep":"\u91C7\u96C6\u6570\u636E\u7528\u4F8B\u96C6/\u91C7\u96C6\u7535\u673A\u72B6\u6001"}</Parameter>
    </Case>
    <Case Active="False" Step="测试用例集/电机运动-平面扫描采集数据" Editor="自动测试电机平面扫描采集数据界面">
      <Name>电机运动-平面扫描采集数据</Name>
      <Description>任意两维的蛇形平面扫描运动，每一个点执行一次采集步骤。</Description>
      <Parameter>{"MoveSurfaceType":0,"MoveSurfaceStart1":0.0,"MoveSurfaceEnd1":20.0,"MoveSurfaceStart2":0.0,"MoveSurfaceEnd2":20.0,"MoveSurfaceLineSpace":2.0,"MoveSurfaceStepLength":2.0,"MoveSurfaceStepInterval":0.5,"ActionStep":null}</Parameter>
    </Case>
    <Case Active="False" Step="测试用例集/电机运动-曲线运动采集数据" Editor="自动测试电机曲线运动采集数据界面">
      <Name>电机运动-曲线运动采集数据</Name>
      <Description>任意三维曲线运动，每一个点执行一次采集步骤。</Description>
      <Parameter>{"MoveCurveExpX":"30+30*Cos(3.14*T/180)","MoveCurveExpY":"30+30*Sin(3.14*T/180)","MoveCurveExpZ":"0","MoveCurveStepInterval":0.5,"MoveCurveStartT":0.0,"MoveCurveEndT":360.0,"MoveCurveStepT":1.0,"ActionStep":null}</Parameter>
    </Case>
  </Cases>
</StepExecutionScheme>