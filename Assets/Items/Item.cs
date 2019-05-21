using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Item
{
    public enum Type{
        UNDEF,
        BATTERY
    }
    private static string[] names = new string[] {
        "Undef",
        "Battery"
    };

    private static Texture2D[] sprites;
    private static Material[] itemMaterial;

    public static void loadSprites() {
        sprites = new Texture2D[names.Length];
        for (int i = 0;i<names.Length;i++) {
            string filePath = Application.dataPath + "/Items/Sprites/Item_" + names[i] + ".png";
            if (File.Exists(filePath)) {
                sprites[i] = new Texture2D(128, 64);
                sprites[i].LoadImage(File.ReadAllBytes(filePath));
            }
        }
    }

    public static void loadMaterial() {
        itemMaterial = new Material[names.Length];
        for (int i = 0;i<names.Length;i++) {
            if (sprites[i]!=null) {
                itemMaterial[i] = new Material(Shader.Find("Sprites/Diffuse"));
                itemMaterial[i].mainTexture = sprites[i];

                /*
                itemMaterial[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                itemMaterial[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                itemMaterial[i].SetInt("_ZWrite", 1);
                itemMaterial[i].EnableKeyword("_ALPHATEST_ON");
                itemMaterial[i].DisableKeyword("_ALPHABLEND_ON");
                itemMaterial[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                itemMaterial[i].renderQueue = 2450;
                */
            }
        }
    }

    public static Texture2D getSprite(Type itemType) {
        if (sprites[(int)itemType] == null) {
            loadSprites();
        }
        else{
            return sprites[(int)itemType];
        }
        

        return null;
    }

    public static Mesh getItemMesh() {
        Mesh itemMesh = new Mesh();

        //set vertices

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-1, -0.5f, 0);
        vertices[1] = new Vector3(1, -0.5f, 0);
        vertices[2] = new Vector3(-1, 0.5f, 0);
        vertices[3] = new Vector3(1, 0.5f, 0);
        itemMesh.vertices = vertices;

        //set triangles

        int[] tri = new int[6];
        //  Lower left triangle.
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;
        //  Upper right triangle.   
        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;
        itemMesh.triangles = tri;

        //set normals

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        itemMesh.normals = normals;

        //set UVs

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        itemMesh.uv = uv;

        return itemMesh;
    }

    public static GameObject createItem(Type itemType,Vector3 position) {
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        item.AddComponent<ItemBehavior>();
        item.GetComponent<MeshFilter>().mesh = getItemMesh();

        item.GetComponent<MeshRenderer>().material = itemMaterial[(int) itemType];
        

        //item.GetComponent<MeshCollider>().sharedMesh = getItemMesh();
        item.GetComponent<SphereCollider>().isTrigger = true;



        item.GetComponent<ItemBehavior>().drop();

        item.transform.position = position;
        return item;
    }

    
}
