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

    /*
    public struct SpawnPoint {
        public Vector3 pos;
        public Vector3[] route;
    }
    */

    private static ConstructPlace[] constructPlaces = new ConstructPlace[0];
    private static Container[] container = new Container[0];

    public static void load(string levelName) {
        string path = Application.dataPath + "/leveldata/" + levelName + "/";
        string constructPlacesFile = path + "objects_constructPlaces.obj";
        string containerFile = path + "objects_container.obj";
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

    }

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
}
