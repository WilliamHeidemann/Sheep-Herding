using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace SheepAi.Jobs
{
    public struct ForwardJob : IJobParallelForTransform
    {
        public NativeArray<Vector3> Targets;
        public float ForwardWeight;

        public void Execute(int index, TransformAccess transform)
        {
            var forward = transform.rotation * Vector3.forward * ForwardWeight;
            Debug.DrawLine(transform.position, transform.position + forward, Color.green);
            Targets[index] += forward;
        }
    }
}