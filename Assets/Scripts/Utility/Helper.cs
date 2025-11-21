using UnityEngine;
using UnityEngine.Splines;

namespace Utility
{
    public static class Helper
    {
        public static Spline ArcSpline(Vector3 start, Vector3 end, float arcFactor, bool clockwise)
        {
            Vector3 dir = (end - start).normalized;

            float radius = Vector3.Distance(start, end) * 0.5f;

            Vector3 sidewaysDirection = Vector3.Cross(dir, clockwise ? Vector3.up : Vector3.down).normalized;

            float k = 0.5522847498f * radius * arcFactor;
            
            Vector3 tangentAtStartingPoint = sidewaysDirection * k; // tangent at A
            Vector3 tangentAtBalancedPoint = -sidewaysDirection * k; // tangent at B

            var knotA = new BezierKnot(start, -tangentAtStartingPoint, tangentAtStartingPoint);
            var knotB = new BezierKnot(end, -tangentAtBalancedPoint, tangentAtBalancedPoint);

            return new Spline { knotA, knotB };
        }
        
        public static void DrawSpline(Spline spline, Color color, int segments = 100)
        {
            for (int i = 0; i < segments; i++)
            {
                float tStart = i / (float)segments;
                float tEnd = (i + 1) / (float)segments;
                var start = spline.EvaluatePosition(tStart);
                var end = spline.EvaluatePosition(tEnd);
                Debug.DrawLine(start, end, color);
            }
        }
        
        public static float ClosestSplineRatio(Spline spline, Vector3 position, int sampleCount = 100)
        {
            float closestT = 0f;
            float closestDistanceSqr = float.MaxValue;
            for (int i = 0; i <= sampleCount; i++)
            {
                float t = i / (float)sampleCount;
                Vector3 pointOnSpline = spline.EvaluatePosition(t);
                float distanceSqr = (position - pointOnSpline).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestT = t;
                }
            }

            return closestT;
        }
    }

}