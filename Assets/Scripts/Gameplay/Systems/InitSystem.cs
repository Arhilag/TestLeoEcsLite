using Leopotam.EcsLite;
using UnityEngine;

class InitSystem : IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        Time.timeScale = 1;
    }
}