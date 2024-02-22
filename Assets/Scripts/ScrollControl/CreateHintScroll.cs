using UnityEngine;

public class CreateHintScroll : ScrollController
{
    public RectTransform[] rectTransforms = new RectTransform[3];
    public CreatePanel createPanel;
    public void OnclickNext()
    {
        if (intPos == posnum - 1)
        {
            transform.parent.gameObject.SetActive(false);
            GameManager.Instance.playerData.firstTimeCreate = false;
            GameManager.Instance.OnSavePlayerData();
        }
        else
        {
            intPos += 1;
            if (intPos != 0)
                createPanel.BoxSelectUI(rectTransforms[(int)intPos - 1]);
        }
    }
    public void OnclickLast()
    {
        if (intPos >= 1)
        {
            intPos -= 1;
            if (intPos != 0)
                createPanel.BoxSelectUI(rectTransforms[(int)intPos - 1]);
        }
    }
}
