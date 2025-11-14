using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace SheepAi.Jobs
{
    public struct FlockingJob : IJobParallelForTransform
    {
        public float DeltaTime;
        [ReadOnly] public NativeArray<Vector3> Positions;
        [ReadOnly] public NativeArray<Vector3> Directions;
        public NativeArray<Vector3> Targets;
        public float FlockingRadius;
        public float CohesionWeight;
        public float AlignmentWeight;
        public float SeparationWeight;
        public float SheepSeparationDistance;

        public void Execute(int index, TransformAccess transform)
        {
            int count = Positions.Length;

            Vector3 position = transform.position;

            Vector3 cohesion = new();
            Vector3 alignment = new();
            Vector3 separation = new();

            for (int i = 0; i < count; i++)
            {
                var offset = Positions[i] - position;
                var direction = Directions[i];
                var distance = math.length(offset);

                // var isInFront = math.dot(direction, offset) >= -0.2f;
                // if (!isInFront) continue; 

                if (FlockingRadius < distance) continue;

                cohesion += offset;
                alignment += direction;

                if (SheepSeparationDistance < distance) continue;

                var separationOffset = -offset;
                separation += separationOffset * (2f - distance / SheepSeparationDistance); // Range [1-2]
            }

            cohesion /= count;
            alignment /= count;

            cohesion *= CohesionWeight;
            alignment *= AlignmentWeight;
            separation *= SeparationWeight;

            Vector3 target = cohesion + alignment + separation;
            Targets[index] = target;

            Debug.DrawLine(position, position + cohesion, Color.yellow);
            Debug.DrawLine(position, position + alignment, Color.blue);
            Debug.DrawLine(position, position + separation, Color.purple);
        }
    }
}