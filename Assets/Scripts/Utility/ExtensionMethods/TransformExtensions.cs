using UnityEngine;
using UnityEngine.Jobs;

namespace Utility.ExtensionMethods
{
    public static class TransformExtensions
    {
        public static void MoveTowards(this Transform transform, Vector3 target, float moveSpeed,
            float turnSpeed)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        
        public static void MoveTowards(this TransformAccess transform, Vector3 target, float moveSpeed,
            float turnSpeed)
        {
            Vector3 newDirection = Vector3.Slerp(transform.rotation * Vector3.forward, target.normalized, turnSpeed);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.position += newDirection * moveSpeed;
        }
        
        public static void RotateTowards(this Transform transform, Quaternion target, float turnSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, turnSpeed * Time.deltaTime);
        }
        
        public static void RotateTowards(this Transform transform, Vector3 target, float turnSpeed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target);
            transform.RotateTowards(targetRotation, turnSpeed);
        }
    }
}