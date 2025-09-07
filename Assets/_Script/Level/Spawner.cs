using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Spawner
{
    [Serializable] public struct Wave
    {
        public int index;
        public GameObject enemyPrefab;
    }

    public Transform spawnPoint;
    public List<Wave> waves;

    public bool HasWave(int wave) => waves.Exists(x => x.index == wave);
}
