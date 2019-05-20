using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item
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

    public static Texture2D getSprite(Type itemType) {
        if (sprites[(int)itemType] == null) {
            loadSprites();
        }
        else{
            return sprites[(int)itemType];
        }
        

        return null;
    }


}
