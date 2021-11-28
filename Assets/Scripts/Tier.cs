using System;
using System.Collections.Generic;

[Serializable]
public class Tier
{
    public int requiredScore;
    public List<Recipe> requiredRecipes;

    public bool IsUnlocked(int currentScore, List<Recipe> discoveredRecipes)
    {
        if (currentScore < requiredScore)
        {
            return false;
        }
        foreach (Recipe recipe in requiredRecipes)
        {
            if (!discoveredRecipes.Contains(recipe))
            {
                return false;
            }
        }
        return true;
    }
}
