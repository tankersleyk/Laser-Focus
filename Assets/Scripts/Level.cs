using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {

    public string Name { get; private set; }
    public List<Vector3> EnemyLocations { get; private set; }

    public Level(string name, int numberOfEnemies, Rect bounds)
    {
        Name = name;
        ConstructEnemyLocations(numberOfEnemies, bounds);
    }

    private void ConstructEnemyLocations(int numberOfEnemies, Rect bounds)
    {
        EnemyLocations = new List<Vector3>();
        if (numberOfEnemies < 1 || numberOfEnemies > 7)
        {
        throw new System.Exception("Cannot make a level with less than 1 or more than 7 enemies");
        }

        if (numberOfEnemies % 2 != 0)
        {
            // center enemy
            EnemyLocations.Add(new Vector3());
            numberOfEnemies -= 1;
        }

        if (numberOfEnemies == 0)
        {
            return;
        }

        float x = -bounds.width / 3;

        for (int i = 0; i < numberOfEnemies / 2; i++)
        {
            float y = ( bounds.height / 2) - ((bounds.height) * ((float) (i + 1) / (numberOfEnemies / 2 + 1)));
            
            // left side
            EnemyLocations.Add(new Vector3(-x, y));
            // right side
            EnemyLocations.Add(new Vector3(x, y));
        }
    }

}
