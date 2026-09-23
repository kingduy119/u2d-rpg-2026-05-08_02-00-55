
public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public abstract class State : IState
{
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
