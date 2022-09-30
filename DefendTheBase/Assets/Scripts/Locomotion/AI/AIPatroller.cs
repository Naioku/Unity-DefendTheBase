using UnityEngine;

namespace Locomotion.AI
{
    public class AIPatroller : MonoBehaviour
    {
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float dwellingTime = 2f;

        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int _currentWaypointIndex;

        public Vector3 GetCurrentWaypointPosition()
        {
            return patrolPath.GetWaypointPosition(_currentWaypointIndex);
        }

        public bool AtWaypoint()
        {
            float distanceToWaypointSquared = Vector3.SqrMagnitude(transform.position - GetCurrentWaypointPosition());
            return distanceToWaypointSquared <= waypointTolerance * waypointTolerance;
        }

        public void ResetTimeSinceArrivedAtWaypoint()
        {
            _timeSinceArrivedAtWaypoint = 0f;
        }

        public void ReloadWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        public bool ShouldMoveToNextWaypoint()
        {
            return _timeSinceArrivedAtWaypoint > dwellingTime;
        }

        public void UpdateTimer()
        {
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }
    }
}
