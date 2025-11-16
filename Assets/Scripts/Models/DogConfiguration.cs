using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Models
{
    [Serializable]
    public class DogConfiguration
    {
        public float BaseMoveSpeed;
        public float BaseTurnSpeed;
    }
}