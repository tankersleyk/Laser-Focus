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
        GetComponent<Image>().color = Color.red;
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
    public ActiveType HandleClick()
    {
        if (Input.GetKeyDown(activateKey))
        {
            if (_state == ActiveType.Aggressive)
            {
                _level -= 1;
                if (_level == 0)
                {
                    _state = ActiveType.Inactive;
                    GetComponent<Image>().color = Color.white;
                }
            }

            else if (_state == ActiveType.Friendly)
            {
                _state = ActiveType.GameOver;
            }
        }

        if (_state == ActiveType.Aggressive && Time.time - _activeTime >= 3.0f)
        {
            _state = ActiveType.GameOver;
        }

        else if (_state == ActiveType.Friendly && Time.time - _activeTime >= 1.0f)
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

            for (int i = 1; i <= _level; ++i)
            {
                float x = (pos.x - rect.width / 2) + rect.width * i / (_level + 1) - w/2;
                GUI.Box(new Rect(x, Screen.height - y, w, h), GUIContent.none, _RectStyle);
            }
        }
    }
}