using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActiveType { Aggressive, Friendly, Inactive, GameOver };

public class Enemy : MonoBehaviour
{
    public KeyCode activateKey;

    private static Color pink = new Color32(255, 192, 203, 255);
    private static Color purple = new Color32(148, 0, 211, 255);
    private ActiveType _state;
    private float _activeTime;
    private int _level;
    private int _chargeLevel; // TODO: Change this var name, too confusing with _level
    private float _elapsedTime;

    private static Dictionary<int, Color> _colorTabel = new Dictionary<int, Color>
    {
        {1, Color.red},
        {2, Color.blue},
        {3, Color.yellow},
        {4, Color.green}
    };

    public Charger _charger;

    // For drawing levels
    private static Texture2D _RectTexture;
    private static GUIStyle _RectStyle;

    private void Awake()
    {
        if (_RectTexture == null)
        {
            _RectTexture = new Texture2D(1, 1);
            _RectTexture.SetPixel(0, 0, Color.black);
            _RectTexture.Apply();
            _RectStyle = new GUIStyle { normal = new GUIStyleState { background = _RectTexture } };
        }
    }


    private void Start()
    {
        _state = ActiveType.Inactive;
        _level = 0;
    }

    /// <summary>
    /// Makes this enemy aggressive
    /// </summary>
    /// <param name="level"> The level to activate( >= 1) </param>
    public void Activate(int level)
    {
        _state = ActiveType.Aggressive;
        _activeTime = Time.time;
        _level = level;
        _chargeLevel = Random.Range(1, 4);
        GetComponent<Image>().color = _colorTabel[_chargeLevel];
    }

    /// <summary>
    /// Makes this enemy friendly
    /// </summary>
    public void MakeFriendly()
    {
        _state = ActiveType.Friendly;
        _activeTime = Time.time;
        GetComponent<Image>().color = purple;
    }

    /// <summary>
    ///  Handles updates including keypresses and time management for game over/resetting to inactive
    /// </summary>
    /// <returns> the state of this enemy after the update has effected </returns>
    public ActiveType ProcessInput()
    {
        float chargeAmount = _charger.Charge();

        if (_state == ActiveType.Aggressive)
        {
            if (chargeAmount >= 0.25f * _chargeLevel)
            {
                _level -= 1;

                if (_level == 0)
                {
                    _state = ActiveType.Inactive;
                    GetComponent<Image>().color = Color.white;
                }

                else
                {
                    // TODO: rethink this, possible to abuse?
                    // Give only what is possible for player to handle in time 
                    // 2.8 instead of 3 so that player has 0.2s at least to react
                    int maxChargeLevel = (int)System.Math.Min((2.8f - (Time.time - _activeTime)) / 0.5f, 4f);
                    _chargeLevel = Random.Range(1, maxChargeLevel);

                    GetComponent<Image>().color = _colorTabel[_chargeLevel];
                }
            }
        }

        else if (_state == ActiveType.Friendly)
        {
            if (chargeAmount >= 0.25f)
            {
                _state = ActiveType.GameOver;
            }
        }

        _elapsedTime = Time.time - _activeTime;

        if (_state == ActiveType.Aggressive && _elapsedTime >= 3.0f)
        {
            _state = ActiveType.GameOver;
        }

        else if (_state == ActiveType.Friendly && _elapsedTime >= 1.0f)
        {
            _state = ActiveType.Inactive;
            GetComponent<Image>().color = Color.white;
        }

        return _state;
    }

    public float GetActiveTime()
    {
        return _activeTime;
    }

    public ActiveType GetState()
    {
        return _state;
    }

    public void Reset()
    {
        _state = ActiveType.Inactive;
        _level = 0;
        GetComponent<Image>().color = Color.white;
        _charger.Reset();
        _elapsedTime = 0;
    }

    private void OnGUI()
    {
        if (_level > 0)
        {
            Vector3 pos = this.transform.position;
            Rect rect = GetComponent<RectTransform>().rect;

            float y = pos.y + rect.height / 4;
            float h = rect.height / 2;
            float w = rect.width / 8;
            float x;

            for (int i = 1; i <= _level; ++i)
            {
                x = (pos.x - rect.width / 2) + rect.width * i / (_level + 1) - w / 2;
                DrawRect(x, y, w, h, Color.black);
            }

            h = 20f;
            w = (2 * rect.width) * (3.0f - _elapsedTime) / 3.0f;
            x = pos.x - (rect.width);
            y = pos.y - rect.height;
            DrawRect(x, y, w, h, Color.green);
        }
    }

    /// <summary>
    /// Draws a rectangle at the given world space with the given width and height
    /// </summary>
    /// <param name="x"> the global x-coordinate </param>
    /// <param name="y"> the global y-coordinate </param>
    /// <param name="w"> the width of the rectangle </param>
    /// <param name="h"> the height of the rectangle </param>
    /// <param name="color"> the desired color of the rectangle </param>
    private void DrawRect(float x, float y, float w, float h, Color color)
    {
        _RectTexture.SetPixel(0, 0, color);
        _RectTexture.Apply();
        GUI.Box(new Rect(x, Screen.height - y, w, h), GUIContent.none, _RectStyle);
    }
}