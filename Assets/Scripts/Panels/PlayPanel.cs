using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayPanel : BasePanel
{
    public TextMeshProUGUI textTMP;
    Button homeBtn;
    public Button nextBtn;
    public Transform arrow;
    public GameObject mask;
    private void Awake()
    {
        textTMP = transform.Find("UpMid/Text").GetComponent<TextMeshProUGUI>();
        homeBtn = transform.Find("BottomRight/HomeBtn").GetComponent<Button>();
        nextBtn = transform.Find("BottomRight/NextBtn").GetComponent<Button>();
        arrow = transform.Find("UpRight/Arrow");

        homeBtn.onClick.AddListener(OnClickHomeBtn);
    }

    private void OnClickHomeBtn()
    {
        GameManager.Instance.fsm.SwitchState(GameState.Menu);
    }
}
