using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Utility;

namespace SheepAi.Jobs
{
    public struct MoveJob : IJobParallelForTransform
    {
        public float DeltaTime;
        public float MoveSpeed;
        public float TurnSpeed;
        public NativeArray<Vector3> Targets;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 target = Targets[index];

            float distance = math.length(target);
            float turnSpeed = distance * TurnSpeed * DeltaTime;
            float moveSpeed = distance * MoveSpeed * DeltaTime;
            
            transform.MoveTowards(target, moveSpeed, turnSpeed);
            
            Debug.DrawLine(transform.position, transform.position + target, Color.white);
        }
    }
}