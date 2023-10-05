using System;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerMovementData MovementData;
        public PlayerMeshData MeshData;

    }

    [Serializable]
    public struct PlayerMovementData
    {
        public float ForwardSpeed;
        public float SidewaysSpeed;
    }
    [Serializable]
    public struct PlayerMeshData
    {
        public float ScaleCounter;
        public float MiniGameScaleCounter;
    }

}