using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private Image[] goalUI; 
    [SerializeField]
    private List<Sprite> goalSprite = new List<Sprite>();
    [SerializeField]
    private List<int> goalCounter = new List<int>();
    [SerializeField]
    private Sprite checkMark;

    private GameMode gameMode;


    private void Awake()
    {
        gameMode = FindObjectOfType<GameMode>();
        winPanel.SetActive(false);
        SetSprite();
    }
    void Update()
    {
        for (int i = 0; i < goalCounter.Count; i++)
        {
            goalUI[i].gameObject.GetComponentInChildren<Text>().text = string.Format("{0}", goalCounter[i] <= 0 ? 0 : goalCounter[i]);
        }

        int c = 0;

        for (int i = 0; i < goalCounter.Count; i++)
        {
            if (goalCounter[i] <= 0)
            {
                c++;
                goalUI[i].sprite = checkMark;
               // Debug.Log("C is: " + c);
                //Debug.Log("Goal lis count is: " + goalUI[i]);
            }
        }
        if (c == goalCounter.Count && !gameMode.IsShifting)
        {
            winPanel.SetActive(true);
        }

    }

    private void SetSprite()
    {
        for (int i = 0; i < goalUI.Length; i++)
        {
            goalUI[i].enabled = false;
            goalUI[i].gameObject.GetComponentInChildren<Text>().enabled = false;
        }

        for (int i = 0; i < goalSprite.Count; i++)
        {
            goalUI[i].GetComponent<Image>().sprite = goalSprite[i];
            goalUI[i].gameObject.GetComponentInChildren<Text>().enabled = true;
            goalUI[i].enabled = true;
            
        }
    }

    public void EvaluationOfGoal(Sprite sprite)
    {
        if (goalCounter == null)
        {
            return;
        }
        for (int i = 0; i < goalSprite.Count; i++)
        {
            if (goalSprite[i] == sprite)
            {
                goalCounter[i] -= 1;
            }

        }
        
    }


}
