using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    string path;

    public event System.Action OnAfterLoad;

    public int? pendingLoadSlot = null;

    string GetPath(int slot)
{
    return Application.persistentDataPath + $"/save_{slot}.json";
}


    void Awake()
    {  
        if (Instance != null && Instance != this)
         {
            Destroy(gameObject);
           return;
          }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath + "/save.json";
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (Instance == null)
        {
            var obj = new GameObject("SaveManager");
            obj.AddComponent<SaveManager>();
        }
    }

    public void Save(int slot)
    {
        SaveData data = CreateSaveData();

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(GetPath(slot), json);

        Debug.Log("SAVE完了");
    }

    public void Load(int slot)
    {
        string path = GetPath(slot);

        if (!File.Exists(path))
        {
            Debug.Log("セーブデータなし");
            return;
        }

        string json = System.IO.File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        ApplySaveData(data);

        Debug.Log("LOAD完了");

    OnAfterLoad?.Invoke();
    }

    public void Delete(int slot)
    {
        string path = GetPath(slot);

        if (File.Exists(path))
            File.Delete(path);
    }
    

    // 保存
    SaveData CreateSaveData()
{
    SaveData data = new SaveData();

    data.saveTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");

    data.money = MoneyManager.Instance.Money;
    data.instability = GovernmentStabilityManager.Instance.Instability;
    data.currentTurn = TurnManager.Instance.CurrentTurn;

    // アイテム
    data.items = ItemInventoryManager.Instance.GetAll();

    // Variable
    data.variables = CountryVariableManager.Instance.GetAllSaveData();
    
    data.myAccs = MyAccManager.Instance.GetSaveData();
    
     data.playerPoint = PlayerInformationPointManager.Instance.CurrentPoint;

　　// Tag関係
    data.unlockProgress = UnlockProgressManager.Instance.GetSaveData();
　　data.unlockedTags = TagManager.Instance.GetSaveData();

        
    data.demo = DemoManager.Instance.GetSaveData();
        
        
        
        
        return data;
}

public SaveData LoadMeta(int slot)
{
    string path = GetPath(slot);

    if (!File.Exists(path)) return null;

    string json = File.ReadAllText(path);
    return JsonUtility.FromJson<SaveData>(json);
}

// ロード側
void ApplySaveData(SaveData data)
{
    MoneyManager.Instance.Set(data.money);
    GovernmentStabilityManager.Instance.SetInstability(data.instability);
    TurnManager.Instance.SetTurn(data.currentTurn);

    ItemInventoryManager.Instance.Load(data.items);
    CountryVariableManager.Instance.Load(data.variables);

MyAccManager.Instance.MyAccs.Clear();
    MyAccManager.Instance.Load(data.myAccs);
    PlayerInformationPointManager.Instance.Set(data.playerPoint);
    
    TagManager.Instance.Load(data.unlockedTags);
    UnlockProgressManager.Instance.Load(data.unlockProgress);

        if (data.demo != null)
        {
            DemoManager.Instance.Load(data.demo);
        }
}
}