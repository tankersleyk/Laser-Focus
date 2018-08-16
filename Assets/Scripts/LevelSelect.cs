using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    private int _selectIndex;
    private Level _selectedLevel;
    private List<Level> _levelPreviews = new List<Level>();
    public GameObject previewBox;
    private Rect _previewRect;
    public List<GameObject> enemyPreviews;
    public Text levelName;
    public GameObject leftArrow;
    public GameObject rightArrow;

	// Use this for initialization
	void Start () {
        RectTransform previewTransform = previewBox.GetComponent<RectTransform>();
        _previewRect = new Rect(previewTransform.position.x, previewTransform.position.y, previewTransform.rect.width, previewTransform.rect.height);
        _levelPreviews.Add(new Level("Get Acclimated", 1, _previewRect));
        _levelPreviews.Add(new Level("Simple", 2, _previewRect));
        _levelPreviews.Add(new Level("Tricky", 3, _previewRect));
        _levelPreviews.Add(new Level("The Wall", 4, _previewRect));
        _levelPreviews.Add(new Level("Actually Difficult", 5, _previewRect));
        _levelPreviews.Add(new Level("Good luck", 6, _previewRect));
        _levelPreviews.Add(new Level("Impossible", 7, _previewRect));
        _selectIndex = 1;
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
                SceneManager.LoadScene(2);
            }
            return;
        }
        
        for (int i = 0; i < _selectedLevel.EnemyLocations.Count; i++)
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
        if (_selectIndex < _levelPreviews.Count - 2)
        {
            _selectIndex += 1;
            rightArrow.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _selectIndex = _levelPreviews.Count - 1;
            rightArrow.GetComponent<Image>().color = Color.gray;
        }
        leftArrow.GetComponent<Image>().color = Color.white;
    }

    void UpdateSelectedLevel()
    {
        _selectedLevel = _levelPreviews[_selectIndex];
        levelName.text = _selectedLevel.Name;

        for (int i = 0; i < _selectedLevel.EnemyLocations.Count; i++)
        {
            Vector3 pos = _selectedLevel.EnemyLocations[i];
            enemyPreviews[i].transform.localPosition = pos;
            enemyPreviews[i].SetActive(true);
        }
    }
}
