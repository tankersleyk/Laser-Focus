using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static List<Level> Levels { get; private set; }
    public static Level currentLevel;
    public static Dictionary<Level, LevelInfo> levelDefinitions;

    // Use this for initialization
    void Start()
    {
        // Add levels - name + number of enemies + "level number"
        Levels = new List<Level>();
        Levels.Add(new Level("Get Acclimated", 0, 1));
        Levels.Add(new Level("Simple", 1, 2));
        Levels.Add(new Level("Tricky", 2, 3));
        Levels.Add(new Level("The Wall", 3, 4));
        Levels.Add(new Level("Actually Difficult", 4, 5));
        Levels.Add(new Level("Good luck", 5, 6));
        Levels.Add(new Level("Impossible", 6, 7));

        currentLevel = Levels[1];

        // Add level defintions
        // TODO: define these more individually
        levelDefinitions = new Dictionary<Level, LevelInfo>();
        foreach (Level level in Levels)
        {
            levelDefinitions.Add(level, new LevelInfo(3f, 4, 4));
        }

    }
}
