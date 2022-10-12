using UnityEngine;

namespace Utils
{
    public static class Helpers
    {
        public static bool IsInMaxAndMinRangeOfTarget(Vector3 startingPosition, Vector3 targetPosition, float maxRange, float minRange)
        {
            return IsInMaxRangeOfTarget(startingPosition, targetPosition, maxRange) && 
                   IsInMinRangeOfTarget(startingPosition, targetPosition, minRange);
        }

        public static bool IsInMaxRangeOfTarget(Vector3 startingPosition, Vector3 targetPosition, float range)
        {
            return (targetPosition - startingPosition).sqrMagnitude
                   <= Mathf.Pow(range, 2);
        }

        public static bool IsInMinRangeOfTarget(Vector3 startingPosition, Vector3 targetPosition, float range)
        {
            return (targetPosition - startingPosition).sqrMagnitude
                   >= Mathf.Pow(range, 2);
        }
    }
}
