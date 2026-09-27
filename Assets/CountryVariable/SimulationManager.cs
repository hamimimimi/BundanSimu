using UnityEngine;
using System.Collections.Generic;


public class SimulationManager : MonoBehaviour
{
    private List<SimulationInstance> simulations =
        new List<SimulationInstance> ();

    public static SimulationManager Instance;
     
    private void Awake()
    {
        Instance = this;
    }

    public void AddSimulation(
    SimulationInstance simulation)
    {
        simulations.Add(simulation);
    }

    public void StepAll()
    {
        UpdateSimulations();
        ApplyToVariables();
        UpdateDebateLevels();
        RemoveFinished();
    }


    // ターンごとの変化計算とターン進行。
    private void UpdateSimulations()
    {
        SimulationCalculator calculator =
           new SimulationCalculator();
        
        foreach (var simulation in simulations)
        {
          calculator.Step(simulation.state, simulation.parameter, simulation.Delta);
        
            simulation.RemainingTurn--;
        }
    }


   // Variableへの反映 
   private void ApplyToVariables()
    {

        Dictionary<CountryVariable, SimulationDelta> totals
            = new();


    foreach (var simulation in simulations)
     {
            CountryVariable target = simulation.target;

            if (!totals.ContainsKey(target))
            {
                totals.Add(
                    target,
                    new SimulationDelta());
            }

        SimulationDelta delta = simulation.Delta;

            totals[target].yes += delta.yes;

            totals[target].no += delta.no;

            totals[target].neutral += delta.neutral;

     }

       foreach(var pair in totals)
         {
            CountryVariable variable = pair.Key;

            SimulationDelta delta = pair.Value;

            variable.yes += Mathf.RoundToInt(delta.yes);

            variable.no += Mathf.RoundToInt(delta.no);

            variable.neutral += Mathf.RoundToInt(delta.neutral);

            int addPoint =
                   Mathf.Abs(Mathf.RoundToInt(delta.yes))
                   + Mathf.Abs(Mathf.RoundToInt(delta.no));

            variable.UpdateDebatePoint(addPoint);



            CountryVariableManager.Instance.NotifyUpdated(variable);
         }

        
    }
    private void UpdateDebateLevels()
    {
        foreach (CountryVariable variable
    in CountryVariableManager.Instance.GetAll())
        {
            if (variable.UpdateDebateLevel())
            {
                CountryVariableManager.Instance
                    .NotifyDebateLevelUp(variable);
            }
        }
    }

    // 終了したものを切除
    private void RemoveFinished()
    { 


            for(int i = simulations.Count-1 ; i >=0 ; i--)
              {
                if(simulations[i].RemainingTurn<=0)
                  {
                     simulations.RemoveAt(i);
                  }
              }
        
    }


}
