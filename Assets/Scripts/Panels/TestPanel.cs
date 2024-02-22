using UnityEngine.UI;

public class TestPanel : BasePanel
{
    Button returnBtn;
    Button aStarBtn;
    Button navMeshBtn;
    Button keyboardBtn;

    PlayerC player;
    
    private void Awake()
    {
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();

        aStarBtn = transform.Find("DownLeft/WalkMode/AStarBtn").GetComponent<Button>();
        navMeshBtn = transform.Find("DownLeft/WalkMode/NavMeshBtn").GetComponent<Button>();
        keyboardBtn = transform.Find("DownLeft/WalkMode/KeyboardBtn").GetComponent<Button>();

        
        returnBtn.onClick.AddListener(OnClickReturnBtn);

        aStarBtn.onClick.AddListener(OnClickAStarBtn);
        navMeshBtn.onClick.AddListener(OnClickNavMeshBtn);
        keyboardBtn.onClick.AddListener(OnClickKeyboardBtn);

        player = FindObjectOfType<PlayerC>().GetComponent<PlayerC>();
        
    }
    
    void OnClickReturnBtn()
    {
        GameManager.Instance.fsm.SwitchState(GameState.Create);
    }

    void OnClickAStarBtn()
    {
        player.SwitchWalkMode(WalkMode.AStar);
    }
    void OnClickNavMeshBtn()
    {
        player.SwitchWalkMode(WalkMode.NavMesh);
        
    }
    void OnClickKeyboardBtn()
    {
        player.SwitchWalkMode(WalkMode.Keyboard);
    }
}
