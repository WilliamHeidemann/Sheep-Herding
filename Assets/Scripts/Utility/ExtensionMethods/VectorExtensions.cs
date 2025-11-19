using UnityEngine;

namespace Utility.ExtensionMethods
{
    public static class VectorExtensions
    {
        public static Vector3 BalancedPoint(this Vector3 origin, Vector3[] sheepPositions)
        {
            if (sheepPositions.Length == 0)
            {
                return origin;
            }

            Vector3 center = Vector3.zero;
            foreach (var pos in sheepPositions)
            {
                center += pos;
            }

            center /= sheepPositions.Length;
            
            var centerOffset = center - origin;
            
            Debug.DrawLine(origin, origin + centerOffset, Color.blue);

            var balancedPoint = origin + centerOffset + centerOffset;
            
            Debug.DrawLine(origin + centerOffset, balancedPoint, Color.red);

            return balancedPoint;
        }
    }
}