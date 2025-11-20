using System.Linq;
using Models;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Utility.ExtensionMethods;

namespace DogAi.Strategies
{
    public class ClockwiseStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Transform _man;
        private readonly DogConfiguration _dogConfiguration;

        public ClockwiseStrategy(Transform dog, Transform[] sheep, Transform man, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _sheep = sheep;
            _man = man;
            _dogConfiguration = dogConfiguration;
        }

        public void Execute()
        {
            /*
             * Define the semi-circle from the man to the balance point
             * Calculate the interpolation value based on the dog's position along that semi-circle
             * Use the interpolation value to find the target point on the semi-circle
             * Move the dog towards that target point
             */

            var spline = new Spline();
            var startingPoint = _man.position;
            var balancedPoint = _man.position.BalancedPoint(_sheep.Select(s => s.position).ToArray());
            
            Vector3 dir = (balancedPoint - startingPoint).normalized;

            float radius = Vector3.Distance(startingPoint, balancedPoint) * 0.5f;
            
            Vector3 up = Vector3.Cross(dir, Vector3.up);
            if (up.sqrMagnitude < 0.001f)
                up = Vector3.Cross(dir, Vector3.right);
            up.Normalize();

            var arcFactor = 2;
            float k = 0.5522847498f * radius * arcFactor;
            
            Vector3 tangentAtStartingPoint =  up * k;  // tangent at A
            Vector3 tangentAtBalancedPoint = -up * k;  // tangent at B

            var knotA = new BezierKnot(startingPoint, -tangentAtStartingPoint,  tangentAtStartingPoint);
            var knotB = new BezierKnot(balancedPoint,  -tangentAtBalancedPoint, tangentAtBalancedPoint);

            spline.Add(knotA);
            spline.Add(knotB);

            var segments = 100;
            for (int i = 0; i < segments; i++)
            {
                float tStart = i / (float)segments;
                float tEnd = (i + 1) / (float)segments;
                var start = spline.EvaluatePosition(tStart);
                var end = spline.EvaluatePosition(tEnd);
                Debug.DrawLine(start, end, Color.orange);
            }
            
            // Find the closest point on the spline to the dog's position
            float closestT = 0f;
            float closestDistanceSqr = float.MaxValue;
            int sampleCount = 100;
            for (int i = 0; i <= sampleCount; i++)
            {
                float t = i / (float)sampleCount;
                Vector3 pointOnSpline = spline.EvaluatePosition(t);
                float distanceSqr = (_dog.position - pointOnSpline).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestT = t;
                }
            }
            
            // Determine the target point slightly ahead on the spline
            float lookAheadT = math.clamp(closestT + 0.05f, 0f, 1f);
            Vector3 targetPoint = spline.EvaluatePosition(lookAheadT);
            _dog.MoveTowards(targetPoint, _dogConfiguration.BaseMoveSpeed, _dogConfiguration.BaseTurnSpeed);
            Debug.DrawLine(_dog.position, targetPoint, Color.black);
        }
    }
}