﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoundManager : MonoBehaviour
{
    private static bool roundActive = false;
    private Enemy _circle1;
    private Enemy _circle2;
    private Enemy _circle3;
    private Enemy _circle4;
    private float _lastRandomTime;
    private float _startTime;

    public GameObject _gameOverText;
    public Text _timerText;
    
    public List<Enemy> _inactiveEnemies = new List<Enemy>(); // easier to randomly pull, not a big deal since not that many elements
    private List<Enemy> _activeEnemies = new List<Enemy>();

    private void Awake()
    {
        _gameOverText.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
        roundActive = true;
        _lastRandomTime = Time.time + .5f; // wait a little bit at start to give some breathing room
        _startTime = Time.time;
    }

    void RandomEnable()
    {
        if (_inactiveEnemies.Count == 0)
        {
            return;
        }

        int randIndx = Random.Range(0, _inactiveEnemies.Count);
        Enemy enemy = (Enemy)_inactiveEnemies[randIndx];

        if (Time.time - enemy.GetActiveTime() >= 5.0f) // Give breathing room before reactivation
        {
            _inactiveEnemies.RemoveAt(randIndx);

            if (Random.Range(1, 10) < 2.0f)
            {
                enemy.MakeFriendly();
                _activeEnemies.Add(enemy);
            }
            else
            {
                enemy.Activate(Random.Range(1, 4));
                _activeEnemies.Add(enemy);
            }

            _lastRandomTime = Time.time;
        }

        
    }

	// Update is called once per frame
	void Update ()
    {
		if (roundActive)
        {
            if (Time.time - _lastRandomTime >= 0.5f)
            {
                RandomEnable();
            }
            else if (Time.time - _lastRandomTime >= 0.01f)
            {
                if (Random.Range(0, 3) >= 2.6f)
                {
                    RandomEnable();
                }
            }

            ArrayList toRemove = new ArrayList();

            foreach (Enemy enemy in _activeEnemies)
            {
                ActiveType result = enemy.ProcessInput();

                if (result == ActiveType.GameOver)
                {
                    roundActive = false;
                    _gameOverText.SetActive(true);
                }

                else if (result == ActiveType.Inactive)
                {
                    toRemove.Add(enemy);
                }
            }

            foreach (Enemy enemy in toRemove)
            {
                _activeEnemies.Remove(enemy);
                _inactiveEnemies.Add(enemy);
            }

            _timerText.text = (int) (Time.time - _startTime) + "s";
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                roundActive = true;
                _gameOverText.SetActive(false);

                ArrayList toRemove = new ArrayList();

                foreach (Enemy enemy in _activeEnemies)
                {
                    enemy.Reset();
                    toRemove.Add(enemy);
                    _inactiveEnemies.Add(enemy);
                }

                foreach (Enemy enemy in toRemove)
                {
                    _activeEnemies.Remove(enemy);
                }

                _lastRandomTime = Time.time + 0.5f;
                _startTime = Time.time;
            }
        }
	}
}
