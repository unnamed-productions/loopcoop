using System.Collections;
using UnityEngine;
using TMPro;
using System;


public class ScorePopup : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreFlavor;

    public float animationTime = 1f;
    public AnimationCurve scaleCurve;

    String GetFlavorText(int score)
    {

        if (score > 500)
        {
            return "Looping Legend!!";
        }
        else if (score > 300)
        {
            return "Cattastic!";
        }
        else if (score > 100)
        {
            return "Looking Cool!";
        }
        else
        {
            return "Meh";
        }
    }


    String GetScoreText(int score)
    {
        return "+" + score.ToString();
    }


    IEnumerator PopUp()
    {
        Vector3 initScale = transform.localScale;
       
      

        float elapsed = 0f;
        while (elapsed < animationTime)
        {

            transform.localScale = new(initScale.x,
                initScale.y * scaleCurve.Evaluate(elapsed / animationTime), 1);

            elapsed += Time.deltaTime;
            yield return null;

        }
      
        transform.localScale = initScale;
        scoreText.enabled = false;
        scoreFlavor.enabled = false;
        yield return null;
    }


    public void PopUpScore(int score)
    {
        scoreFlavor.SetText(GetFlavorText(score));
        scoreText.SetText(GetScoreText(score));
        scoreText.enabled = true;
        scoreFlavor.enabled = true;
        StartCoroutine(PopUp());

    }

  void Start()
  {
    scoreText.enabled = false;
    scoreFlavor.enabled = false;
  }

  void Update()
  {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PopUpScore(500);
        }   
  }



}

