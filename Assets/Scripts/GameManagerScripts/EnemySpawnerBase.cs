using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBase : MonoBehaviour
{
    protected virtual void chunkReached(float currentX, int chunkNumber)
    {}

    protected virtual async void enemyChunkSpawner(int delay_ms, int chunkNumber, int difficulty, float currentX)
    {}

}
