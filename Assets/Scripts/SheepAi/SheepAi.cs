using SheepAi.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace SheepAi
{
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
}