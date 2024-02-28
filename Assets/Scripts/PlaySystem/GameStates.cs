using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.SceneManagement;



public class Menu:IState
{
    string name = "Menu";
    string panelName = UIConst.StartPanel;
    public void OnEnter()
    {
        UIManager.Instance.panelDict.Clear();
        GameManager.Instance.LoadSceneAsync(name,(obj)=> { UIManager.Instance.OpenPanel(panelName); });
    }
    public void OnExit()
    {
        UIManager.Instance.ClosePanel(UIConst.StartPanel);
    }
    public void OnFixedUpdate() { }

    public void OnUpdate() { }
    
}
public class PlayState : IState
{
    string name = "Play";
    public PlaySys playSys;
    bool isLoading;

    public void OnEnter()
    {
        UIManager.Instance.panelDict.Clear();
        isLoading = true;
        GameManager.Instance.LoadSceneAsync(name, InitScene);
    }
    public void OnExit()
    {
        isLoading = true;
        GameManager.Instance.playerInput.CameraControl.Disable();
    }
    public void OnUpdate()
    {
        // Wait for loading to end
        if (!isLoading)
        {
            playSys.InGame();
        }
    }
    public void OnFixedUpdate()
    {
        if (!isLoading)
        {
            playSys.OnFixedUpdate();
        }   
    }
    void InitScene(AsyncOperation obj)
    {
        playSys = new PlaySys();
        isLoading = false;
    }
}
public class CreateState : IState
{
    string name = "Create";
    string panelName = UIConst.CreatePanel;

    public void OnEnter()
    {
        if(SceneManager.GetActiveScene().name == name)
        {
            UIManager.Instance.OpenPanel(panelName);
        }
        else
        {
            UIManager.Instance.panelDict.Clear();
            GameManager.Instance.LoadSceneAsync(name, InitScene);
        }
        GameManager.Instance.playerInput.CameraControl.Enable();
    }
    public void OnExit()
    {
        UIManager.Instance.ClosePanel(UIConst.CreatePanel);
        GameManager.Instance.playerInput.CameraControl.Disable();
    }
    public void OnFixedUpdate() { }
    public void OnUpdate() { }
    void InitScene(AsyncOperation obj)
    {
        UIManager.Instance.OpenPanel(panelName);
    }
}
public class TestState : IState
{
    string panelName = UIConst.TestPanel;

    GameObject PlayerPrefab;
    GameObject AgentPrefab;
    GameObject player; 
    GameObject agent;

    CameraC cameraC;
    Map map;
    bool firstTimeEnter = true;

    NavMeshSurface navMesh;

    
    public void OnEnter()
    {
        if (firstTimeEnter)
        {
            PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            AgentPrefab = Resources.Load<GameObject>("Prefabs/Agent");
            cameraC = Object.FindObjectOfType<CameraC>().GetComponent<CameraC>();
            navMesh = GameObject.Find("Navmesh").GetComponent<NavMeshSurface>();
            firstTimeEnter = false;
        }
        
        map = MapManager.Instance.currentMap;
        
        navMesh.BuildNavMesh();
        Vector3 startPos = map.start.pos + new Vector3(0.5f, 0.5f, 0);
        cameraC.SetFollow(true);
        GameManager.Instance.playerInput.CameraControl.Disable();
        player = GameObject.Instantiate(PlayerPrefab, startPos, Quaternion.identity);
        agent = GameObject.Instantiate(AgentPrefab, startPos, Quaternion.identity);
        player.GetComponent<PlayerC>().Agent = agent.GetComponent<AIMovement>();
        UIManager.Instance.OpenPanel(panelName);

        
    }

    public void OnExit()
    {
        UIManager.Instance.ClosePanel(UIConst.TestPanel);
        GameObject.Destroy(player);
        cameraC.SetFollow(false); 
    }

    public void OnFixedUpdate()
    {
        cameraC.CameraFollow(player.transform);
    }
    
    public void OnUpdate() 
    {
        if (!map.legal && AchieveTarget())
        {
            map.legal = true;
            Functions.SetWarning("µØÍ¼Í¨¹ý²âÊÔ !!");
        }
    }
    bool AchieveTarget()
    {
        return (player.transform.position - new Vector3(map.target.pos.x, map.target.pos.y, 0) - new Vector3(0.5f, 0.5f, 0)).magnitude < 0.8f;
    }
}
