using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections.Generic;



// MyAcc作成用決定ボタン。


public class DecisionButtonController : MonoBehaviour
{
    [Header("Inputs (TextMeshPro)")]
    public TMP_InputField inputName;
    public TMP_InputField inputNote;      // 使うなら Inspector で割り当て。不要なら null のまま可.
    public List<Toggle> optionToggles;

    [Header("References")]
    public UIController uiController;     // Inspector で UIController を割り当て

    // ボタンの OnClick にこれを割り当てる
    public void OnSaveButtonPressed()
    {
        if (inputName == null)
        {
            Debug.LogError("DecisionButtonController: inputName がアサインされていません");
            return;
        }

        string name = inputName.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("名前を入力してください");
            return;
        }

            StatusTagType selectedStatus = StatusTagType.InfluencePoint;

            foreach (Toggle toggle in optionToggles)
                {

            if (!toggle.isOn)
                continue;

            StatusToggle statusToggle =
                toggle.GetComponent<StatusToggle>();
         

            
            if (statusToggle != null)
            {
               selectedStatus = statusToggle.statusType;
            }


            break;
        }



        string note = (inputNote != null) ? inputNote.text.Trim() : "";

        // 保存処理
        

        MyAccData newAcc = new MyAccData(name, note);

        newAcc.AddStatusLevel(selectedStatus, 1);

        MyAccManager.Instance.AddMyAcc(newAcc); 

        SaveManager.Instance.Save(0);

        // 保存後に一覧を更新
        if (uiController != null)
        {
            uiController?.RefreshListUI();
        }
        else
        {
            Debug.LogWarning("UIController がアサインされていません。RefreshListUI を呼べません。");
        }

        // 入力をクリア
        inputName.text = "";
        optionToggles.ForEach(t => t.isOn = false);
        if (inputNote != null) inputNote.text = "";
    }
    
}
