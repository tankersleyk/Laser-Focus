using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoundManager : MonoBehaviour {

    private class Enemy
    {
        private static Color pink = new Color32(255, 192, 203, 255);

        private GameObject _circle;
        private ActiveType _state;
        private float _activeTime;

        public Enemy (GameObject circle)
        {
            _circle = circle;
            _state = ActiveType.Inactive;
        }

        public void Activate ()
        {
            _state = ActiveType.Level1;
            _activeTime = Time.time;
            _circle.GetComponent<Image>().color = Color.red;
        }

        public void MakeFriendly ()
        {
            _state = ActiveType.Friendly;
            _activeTime = Time.time;
            _circle.GetComponent<Image>().color = pink;
        }

        public float GetActiveTime ()
        {
            return _activeTime;
        }

        public ActiveType GetState ()
        {
            return _state;
        }

       /// <summary>
       /// Deactives the game object
       /// </summary>
       /// <returns> true iff the gameobject was moved to the inactive state</returns>
        public bool Deactive ()
        {
            if (_state != ActiveType.Inactive)
            {
                _state = ActiveType.Inactive;
                _circle.GetComponent<Image>().color = Color.white;
                return true;
            }
            return false;
        }
    }

    private enum ActiveType { Level1, Friendly, Inactive };

    private Enemy _circle1;
    private Enemy _circle2;
    private Enemy _circle3;
    private Enemy _circle4;
    private GameObject _gameOverText;

    private bool _roundActive;
    private float _lastRandomTime;

    private Dictionary<KeyCode, Enemy> _keyMap = new Dictionary<KeyCode, Enemy>();
    private ArrayList _inactiveEnemies = new ArrayList(); // easier to randomly pull, not a big deal since not that many elements
    private ArrayList _activeEnemies = new ArrayList();
    private ArrayList _friendlyEnemies = new ArrayList();

    private void Awake()
    {
        _gameOverText = GameObject.Find("GameOverText");
        _gameOverText.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
        _circle1 = new Enemy(GameObject.Find("circle1"));
        _circle2 = new Enemy(GameObject.Find("circle2"));
        _circle3 = new Enemy(GameObject.Find("circle3"));
        _circle4 = new Enemy(GameObject.Find("circle4"));

        _keyMap.Add(KeyCode.Q, _circle1);
        _keyMap.Add(KeyCode.A, _circle2);
        _keyMap.Add(KeyCode.O, _circle3);
        _keyMap.Add(KeyCode.L, _circle4);

        _inactiveEnemies.Add(_circle1);
        _inactiveEnemies.Add(_circle2);
        _inactiveEnemies.Add(_circle3);
        _inactiveEnemies.Add(_circle4);

        _roundActive = true;
        _lastRandomTime = Time.time + .5f;
    }
	
    void RandomEnable()
    {
        if (_inactiveEnemies.Count == 0)
        {
            return;
        }
        
        int randIndx = Random.Range(0, _inactiveEnemies.Count);
        Enemy enemy = (Enemy)_inactiveEnemies[randIndx];

        _inactiveEnemies.RemoveAt(randIndx);

        if (Random.Range(1, 10) < 2.0f)
        {
            enemy.MakeFriendly();
            _friendlyEnemies.Add(enemy);
        }
        else
        {
            enemy.Activate();
            _activeEnemies.Add(enemy);
        }

        _lastRandomTime = Time.time;
    }

	// Update is called once per frame
	void Update ()
    {
		if (_roundActive)
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

            foreach (KeyValuePair<KeyCode, Enemy> entry in _keyMap)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    Enemy enemy = entry.Value;

                    if (enemy.GetState() == ActiveType.Friendly) // killed a friendly D:
                    {
                        _roundActive = false;
                        _gameOverText.SetActive(true);
                    }

                    else
                    {
                        bool deactived = enemy.Deactive();

                        if (deactived)
                        {
                            _inactiveEnemies.Add(enemy);
                            _activeEnemies.Remove(enemy);
                        }
                    }
                }
            }

            foreach (Enemy enemy in _activeEnemies)
            {
                if (Time.time - enemy.GetActiveTime() >= 3.0f)
                {
                    _roundActive = false;
                    _gameOverText.SetActive(true);
                }
            }

            ArrayList toRemove = new ArrayList();

            foreach (Enemy enemy in _friendlyEnemies) // remove friendlies who have been there too long ;P
            {
                if (Time.time - enemy.GetActiveTime() >= 1.0f)
                {
                    enemy.Deactive();
                    _inactiveEnemies.Add(enemy);
                    toRemove.Add(enemy);
                }
            }

            foreach (Enemy enemy in toRemove)
            {
                _friendlyEnemies.Remove(enemy);
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _roundActive = true;
                _gameOverText.SetActive(false);

                ArrayList toRemove = new ArrayList();

                foreach (Enemy enemy in _activeEnemies)
                {
                    enemy.Deactive(); // TODO: when levels, make sure have way to reset
                    toRemove.Add(enemy);
                    _inactiveEnemies.Add(enemy);
                }

                foreach (Enemy enemy in toRemove)
                {
                    _activeEnemies.Remove(enemy);
                }

                _lastRandomTime = Time.time + 0.5f;
            }
        }

	}
}
