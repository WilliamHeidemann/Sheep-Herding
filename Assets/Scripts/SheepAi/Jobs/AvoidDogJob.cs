using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace SheepAi.Jobs
{
    public struct AvoidDogJob : IJobParallelForTransform
    {
        public NativeArray<Vector3> Targets;
        public float DogSeparationDistance;
        public Vector3 DogPosition;

        public void Execute(int index, TransformAccess transform)
        {
            var offset = DogPosition - transform.position;
            var distance = math.length(offset);
            if (distance > DogSeparationDistance) return;
            var separation = -offset * (1f - distance / DogSeparationDistance);

            Debug.DrawLine(transform.position, transform.position + separation, Color.darkOrange);

            Targets[index] += separation;
        }
    }
}