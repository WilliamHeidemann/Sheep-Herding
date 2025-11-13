using System;
using Unity.Jobs;
using UnityEngine;

public class DogAi : MonoBehaviour
{
    [SerializeField] private CircleHerdingStrategy _circleHerdingStrategy;

    public IHerdingStrategy HerdingStrategy;

    private void Awake()
    {
        HerdingStrategy = _circleHerdingStrategy;
    }

    private void Update()
    {
        HerdingStrategy.Execute();
    }
}

[Serializable]
public class Pen
{
    public Transform CenterOfPen;
    public float PenRadius = 10f;
}

public interface IHerdingStrategy
{
    public void Execute();
}

[Serializable]
public class CircleHerdingStrategy : IHerdingStrategy
{
    public Transform Dog;
    public Transform[] Sheep;
    public Pen Pen;
    public float DogMoveSpeed;
    public float DogTurnSpeed;
    
    public void Execute()
    {
        var furthestDistanceToPen = float.MinValue;
        foreach (Transform transform in Sheep)
        {
            float distanceToPen = Vector3.Distance(transform.position, Pen.CenterOfPen.position);
            if (distanceToPen > furthestDistanceToPen)
            {
                furthestDistanceToPen = distanceToPen;
            }
        }
        
        var circleX = Pen.CenterOfPen.position.x + furthestDistanceToPen * Mathf.Cos(Time.time) * 1.2f; 
        var circleZ = Pen.CenterOfPen.position.z + furthestDistanceToPen * Mathf.Sin(Time.time)* 1.2f; 
        var targetPosition = new Vector3(circleX, Dog.position.y, circleZ);
        
        Debug.DrawLine(Dog.position, targetPosition, Color.black);
        
        var direction = (targetPosition - Dog.position).normalized;

        var turnSpeed = DogTurnSpeed * Time.deltaTime;
        var moveSpeed = DogMoveSpeed * Time.deltaTime;
        Dog.rotation = Quaternion.Slerp(Dog.rotation, Quaternion.LookRotation(direction), turnSpeed);
        Dog.position += direction * moveSpeed;
    }
}