using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class SheepAi : MonoBehaviour
{
    [SerializeField] private Transform[] _sheep;
    [SerializeField] private Transform _dog;
    [SerializeField] private float _forwardWeight = 1f;
    [SerializeField] private float _flockingRadius = 5f;
    [SerializeField] private float _cohesionWeight = 1f;
    [SerializeField] private float _alignmentWeight = 1f;
    [SerializeField] private float _separationWeight = 1f;
    [SerializeField] private float _dogSeparationDistance = 8f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _turnSpeed = 1f;
    
    private TransformAccessArray _transformAccessArray;
    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _directions;
    private NativeArray<Vector3> _targets;

    private void Awake()
    {
        _transformAccessArray = new TransformAccessArray(_sheep);
        _positions = new NativeArray<Vector3>(_sheep.Length, Allocator.Persistent);
        _directions = new NativeArray<Vector3>(_sheep.Length, Allocator.Persistent);
        _targets = new NativeArray<Vector3>(_sheep.Length, Allocator.Persistent);
    }

    private void Update()
    {
        for (int i = 0; i < _sheep.Length; i++)
        {
            _positions[i] = _sheep[i].position;
            _directions[i] = _sheep[i].forward;
        }

        var flockingJob = new FlockingJob
        {
            DeltaTime = Time.deltaTime,
            Positions = _positions,
            Directions = _directions,
            Targets = _targets,
            FlockingRadius = _flockingRadius,
            CohesionWeight = _cohesionWeight,
            AlignmentWeight = _alignmentWeight,
            SeparationWeight = _separationWeight,
            SheepSeparationDistance = _flockingRadius / 3f
        };

        var avoidDogJob = new AvoidDogJob
        {
            Targets = _targets,
            DogSeparationDistance = _dogSeparationDistance,
            DogPosition = _dog.position
        };

        var forwardJob = new ForwardJob
        {
            Targets = _targets,
            ForwardWeight = _forwardWeight
        };

        var moveJob = new MoveJob
        {
            DeltaTime = Time.deltaTime,
            Targets = _targets,
            MoveSpeedModifier = _moveSpeed,
            TurnSpeedModifier = _turnSpeed
        };

        JobHandle flockingHandle = flockingJob.Schedule(_transformAccessArray);
        JobHandle avoidDogHandle = avoidDogJob.Schedule(_transformAccessArray, flockingHandle);
        JobHandle forwardHandle = forwardJob.Schedule(_transformAccessArray, avoidDogHandle);
        JobHandle moveHandle = moveJob.Schedule(_transformAccessArray, forwardHandle);
        moveHandle.Complete();
    }

    private void OnDestroy()
    {
        _transformAccessArray.Dispose();
        _positions.Dispose();
        _directions.Dispose();
        _targets.Dispose();
    }
}

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

        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.position += newDirection * moveSpeed;
    }
}