﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    private LevelInfo _info;

    public GameObject _enemyLocationsBox;
    private Rect _enemyRect;
    public GameObject _gameOverText;
    public Text _timerText;

    public List<GameObject> potentialEnemies = new List<GameObject>();
    private List<Enemy> _inactiveEnemies = new List<Enemy>(); // easier to randomly pull, not a big deal since not that many elements
    private List<Enemy> _activeEnemies = new List<Enemy>();

    private float _time;

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
        _info = GlobalManager.levelDefinitions[GlobalManager.currentLevel];

        RectTransform locationTransform = _enemyLocationsBox.GetComponent<RectTransform>();

        _enemyRect = new Rect(locationTransform.position.x, locationTransform.position.y, locationTransform.rect.width, locationTransform.rect.height);

        List<Vector3> enemyLocations = GlobalManager.currentLevel.GetEnemyLocations(_enemyRect);

        for (int i = 0; i < enemyLocations.Count; i++)
        {
            Vector3 pos = enemyLocations[i];
            GameObject enemy = potentialEnemies[i];
            enemy.transform.localPosition = pos;
            enemy.SetActive(true);
            _inactiveEnemies.Add(enemy.GetComponent<Enemy>());
        }
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
                enemy.Activate(Random.Range(1, _info.maxLevel));
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
                    string highscoreKey = "highScore" + GlobalManager.currentLevel.Name;
                    if (!PlayerPrefs.HasKey(highscoreKey) || PlayerPrefs.GetFloat(highscoreKey) < _time)
                    {
                        PlayerPrefs.SetFloat(highscoreKey, _time);
                    }
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


            _time = (Time.time - _startTime);
            _timerText.text = _time.ToString("0.<size=45>00</size>");
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

            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(1);
            }
        }
	}
}
