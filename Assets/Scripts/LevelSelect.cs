using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    private int _selectIndex;
    private Level _selectedLevel;
    public GameObject previewBox;
    private Rect _previewRect;
    private List<Vector3> _enemyLocations;
    public List<GameObject> enemyPreviews;
    public Text levelName;
    public Text highScoreText;
    public GameObject lockOverlay;
    public Text lockText;
    public GameObject leftArrow;
    public GameObject rightArrow;

	// Use this for initialization
	void Start () {
        RectTransform previewTransform = previewBox.GetComponent<RectTransform>();
        _previewRect = new Rect(previewTransform.position.x, previewTransform.position.y, previewTransform.rect.width, previewTransform.rect.height);
        _selectedLevel = GlobalManager.currentLevel;
        _selectIndex = _selectedLevel.LevelNumber;
        _enemyLocations = _selectedLevel.GetEnemyLocations(_previewRect);

        UpdateSelectedLevel();
    }

    // Update is called once per frame
    void Update()  {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DisplayLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DisplayRight();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                string highScoreKey = "highScore" + GlobalManager.Levels[_selectedLevel.LevelNumber - 1].Name;
                if (_selectedLevel.LevelNumber < 2 || (PlayerPrefs.HasKey(highScoreKey) && PlayerPrefs.GetFloat(highScoreKey) >= 60f))
                {
                    GlobalManager.currentLevel = _selectedLevel;
                    SceneManager.LoadScene(2);
                }
            }
            return;
        }
        
        for (int i = 0; i < _enemyLocations.Count; i++)
        {
            enemyPreviews[i].SetActive(false);
        }

        UpdateSelectedLevel();
    }

    public void DisplayLeft()
    {
        if (_selectIndex > 1)
        {
            _selectIndex -= 1;
            leftArrow.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _selectIndex = 0;
            leftArrow.GetComponent<Image>().color = Color.gray;
        }
        rightArrow.GetComponent<Image>().color = Color.white;
    }

    public void DisplayRight()
    {
        if (_selectIndex < GlobalManager.Levels.Count - 2)
        {
            _selectIndex += 1;
            rightArrow.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _selectIndex = GlobalManager.Levels.Count - 1;
            rightArrow.GetComponent<Image>().color = Color.gray;
        }
        leftArrow.GetComponent<Image>().color = Color.white;
    }

    void UpdateSelectedLevel()
    {
        _selectedLevel = GlobalManager.Levels[_selectIndex];
        _enemyLocations = _selectedLevel.GetEnemyLocations(_previewRect);
        levelName.text = _selectedLevel.Name;
        levelName.color = Color.green;
        lockText.text = "";

        lockOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 0);


        if (PlayerPrefs.HasKey("highScore" + _selectedLevel.Name))
        {
            highScoreText.text = "Best Time: " + PlayerPrefs.GetFloat("highScore" + _selectedLevel.Name).ToString("0.00s");
        }

        else
        {
            highScoreText.text = "Best Time: 0.00s";
        }

        if (_selectedLevel.LevelNumber >= 2)
        {

            string prevLevel = "highScore" + GlobalManager.Levels[_selectedLevel.LevelNumber - 1].Name;
            if (!PlayerPrefs.HasKey(prevLevel) || PlayerPrefs.GetFloat(prevLevel)  < 60f)
            {
                lockOverlay.GetComponent<Image>().color = new Color(0, 0, 0, .9f);
                levelName.color = Color.red;
                lockText.text = "Survive for 60s or more in \"" + prevLevel.Substring("highScore".Length) +  "\"";
            }
        }

        for (int i = 0; i < _enemyLocations.Count; i++)
        {
            Vector3 pos = _enemyLocations[i];
            enemyPreviews[i].transform.localPosition = pos;
            enemyPreviews[i].SetActive(true);
        }
    }
}
