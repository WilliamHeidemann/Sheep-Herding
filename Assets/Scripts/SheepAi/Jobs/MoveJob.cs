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
        public float MoveSpeedModifier;
        public float TurnSpeedModifier;
        public NativeArray<Vector3> Targets;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 target = Targets[index];
            Debug.DrawLine(transform.position, transform.position + target, Color.white);

            float distance = math.length(target);
            float turnSpeed = distance * TurnSpeedModifier * DeltaTime;
            float moveSpeed = distance * MoveSpeedModifier * DeltaTime;

            Vector3 newDirection = Vector3.Slerp(transform.rotation * Vector3.forward, target.normalized, turnSpeed);

            // transform.rotation = Quaternion.LookRotation(newDirection);
            // transform.position += newDirection * moveSpeed;
            
            transform.MoveTowards(target, moveSpeed, turnSpeed);
        }
    }
}