using UnityEngine.UI;

public class StartPanel : BasePanel
{
    Button createBtn;
    Button walkBtn;
    private void Awake()
    {
        createBtn = transform.Find("CreateBtn").GetComponent<Button>();
        walkBtn = transform.Find("WalkBtn").GetComponent<Button>();
        createBtn.onClick.AddListener(OnCreateBtn);
        walkBtn.onClick.AddListener(OnWalkBtn);
    }
    void OnCreateBtn()
    {
        GameManager.Instance.fsm.SwitchState(GameState.Create);
    }
    void OnWalkBtn()
    {
        GameManager.Instance.fsm.SwitchState(GameState.Walk);
    }
    
}
