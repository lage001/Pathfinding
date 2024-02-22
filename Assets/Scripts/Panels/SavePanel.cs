using UnityEngine.UI;
using TMPro;


public class SavePanel : BasePanel
{
    TMP_InputField inputField;
    Button saveBtn;
    Button cancelBtn;
    string currentMapName;
    // Start is called before the first frame update
    private void Awake()
    {
        inputField = transform.Find("Image/InputField").GetComponent<TMP_InputField>();

        saveBtn = transform.Find("Image/Buttons/SaveButton").GetComponent<Button>();
        cancelBtn = transform.Find("Image/Buttons/CancelButton").GetComponent<Button>();

        saveBtn.onClick.AddListener(OnClickSaveBtn);
        cancelBtn.onClick.AddListener(OnClickCancelBtn);

        
    }
    private void OnEnable()
    {
        InitInputField();
    }
    void InitInputField()
    {
        currentMapName = MapManager.Instance.currentMap.name;
        if (currentMapName != null)
        {
            inputField.text = currentMapName;
        }
    }
    private void OnClickCancelBtn()
    {
        ClosePanel();
    }

    private void OnClickSaveBtn()
    {
        
        if(inputField.text != "")
        {
            MapManager.Instance.OnSave(inputField.text);
            ClosePanel();
        }
        else
        {
            print("请设置一个区别于其他地图名的名字!!");
        }
    }
    void ClosePanel()
    {
        UIManager.Instance.ClosePanel(UIConst.SavePanel);
    }
}
