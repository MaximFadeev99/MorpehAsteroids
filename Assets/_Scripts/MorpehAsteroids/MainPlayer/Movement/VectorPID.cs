using System;
using UnityEngine;

namespace MorpehAsteroids.MainPlayer.Movement
{
    [Serializable]
    public class VectorPID
    {
        [SerializeField] private float _pFactor;
        [SerializeField] private float _iFactor;    
        [SerializeField] private float _dFactor;

        private Vector3 _currentDelta;
        private Vector3 _integral;
        private Vector3 _previousError;

        public Vector3 Update(Vector3 currentError, float timeFrame)
        {
            _integral += currentError * timeFrame;
            _currentDelta = (currentError - _previousError) / timeFrame;
            _previousError = currentError;

            return currentError * _pFactor
                + _integral * _iFactor
                + _currentDelta * _dFactor;
        }
    }
}
