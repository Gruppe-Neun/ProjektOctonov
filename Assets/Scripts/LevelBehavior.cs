using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class LevelBehavior : MonoBehaviour
{
    [SerializeField] SpawnerBehavior spawnPoint;

    private struct Wave {
        public Wave(int[] amount, int[] level, bool[] randomSpawn = null, float[] startTime = null, float[] cooldownTime = null) {
            this.amount = amount;
            this.level = level;
            if (randomSpawn != null) this.randomSpawn = randomSpawn;
            else this.randomSpawn = null;
            if (startTime != null) this.startTime = startTime;
            else this.startTime = null;
            if (cooldownTime != null) this.cooldownTime = cooldownTime;
            else this.cooldownTime = null;
        }


        public int[] amount;
        public int[] level;
        public bool[] randomSpawn;
        public float[] startTime;
        public float[] cooldownTime;
    }

    private SpawnerBehavior[] spawners;
    private SpawnerBehavior[] heavySpawners;
    private SpawnerBehavior[] flyingSpawners;
    private SpawnerBehavior[] playerTargetSpawner;

    private Wave[] waves;
    private float[] between;  //> 0 : start new Wave after time; -1 wait for all Enemies to be killed
    private int activeWaveNum = 0;
    private Wave activeWave;
    private bool running = false;
    private bool waveEnd = false;

    private float[] nextSpawn;
    private int[] toSpawn;
    private float waveTime = 0f;
    private List<Enemy> alive;

    public void Awake() {
        alive = new List<Enemy>();
    }

    public void loadLevelData(string levelName, FirstPersonController player, GameObject mainTarget) {
        Objects.SpawnPoint[] all = Objects.loadSpawn(levelName);
        int s = -1, h = -1, f = -1, p = -1;
        foreach(Objects.SpawnPoint spawn in all) {
            switch (spawn.type) {
                case 0:
                    s++;
                    break;
                case 1:
                    h++;
                    break;

                case 2:
                    f++;
                    break;

                case 3:
                    p++;
                    break;
            }
        }
        spawners = new SpawnerBehavior[s+1];
        heavySpawners = new SpawnerBehavior[h+1];
        flyingSpawners = new SpawnerBehavior[f+1];
        playerTargetSpawner = new SpawnerBehavior[p + 1];

        foreach (Objects.SpawnPoint spawn in all) {
            switch (spawn.type) {
                case 0:
                    spawners[s] = Instantiate(spawnPoint, new Vector3(spawn.pos[0], spawn.pos[1], spawn.pos[2]), new Quaternion());
                    spawners[s].target = mainTarget;
                    spawners[s].currentLevel = this;
                    s--;
                    break;
                case 1:
                    spawners[h] = Instantiate(spawnPoint, new Vector3(spawn.pos[0], spawn.pos[1], spawn.pos[2]), new Quaternion());
                    spawners[h].target = mainTarget;
                    spawners[h].currentLevel = this;
                    h--;
                    break;

                case 2:
                    spawners[f] = Instantiate(spawnPoint, new Vector3(spawn.pos[0], spawn.pos[1], spawn.pos[2]), new Quaternion());
                    spawners[f].target = mainTarget;
                    spawners[f].currentLevel = this;
                    f--;
                    break;

                case 3:
                    spawners[p] = Instantiate(spawnPoint, new Vector3(spawn.pos[0], spawn.pos[1], spawn.pos[2]), new Quaternion());
                    spawners[p].target = player.gameObject;
                    spawners[p].currentLevel = this;
                    p--;
                    break;
            }
        }
    }

    public void loadTestLevel() {
        waves = new Wave[] {
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 }),
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 1, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 }),
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 2, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 }),
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 3, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 }),
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 4, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 }),
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 5, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 }),
            new Wave(new int[]{ 10, 0, 0, 0 },new int[]{ 6, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 5, 5 })
        };
    }

    /*
    public void startInfinite() {

    }

    public Wave getRandomWave() {


        return null
    }
    */

    public void startLevel() {
        activeWaveNum = 0;
        activeWave = waves[activeWaveNum];
        waveTime = 0;
        nextSpawn = new float[activeWave.amount.Length];
        toSpawn = new int[activeWave.amount.Length];
        for(int i = 0; i< nextSpawn.Length; i++) {
            nextSpawn[i] = activeWave.startTime[i];
            toSpawn[i] = activeWave.amount[i];
        }

        waveEnd = false;
        running = true;
    }

    private void nextWave() {
        running = false;
        activeWaveNum++;
        activeWave = waves[activeWaveNum];
        waveTime = 0;
        nextSpawn = new float[activeWave.amount.Length];
        for (int i = 0; i < nextSpawn.Length; i++) {
            nextSpawn[i] = activeWave.startTime[i];
            toSpawn[i] = activeWave.amount[i];
        }
        waveEnd = false;
        running = true;
    }

    void FixedUpdate() {
        if (running) {
            if (!waveEnd) {
                waveEnd = true;
                for (int i = 0; i < activeWave.amount.Length; i++) {
                    if (toSpawn[i] > 0) {
                        waveEnd = false;
                        if (waveTime >= nextSpawn[i]) {
                            nextSpawn[i] += activeWave.cooldownTime[i];
                            toSpawn[i]--;
                            Enemy neu = spawners[UnityEngine.Random.Range(0, spawners.Length)].spawnEnemy((Enemy.Type)i, activeWave.level[i]);
                            neu.dieCallback = this.enemyKilled;
                            alive.Add(neu);
                        }
                    }
                }
                waveTime += Time.fixedDeltaTime;
            } else {
                if (alive.ToArray().Length == 0) {
                    nextWave();
                }
            }
            
        }
    }

    public void enemyKilled(Enemy dead) {
        alive.Remove(dead);
    }
}
