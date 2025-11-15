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
            Vector3 forward = transform.rotation * Vector3.forward * ForwardWeight;

            Targets[index] += forward;
            
            Debug.DrawLine(transform.position, transform.position + forward, Color.green);
        }
    }
}