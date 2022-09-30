namespace StateMachines.AI
{
    public class AIImpactState : AIBaseState
    {
        // Temporary value. Instead of it add new impact animation and when it is finished switch state.
        private float _duration = 1f;
        
        public AIImpactState(AIStateMachine stateMachine) : base(stateMachine) {}
        
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
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
