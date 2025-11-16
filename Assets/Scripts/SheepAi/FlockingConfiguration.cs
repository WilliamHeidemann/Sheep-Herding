using UnityEngine;

namespace SheepAi
{
    [CreateAssetMenu]
    public class FlockingConfiguration : ScriptableObject
    {
        [Header("View Radii")]
        public float SheepViewRadius;
        public float DogViewRadius;
        public float SheepSeparationRadius;
        public float ObstacleSeparationRadius;
        
        [Header("Weights")]
        public float CohesionWeight;
        public float AlignmentWeight;
        public float SeparationWeight;

        [Header("Speed")]
        public float MoveSpeed;
        public float TurnSpeed;
    }
}