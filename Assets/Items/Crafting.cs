using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Crafting{
    public enum CraftingStationType {
        NONE,
        Tüfteltisch
    }

    public struct Recipe {
        public Recipe(Item.Type[] ing, int[] ingAmount, CraftingStationType station, Item.Type res, int resAmount) {
            ingredients = ing;
            if (ingAmount == null){
                amount = new int[] { 1, 1, 1, 1 };
            } else {
                amount = ingAmount;
            }
            craftingstation = station;
            result = res;
            resultAmount = resAmount;
        }
        public Item.Type[] ingredients;
        public int[] amount;
        public CraftingStationType craftingstation;
        public Item.Type result;
        public int resultAmount;
    }

    private static Recipe[] recipes;

    public static void loadTest() {
        Recipe test0 = new Recipe(new Item.Type[] { Item.Type.UNDEF, Item.Type.UNDEF, Item.Type.Battery, Item.Type.Flashlight }, null, CraftingStationType.NONE, Item.Type.LaserRed, 10);
        recipes = new Recipe[] { test0 };
    }

    //load recipies from file
    public static void loadRecipes(string filePath) {

    }

    //sort ingredients and add recipe
    private static void insertRecipe(Recipe insert) {

    }

    public static Recipe[] getAll() {
        return recipes;
    }

    public static ItemBehavior getResult(ItemBehavior[] input, CraftingStationType station) {
        if (input.Length != 4) return null;
        Item.Type[] sortedInput;


        return null;
    }

    public static ItemBehavior craft(ItemBehavior[] input,CraftingStationType station) {
        if (input.Length != 4) return null;
        ItemBehavior[] sortedInput = new ItemBehavior[4];
        for(int i = 0;i<input.Length;i++) {
            if (input[i] == null) {
                input[i] = Item.createItem(Item.Type.UNDEF, 100, new Vector3(0, 0, 0));
            }
        }
        sortedInput[0] = input[0];
        for(int i = 1; i < sortedInput.Length; i++) {
            int pos = i-1;
            while (pos >= 0) {
                if ((int)sortedInput[pos].type <= (int)input[i].type) {
                    break;
                }
                pos--;
            }

            sortedInput[i] = sortedInput[pos + 1];
            sortedInput[pos + 1] = input[i];
        }

        int recipe = 0;
        while (recipe < recipes.Length) {
            if(sortedInput[0].type==recipes[recipe].ingredients[0] &&
               sortedInput[1].type == recipes[recipe].ingredients[1] &&
               sortedInput[2].type == recipes[recipe].ingredients[2] &&
               sortedInput[3].type == recipes[recipe].ingredients[3] &&
               station == recipes[recipe].craftingstation) {

                if(sortedInput[0].amount >= recipes[recipe].amount[0] &&
                   sortedInput[1].amount >= recipes[recipe].amount[1] &&
                   sortedInput[2].amount >= recipes[recipe].amount[2] &&
                   sortedInput[3].amount >= recipes[recipe].amount[3]) {

                    sortedInput[0].amount -= recipes[recipe].amount[0];
                    sortedInput[1].amount -= recipes[recipe].amount[1];
                    sortedInput[2].amount -= recipes[recipe].amount[2];
                    sortedInput[3].amount -= recipes[recipe].amount[3];
                    ItemBehavior result = Item.createItem(recipes[recipe].result, recipes[recipe].resultAmount, new Vector3(0, 0, 0));
                    return result;
                } else {
                    return null;
                }
            } else {
                recipe++;
            }
        }
        return null;
    }
}
