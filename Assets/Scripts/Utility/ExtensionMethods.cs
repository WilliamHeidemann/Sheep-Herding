using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

namespace Utility
{
    public static class ExtensionMethods
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
        
        public static T MinBy<T>(this IEnumerable<T> source, System.Func<T, float> selector)
        {
            T minItem = default;
            float minValue = float.MaxValue;
            foreach (var item in source)
            {
                float value = selector(item);
                if (value < minValue)
                {
                    minValue = value;
                    minItem = item;
                }
            }
            return minItem;
        }
        
        public static T MaxBy<T>(this IEnumerable<T> source, System.Func<T, float> selector)
        {
            T maxItem = default;
            float maxValue = float.MinValue;
            foreach (var item in source)
            {
                float value = selector(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxItem = item;
                }
            }
            return maxItem;
        }
    }
}