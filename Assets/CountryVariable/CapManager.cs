using UnityEngine;

public class CapManager : MonoBehaviour
{
  public static CapManager Instance;

    public int InitialCap {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeCaps(CountryConstantData data)
    {
       InitialCap =
            Mathf.RoundToInt(data.population * 0.05f);

    }
    public void AddCap(CountryVariable variable, int amount)
    {
        variable.AddCap(amount);

        CountryVariableManager.Instance.NotifyUpdated(variable);
    }

    public void AddCapToAll(int amount)
    {
        InitialCap += amount;

        foreach (CountryVariable variable
            in CountryVariableManager.Instance.GetAll())
        {
            AddCap(variable, amount);
        }
    }
}
