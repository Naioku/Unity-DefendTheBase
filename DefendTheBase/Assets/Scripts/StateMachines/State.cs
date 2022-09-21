namespace StateMachines
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTim);
        public abstract void Exit();
    }
}
