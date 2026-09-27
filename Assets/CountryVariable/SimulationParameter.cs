using UnityEngine;


// CountryVariableの方程式用定数。

public class SimulationParameter
{
    public float b; //中立→支持への移行する量に関わる。信用率を用いる。
    public float r; //中立→反対への移行する量を関わる。危険度を用いる。
    public float w; //支持→中立への移行する量を関わる。飽きを表し、DebateLevel高いほど上がる。
    public float x; //反対→中立への移行する量を関わる。飽きを表し、DebateLevel高いほど上がる。
    public float y; //支持→反対への移行する量を関わる。危険度を用いる。
    public float z; //反対→支持への移行する量を関わる。信用率を用いる。
}

// 変化量
public class SimulationState
{
    public float neutral;
    public float yes;
    public float no;

    public float Reach;

}


// 計算用。
public class SimulationInstance
{
    public SimulationState state;
    public SimulationParameter parameter;

    public CountryVariable target;

    public int RemainingTurn;

    public MyAccData Poster;// MyAcc参照用

    public SimulationDelta Delta = new SimulationDelta();

}

// １変化量
public class SimulationDelta
{
    public float yes;
    public float no;
    public float neutral;
}