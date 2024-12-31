using System.Collections.Generic;
using FixedPoint.SubTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FixedPoint.Samples.MovementComparison
{
    public class MovementComparisonManager : MonoBehaviour
    {
        [Header("Configurations")]
        [SerializeField, Range(1000, 100000)] private int maxAmountOfSteps;
        [SerializeField, Range(1, 500)] private int iterationsEachUpdate;
        [SerializeField, Range(1, 100)] private float stepsSpeed;
        
        [Header("Scene References")] 
        [SerializeField] private GameObject fixedPointCube;
        
        [SerializeField] private GameObject floatingPointCube;

        private readonly Stack<Vector3Fp> _fixedPointsStack = new();
        private Vector3Fp _currentFixedCubePosition;
        
        private readonly Stack<Vector3> _floatingPointsStack = new();
        private Vector3 _currentFloatCubePosition;
        
        private PlayState _playState = PlayState.Stopped;

        private void Start()
        {
            _currentFixedCubePosition = new Vector3Fp(fixedPointCube.transform.position);
            _currentFloatCubePosition = floatingPointCube.transform.position;
        }

        private void FixedUpdate()
        {
            if (_playState == PlayState.Stopped)
                return;

            for (int i = 0; i < iterationsEachUpdate; i++)
            {
                if (_playState == PlayState.Playing)
                {
                    Vector3 randomMoveFloat = Random.onUnitSphere * (Time.fixedDeltaTime * stepsSpeed);
                    
                    AddMovementToFloatCube(randomMoveFloat);
                    AddMovementToFixedCube(randomMoveFloat);

                    if (_fixedPointsStack.Count >= maxAmountOfSteps)
                    {
                        _playState = PlayState.Stopped;
                        return;
                    }
                }
                else// if (_playState == PlayState.Rewinding)
                {
                    RemoveMovementFromFloatCube();
                    RemoveMovementFromFixedCube();

                    if (_fixedPointsStack.Count == 0)
                    {
                        _playState = PlayState.Stopped;
                        return;
                    }
                }
            }
            
        }

        private void AddMovementToFloatCube(Vector3 movement)
        {
            _currentFloatCubePosition += movement;
            Vector3 newPosition = _currentFloatCubePosition;
            floatingPointCube.transform.position = newPosition;
            
            _floatingPointsStack.Push(movement);
        }

        private void RemoveMovementFromFloatCube()
        {
            Vector3 movement = _floatingPointsStack.Pop();
            _currentFloatCubePosition -= movement;
            
            floatingPointCube.transform.position = _currentFloatCubePosition;
        }
        
        private void AddMovementToFixedCube(Vector3 movement)
        {
            Vector3Fp randomMoveFixed = new Vector3Fp(movement);
            _currentFixedCubePosition += randomMoveFixed;
            fixedPointCube.transform.position = (Vector3)_currentFixedCubePosition;
            
            _fixedPointsStack.Push(randomMoveFixed);
        }
        
        private void RemoveMovementFromFixedCube()
        {
            Vector3Fp randomMoveFixed = _fixedPointsStack.Pop();
            _currentFixedCubePosition -= randomMoveFixed;
            Vector3 newPosition = (Vector3)_currentFixedCubePosition;
            
            fixedPointCube.transform.position = newPosition;
        }
        
        public void Play()
        {
            if (_fixedPointsStack.Count >= maxAmountOfSteps)
                return;
            
            _playState = PlayState.Playing;
        }

        public void Rewind()
        {
            if (_fixedPointsStack.Count == 0)
                return;
            
            _playState = PlayState.Rewinding;
        }
        
        private enum PlayState
        {
            Stopped,
            Playing,
            Rewinding
        }
    }
}
