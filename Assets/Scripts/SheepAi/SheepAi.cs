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

        [SerializeField] private FlockingConfiguration _flockingConfiguration;
    
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
                FlockingRadius = _flockingConfiguration.SheepViewRadius,
                CohesionWeight = _flockingConfiguration.CohesionWeight,
                AlignmentWeight = _flockingConfiguration.AlignmentWeight,
                SeparationWeight = _flockingConfiguration.SeparationWeight,
                SheepSeparationDistance = _flockingConfiguration.SheepSeparationRadius
            };

            var avoidDogJob = new AvoidDogJob
            {
                Targets = _targets,
                DogSeparationDistance = _flockingConfiguration.DogViewRadius,
                DogPosition = _dog.position
            };

            var moveJob = new MoveJob
            {
                DeltaTime = Time.deltaTime,
                Targets = _targets,
                MoveSpeed = _flockingConfiguration.MoveSpeed,
                TurnSpeed = _flockingConfiguration.TurnSpeed
            };

            JobHandle flockingHandle = flockingJob.Schedule(_transformAccessArray);
            JobHandle avoidDogHandle = avoidDogJob.Schedule(_transformAccessArray, flockingHandle);
            JobHandle moveHandle = moveJob.Schedule(_transformAccessArray, avoidDogHandle);
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