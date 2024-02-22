using System.Collections.Generic;
using UnityEngine;

public class PlaySys
{
    PlayerC player;
    MaskC mask;
    PlayPanel playPanel;

    Map map;
    int currentMapIndex;
    List<string> mapNameList;

    Camera mainCamera;
    CameraC cameraC;

    bool IsOver;
    public PlaySys()
    {
        mapNameList = MapManager.Instance.mapNameList;
        InitUI();
        InitCamera();
        InitMap();//Initialize map, get player and mask
    }
    #region Initialize Game
    void InitUI()
    {
        playPanel = UIManager.Instance.OpenPanel(UIConst.PlayPanel) as PlayPanel;
        playPanel.nextBtn.onClick.AddListener(OnClickNextBtn);
        playPanel.mask.SetActive(false);
    }
    void OnClickNextBtn()
    {
        currentMapIndex = (currentMapIndex + 1) % mapNameList.Count;
        GameBegain(currentMapIndex);
    }
    void InitCamera()
    {
        mainCamera = Camera.main;
        cameraC = mainCamera.GetComponent<CameraC>();
    }
    void InitMap()
    {
        currentMapIndex = Random.Range(0, mapNameList.Count);
        GameBegain(currentMapIndex);
    }
    #endregion

    #region Game Loop Logic
    void GameBegain(int mapIndex)
    {
        //Get map and start position
        map = MapManager.Instance.OnLoad(mapNameList[mapIndex],false);

        playPanel.textTMP.text = "¡¶" + map.name + "¡·";
        Vector3 startPos = map.start.pos + new Vector3(0.5f, 0.5f, 0);

        //Initialize character
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Player");
        GameObject playerObj = GameObject.Instantiate(prefab, startPos, Quaternion.identity);
        if (player != null)
        {
            GameObject.Destroy(player.gameObject);
        }
        player = playerObj.GetComponent<PlayerC>();
        mask = playerObj.GetComponent<MaskC>();
        cameraC.SetFollow(true);

        IsOver = false;
        player.SwitchWalkMode(WalkMode.Keyboard);
        mainCamera.orthographicSize = 9;
        GameManager.Instance.playerInput.CameraControl.Disable();

    }
    public void InGame()
    {
        float theta = GetAngle();
        playPanel.arrow.rotation *= Quaternion.Euler(0, 0, theta);
        //Debug.Log((player.transform.position - new Vector3(map.target.pos.x, map.target.pos.y, 0) - new Vector3(0.5f, 0.5f, 0)).magnitude) ;
        if (AchieveTarget() && !IsOver)
        {
            GameOver();
        }
    }
    #region Update Angle of Arrow
    float GetAngle()
    {
        //Get vector from player to target
        Vector3 direction = (map.target.pos + new Vector3(0.5f, 0.5f, 0) - player.transform.position).normalized;

        // Define new forward and new right
        Vector3 forward = Rotate(playPanel.arrow.up);
        Vector3 right = Rotate(playPanel.arrow.right);

        // Calculate the direction of rotation
        float rotDir = Mathf.Sign(Vector3.Dot(direction, -right));

        // Calculate the angle of rotation 
        float costheta = Mathf.Clamp(Vector3.Dot(direction, forward), -1, 1);
        float theta = Mathf.Acos(costheta) * 180 / Mathf.PI;

        return rotDir * theta;
    }
    Vector3 Rotate(Vector3 v)
    {
        return new Vector3(v.x + v.y, v.y - v.x) / Mathf.Sqrt(2);
    }
    #endregion
    public void OnFixedUpdate()
    {
        
        cameraC.CameraFollow(player.transform);
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(player.transform.position);
        mask.MaskFollow(screenPoint);
    }
    
    void GameOver()
    {
        Debug.Log("woqunimade");
        IsOver = true;
        cameraC.SetFollow(false);
        
        if (GameManager.Instance.playerData.firstTimeGame)
        {
            playPanel.mask.SetActive(true);
            return;
        }
        else
        {
            LookAround();
        }
        
    }
    public void LookAround()
    {
        GameManager.Instance.playerInput.CameraControl.Enable();
        mask.StartMaskAnim();
    }
    bool AchieveTarget()
    {
        return (player.transform.position - new Vector3(map.target.pos.x, map.target.pos.y, 0) - new Vector3(0.5f, 0.5f, 0)).magnitude < 0.8f;
    }
    #endregion
    
    
}
