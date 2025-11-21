using System.Linq;
using Models;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Utility.ExtensionMethods;
using static Utility.Helper;

namespace DogAi.Strategies
{
    public class ArcStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Transform _man;
        private readonly DogConfiguration _dogConfiguration;
        private readonly bool _clockwise;

        public ArcStrategy(Transform dog, Transform[] sheep, Transform man, DogConfiguration dogConfiguration, bool clockwise)
        {
            _dog = dog;
            _sheep = sheep;
            _man = man;
            _dogConfiguration = dogConfiguration;
            _clockwise = clockwise;
        }

        public void Execute()
        {
            Vector3 startingPoint = _man.position;
            Vector3 balancedPoint = _man.position.BalancedPoint(_sheep.Select(s => s.position).ToArray());
            const float arcFactor = 2f;

            
            float side = Vector3.Cross(balancedPoint - startingPoint, _dog.position - startingPoint).y;  
            
            bool isDogOnCorrectSide = _clockwise ? side < 0 : side > 0;

            if (!isDogOnCorrectSide)
            {
                (balancedPoint, startingPoint) = (startingPoint, balancedPoint);
            }
            
            Spline spline = ArcSpline(startingPoint, balancedPoint, arcFactor, _clockwise);
        
            float closestPointRatio = ClosestSplineRatio(spline, _dog.position);
            float pointAheadRatio = math.clamp(closestPointRatio + 0.05f, 0f, 1f);
            Vector3 targetPoint = spline.EvaluatePosition(pointAheadRatio);
            _dog.MoveTowards(targetPoint, _dogConfiguration.BaseMoveSpeed, _dogConfiguration.BaseTurnSpeed);
        
            Debug.DrawLine(_dog.position, targetPoint, Color.black);
            DrawSpline(spline, Color.orange);
        }
    }
}