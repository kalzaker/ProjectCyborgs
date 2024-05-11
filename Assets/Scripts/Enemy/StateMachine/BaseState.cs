using Mirror;
public abstract class BaseState : NetworkBehaviour
{
    [SyncVar]
    public Enemy enemy;

    [SyncVar]
    public StateMachine stateMachine;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        enemy = GetComponent<Enemy>();
    }

    public abstract void Enter();

    public abstract void Perform();

    public abstract void Exit();
}
