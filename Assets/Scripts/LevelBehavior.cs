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

    private int infiniteRandom = -1;  //-1 not random; 0 = random just level increase; >0 = random level and wave amount increase every X levels

    private UIBehavior ui;

    private SpawnerBehavior[] spawners;
    private SpawnerBehavior[] heavySpawners;
    private SpawnerBehavior[] flyingSpawners;
    private SpawnerBehavior[] playerTargetSpawner;

    private Wave[] waves;
    private float[] between; 

    private int activeWaveNum = 0;
    private Wave[] activeWave;
    private bool running = false;
    private bool waveEnd = false;
    private float[,] nextSpawn;
    private int[,] toSpawn;
    [SerializeField] private float waveTime = 0f;
    private List<Enemy> alive;


    public void Awake() {
        ui = GameObject.Find("UI").GetComponent<UIBehavior>();
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
    
    public void loadInfinite(int difficulty) {
        switch (difficulty) {
            case 0:
                waves = new Wave[] {
                    new Wave(new int[]{ 30, 0, 0, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 10, 5, 5 }),
                    new Wave(new int[]{ 0, 0, 10, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 12, 5 }),
                    new Wave(new int[]{ 0, 40, 0, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 4, 10, 5 }),
                    new Wave(new int[]{ 20, 0, 5, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 15, 5 }),
                    new Wave(new int[]{ 0, 30, 5, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 15, 5 }),
                    new Wave(new int[]{ 20, 30, 0, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 10, 5 }),
                    new Wave(new int[]{ 10, 20, 5, 0 },new int[]{ 0, 0, 0, 0 }, new bool[]{ false, false, false, false }, new float[]{ 0, 0, 0, 0 }, new float[]{ 5, 5, 15, 5 })
                };
                break;

            case 1:

                break;

            case 2:

                break;

            default:

                break;
        }
    }

    

    public void startLevel(int randomInfinite = -1) {
        this.infiniteRandom = randomInfinite;
        if (randomInfinite == -1) {
            activeWave = new Wave[1];
            activeWaveNum = 0;
            activeWave[0] = waves[activeWaveNum];
            waveTime = 0;
            nextSpawn = new float[1,activeWave[0].amount.Length];
            toSpawn = new int[1,activeWave[0].amount.Length];
            for (int i = 0; i < nextSpawn.Length; i++) {
                nextSpawn[0,i] = activeWave[0].startTime[i];
                toSpawn[0,i] = activeWave[0].amount[i];
            }

            waveEnd = false;
            running = true;
            waveTime = -20f;
        } else {
            activeWaveNum = 0;
            activeWave = new Wave[1];
            //nextSpawn = new float[count, waves[0].amount.Length];
            //toSpawn = new int[count, waves[0].amount.Length];
            nextSpawn = new float[1, 4];
            toSpawn = new int[1, 4];
            activeWave[0] = waves[UnityEngine.Random.Range(0, waves.Length)];
            for (int i = 0; i < nextSpawn.Length; i++) {
                nextSpawn[0, i] = activeWave[0].startTime[i];
                toSpawn[0, i] = activeWave[0].amount[i];
            }

            waveEnd = false;
            running = true;
            waveTime = -20f;
            
        }
        Debug.Log("Next Wave:  Spider:" + activeWave[0].amount[0] + "  Spider_fast: " + activeWave[0].amount[1] + "Spider_large: " + activeWave[0].amount[2]);
    }

    private void nextWave() {
        if (infiniteRandom==-1) {
            activeWave = new Wave[1];
            running = false;
            activeWaveNum++;
            activeWave[0] = waves[activeWaveNum];
            nextSpawn = new float[1, activeWave[0].amount.Length];
            toSpawn = new int[1, activeWave[0].amount.Length];
            for (int i = 0; i < nextSpawn.Length; i++) {
                nextSpawn[0,i] = activeWave[0].startTime[i];
                toSpawn[0,i] = activeWave[0].amount[i];
            }
            waveEnd = false;
            waveTime = -10f;
            running = true;
        } else {
            running = false;
            activeWaveNum++;
            int count;
            if (infiniteRandom == 0) count = 1;
            else count = (int)(activeWaveNum / infiniteRandom);
            activeWave = new Wave[count];
            
            nextSpawn = new float[count, waves[0].amount.Length];
            toSpawn = new int[count, waves[0].amount.Length];
            for (int w = 0; w < count; w++) {
                waves[w] = waves[UnityEngine.Random.Range(0, waves.Length)];
                for (int i = 0; i < nextSpawn.Length; i++) {
                    nextSpawn[w, i] = activeWave[w].startTime[i];
                    toSpawn[w, i] = activeWave[w].amount[i];
                }
            }
            waveEnd = false;
            waveTime = -10f;
            running = true;
        }
        Debug.Log("Next Wave:  Spider:" + activeWave[0].amount[0] + "  Spider_fast: " + activeWave[0].amount[1] + "Spider_large: " + activeWave[0].amount[2]);
    }

    void FixedUpdate() {
        if (running) {
            if (waveTime > 0) {
                if (!waveEnd) {
                    waveEnd = true;
                    for(int w = 0; w < activeWave.Length; w++) {
                        for (int i = 0; i < activeWave[w].amount.Length; i++) {
                            if (toSpawn[w,i] > 0) {
                                waveEnd = false;
                                if (waveTime >= nextSpawn[w,i]) {
                                    nextSpawn[w,i] += activeWave[w].cooldownTime[i];
                                    toSpawn[w,i]--;
                                    Enemy neu = spawners[UnityEngine.Random.Range(0, spawners.Length)].spawnEnemy((Enemy.Type)i, activeWave[w].level[i]);
                                    neu.dieCallback = this.enemyKilled;
                                    alive.Add(neu);
                                }
                            }
                        }
                    }
                    waveTime += Time.fixedDeltaTime;
                } else {
                    if (alive.ToArray().Length == 0) {
                        nextWave();
                    }
                }
            } else {
                ui.setWaveCounter(true, activeWaveNum, -waveTime);
                waveTime += Time.fixedDeltaTime;
                if(waveTime > 0) ui.setWaveCounter(false);
            }
        }
    }

    public void enemyKilled(Enemy dead) {
        alive.Remove(dead);
    }
}
