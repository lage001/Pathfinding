using System;

public class GameEndScroll : ScrollController
{
    public Action AfterFirstGame;
    public void OnclickNext()
    {
        if (intPos == posnum - 1)
        {
            transform.parent.gameObject.SetActive(false);
            GameManager.Instance.playerData.firstTimeGame = false;
            GameManager.Instance.OnSavePlayerData();
            PlayState play = GameManager.Instance.fsm.stateDic[GameState.Walk] as PlayState;
            play.playSys.LookAround();
        }
        else
        {
            intPos += 1;
        }
    }
    public void OnclickLast()
    {
        if (intPos >= 1)
        {
            intPos -= 1;
        }
    }

}
