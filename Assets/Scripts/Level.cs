using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {

    public string Name { get; private set; }
    public int LevelNumber { get; private set; }

    private int _numberOfEnemies;

    public Level(string name, int levelNumber, int numberOfEnemies)
    {
        Name = name;
        LevelNumber = levelNumber;
        _numberOfEnemies = numberOfEnemies;

        if (_numberOfEnemies < 1 || _numberOfEnemies > 7)
        {
            throw new System.Exception("Cannot make a level with less than 1 or more than 7 enemies");
        }
    }

    public List<Vector3> GetEnemyLocations(Rect bounds)
    {
        List<Vector3> enemyLocations = new List<Vector3>();

        if (_numberOfEnemies % 2 != 0)
        {
            // center enemy
            enemyLocations.Add(new Vector3());
        }

        float x = -bounds.width / 3;

        for (int i = 0; i < _numberOfEnemies / 2; i++)
        {
            float y = ( bounds.height / 2) - ((bounds.height) * ((float) (i + 1) / (_numberOfEnemies / 2 + 1)));
            
            // left side
            enemyLocations.Add(new Vector3(-x, y));
            // right side
            enemyLocations.Add(new Vector3(x, y));
        }

        return enemyLocations;
    }

}
