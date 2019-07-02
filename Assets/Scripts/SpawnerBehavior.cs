using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    [SerializeField] private PA_WarriorBehavior spider;
    [SerializeField] private PA_WarriorBehavior spider_fast;
    [SerializeField] private PA_WarriorBehavior spider_large;
    [SerializeField] private DroneEnemy drone;
    [SerializeField] private float eliteChance = 0.01f;

    public GameObject target;
    public LevelBehavior currentLevel;

    public int type = 0;

    private UIBehavior ui;
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI").GetComponent<UIBehavior>();
    }

    public Enemy spawnEnemy(Enemy.Type enemy, int level) {
        Enemy ret;
        switch (enemy) {
            case Enemy.Type.Spider:
                ret = Instantiate<PA_WarriorBehavior>(spider, this.transform.position, new Quaternion());
                break;

            case Enemy.Type.Spider_fast:
                ret = Instantiate<PA_WarriorBehavior>(spider_fast, this.transform.position, new Quaternion());
                break;

            case Enemy.Type.Spider_large:
                ret = Instantiate<PA_WarriorBehavior>(spider_large, this.transform.position, new Quaternion());
                break;

            case Enemy.Type.Drone:
                ret = Instantiate<DroneEnemy>(drone, this.transform.position + new Vector3(0,5,0), new Quaternion());
                break;

            default:
                return null;        
        }
        ret.setLevel(level);
        ret.setTarget(target);
        if(Random.value <= eliteChance) {
            ret.setLevel(level, true);
            ui.sendWarning("Elite Enemy has\nbeen spawned");
        }
        ret.dieCallback = currentLevel.enemyKilled;
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
