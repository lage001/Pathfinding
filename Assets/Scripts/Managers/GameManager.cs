using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public enum GameState
{
    Menu,
    Walk,
    Create,
    Test,
}
public class GameManager : SingletonBase<GameManager>
{
    public FSM fsm;
    public PlayerInput playerInput;
    
    AsyncOperation asyncOperation;

    public float angle;
    public PlayerData playerData;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        playerInput = new PlayerInput();

        fsm = new FSM();
        fsm.AddState(GameState.Menu, new Menu());
        fsm.AddState(GameState.Walk, new PlayState());
        fsm.AddState(GameState.Create, new CreateState());
        fsm.AddState(GameState.Test, new TestState());
        fsm.SwitchState(GameState.Menu);

        InitPlayerData();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    private void Update()
    {
        fsm.currentState.OnUpdate();
    }
    private void FixedUpdate()
    {
        fsm.currentState.OnFixedUpdate();
    }
    public void LoadSceneAsync(string sceneName, Action<AsyncOperation> completed)
    {
        StartCoroutine(LoadScene(sceneName, completed));
    }
    IEnumerator LoadScene(string sceneName, Action<AsyncOperation> completed)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.completed += completed;
        while (asyncOperation.progress < 0.9)
        {
            yield return null;
        }
    }

    void InitPlayerData()
    {
        playerData = MyFileHandler.GetFile<PlayerData>("PlayerData.json");
    }
    public void OnSavePlayerData()
    {
        
        MyFileHandler.SaveToJSON<PlayerData>(playerData,"PlayerData.json");
    }
}
[Serializable]
public class PlayerData
{
    public bool firstTimeCreate = true;
    public bool firstTimeGame = true;

}