using UnityEngine;
using System.Linq;
using System.Collections;

// CountryVariableの微小変化計算用。式について詳しくは仕様書へ。

public class SimulationCalculator 
{

public void Step(SimulationState state, SimulationParameter parameter, SimulationDelta delta)
    {

        float S = state.neutral;
        float I = state.yes;
        float R = state.no;

        float reach = state.Reach;

        float dS =
             -parameter.b * S * I / reach
             - parameter.r * S * R / reach
             + parameter.w * I
             + parameter.x * R;

        float dI =
              parameter.b * S * I / reach
            - parameter.y * I
            + parameter.z * R
            - parameter.w * I;
  
        float dR =
              parameter.r * S * R / reach
            + parameter.y * I
            - parameter.z * R
            - parameter.x * R;

        delta.neutral = dS;
        delta.yes = dI;
        delta.no = dR;

        S += dS;
        I += dI;
        R += dR;

        state.neutral = S;
        state.yes = I;
        state.no = R;

        Debug.Log(
            $"S={state.neutral} I={state.yes} R={state.no}");
        Debug.Log(
    $"dS={dS} dI={dI} dR={dR}");

    }

}
