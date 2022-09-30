namespace StateMachines.Enemy
{
    public class EnemyImpactState : EnemyBaseState
    {
        // Temporary value. Instead of it add new impact animation and when it is finished switch state.
        private float _duration = 1f;
        
        public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToCharacterController();
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.AIMover.ApplyForces(deltaTime);

            _duration -= deltaTime;
            if (_duration <= 0f)
            {
                StateMachine.SwitchState(new EnemySuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
