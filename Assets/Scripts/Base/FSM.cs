using System.Collections.Generic;



public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnFixedUpdate();
    void OnExit();

}


public class FSM
{
    public IState currentState;
    public GameState currentStateType;
    public Dictionary<GameState, IState> stateDic;

    public FSM()
    {
        stateDic = new Dictionary<GameState, IState>();
    }

    public void AddState(GameState stateType,IState state)
    {
        if (stateDic.ContainsKey(stateType))
        {
            return;
        }
        stateDic.Add(stateType, state);
    }

    public void SwitchState(GameState stateType)
    {
        if (!stateDic.ContainsKey(stateType))
            return;

            
        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentState = stateDic[stateType];
        currentStateType = stateType;
        currentState.OnEnter();
    }
}

