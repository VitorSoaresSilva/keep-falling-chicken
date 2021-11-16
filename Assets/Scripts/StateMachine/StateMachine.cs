using UnityEngine;
using Utilities;

public class StateMachine : Singleton<StateMachine>
{
    private BaseState currentState;

    [SerializeField] 
    private UIRoot ui;

    public UIRoot UI => ui;

    private void Start()
    {
        ChangeState(new MenuState());
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(BaseState newState)
    {
        currentState?.DestroyState();

        currentState = newState;

        if (currentState != null)
        {
            currentState.owner = this;
            currentState.PrepareState();
        }
    }
}
