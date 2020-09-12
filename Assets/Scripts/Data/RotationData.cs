using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ProgrammingBatch.Magnetize.Data
{
    [Serializable]
    public sealed class RotationData
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public float W { get; private set; }

        public RotationData(Quaternion quaternion)
        {
            X = quaternion.x;
            Y = quaternion.y;
            Z = quaternion.z;
            W = quaternion.w;
        }

        [JsonConstructor]
        public RotationData(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion CreateQuaternion()
        {
            return new Quaternion(X, Y, Z, W);
        }
    }
}