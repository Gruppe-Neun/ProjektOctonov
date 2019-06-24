using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class Objects
{
    [Serializable()]
    public struct ConstructPlace {
        public ConstructPlace(Vector3 position, int constructType) {
            pos = new float[] { position.x, position.y, position.z };
            type = constructType;
        }
        public float[] pos;
        public int type;      //0 = small, 1 = large
    }

    [Serializable()]
    public struct Container {
        public Container(Vector3 position, int containerType) {
            pos = new float[] { position.x, position.y, position.z };
            type = containerType;
        }
        public float[] pos;
        public int type;
    }

    [Serializable()]
    public struct LightSource {
        public LightSource(Vector3 position, float range, float intensity) {
            pos = new float[] { position.x, position.y, position.z };
            this.range = range;
            this.intensity = intensity;
        }
        public float[] pos;
        public float range;
        public float intensity;
    }

    [Serializable()]
    public struct SpawnPoint {
        public SpawnPoint(Vector3 position, int spawnType) {
            pos = new float[] { position.x, position.y, position.z };
            type = spawnType;
        }
        public float[] pos;
        public int type;
    }

    private static ConstructPlace[] constructPlaces = new ConstructPlace[0];
    private static Container[] container = new Container[0];
    private static LightSource[] light = new LightSource[0];
    private static SpawnPoint[] spawn = new SpawnPoint[0];

    /*
    public static void load(string levelName) {
        string path = Application.dataPath + "/leveldata/" + levelName + "/";
        string constructPlacesFile = path + "objects_constructPlaces.obj";
        string containerFile = path + "objects_container.obj";
        string lightFile = path + "objects_light.obj";
        string spawnFile = path + "objects_spawn.obj";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        if (File.Exists(constructPlacesFile)) {
            file = File.Open(constructPlacesFile, FileMode.Open);
            constructPlaces = (ConstructPlace[])bf.Deserialize(file);
            file.Close();
        }
        if (File.Exists(containerFile)) {
            file = File.Open(containerFile, FileMode.Open);
            container = (Container[]) bf.Deserialize(file);
            file.Close();
        }
        if (File.Exists(lightFile)) {
            file = File.Open(lightFile, FileMode.Open);
            light = (LightSource[])bf.Deserialize(file);
            file.Close();
        }
        if (File.Exists(spawnFile)) {
            file = File.Open(spawnFile, FileMode.Open);
            spawn = (SpawnPoint[])bf.Deserialize(file);
            file.Close();
        }
    }
    */

    public static ConstructPlace[] loadConstruct(string levelName) {
        string path = Application.dataPath + "/leveldata/" + levelName + "/";
        string constructPlacesFile = path + "objects_constructPlaces.obj";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(constructPlacesFile)) {
            file = File.Open(constructPlacesFile, FileMode.Open);
            ConstructPlace[] ret = (ConstructPlace[])bf.Deserialize(file);
            file.Close();
            return ret;
        } else {
            return new ConstructPlace[0];
        }
    }

    public static Container[] loadContainer(string levelName) {
        string path = Application.dataPath + "/leveldata/" + levelName + "/";
        string containerFile = path + "objects_container.obj";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(containerFile)) {
            file = File.Open(containerFile, FileMode.Open);
            Container[] ret = (Container[])bf.Deserialize(file);
            file.Close();
            return ret;
        } else {
            return new Container[0];
        }

    }

    public static LightSource[] loadLight(string levelName) {
        string path = Application.dataPath + "/leveldata/" + levelName + "/";
        string lightFile = path + "objects_light.obj";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(lightFile)) {
            file = File.Open(lightFile, FileMode.Open);
            LightSource[] ret = (LightSource[])bf.Deserialize(file);
            file.Close();
            return ret;
        } else {
            return new LightSource[0];
        }
    }

    public static SpawnPoint[] loadSpawn(string levelName) {
        string path = Application.dataPath + "/leveldata/" + levelName + "/";
        string spanwFile = path + "objects_spawn.obj";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(spanwFile)) {
            file = File.Open(spanwFile, FileMode.Open);
            SpawnPoint[] ret = (SpawnPoint[])bf.Deserialize(file);
            file.Close();
            return ret;
        } else {
            return new SpawnPoint[0];
        }
    }
}
