using UnityEngine;
using System.Linq;
using System.Collections;

// CountryVariableとMyAccからStateとParameterを作成する。

public class SimulationInitializer 
{

  private SimulationParameter parameter = new SimulationParameter();
  private  SimulationState state = new SimulationState();

    public SimulationParameter Parameter
    {
        get { return parameter; }
    }

    public SimulationState State
    {
        get { return state; }
    }



    public void Setup(MyAccData data, CountryVariable variable)
    {
        GenerateState(data, variable);

        GenerateParameter(data, variable);

    }


 // 初期値計算
 private void GenerateState(MyAccData data, CountryVariable variable)
    {



        int Infl = data.influencePoint;
        int N = variable.cap;

        int  currentTurn = TurnManager.Instance.CurrentTurn;
        
        string opinionKey = $"{variable.tag}:{variable.key}";
        OpinionCounts counts = data.GetOpinion(opinionKey);

            int a = counts.yes;
            int b = counts.no;
            int c = counts.neutral;

        int n = a + b + c;


            int A = variable.yes;
            int B = variable.no;
            int C = variable.neutral;
         Debug.Log(
$"Infl={Infl} N={N} A={A} B={B} C={C} a={a} b={b} c={c} n={n}");
        if (n == 0)
        {
            a = A;
            b = B;
            c = C;

            n = N;
        }
       
        float s = Mathf.Min(currentTurn / 200f, 1f);

        float reach = (float)Infl * N / (Infl + N);
        float S = reach * (s * (float)c / n + (1 - s) * (float)C / N);
        float I = reach * (s * (float)a / n + (1 - s) * (float)A / N);
        float R = reach * (s * (float)b / n + (1 - s) * (float)B / N);


        state.Reach = reach;
        state.neutral = S;
        state.yes = I;
        state.no = R;
    }


　　　// 定数計算
　　　private void GenerateParameter(MyAccData data, CountryVariable variable)
    {
        parameter.b = data.creditRatio / 1000f;
        parameter.r = data.RiskLevel / 1000f;
        parameter.w = variable.debateLevel / 100f;
        parameter.x = variable.debateLevel / 100f;
        parameter.y = data.RiskLevel / 2000f;
        parameter.z = data.creditRatio / 2000f;      
    }


    // Instance生成。
    public SimulationInstance CreateSimulation(
    MyAccData data,
    CountryVariable variable)
    {
        Setup(data, variable);

        SimulationInstance simulation =
            new SimulationInstance();

        simulation.state = state;
        simulation.parameter = parameter;

        simulation.target = variable;
        simulation.Poster = data;

        simulation.RemainingTurn = 10;

        return simulation;
    }
}
