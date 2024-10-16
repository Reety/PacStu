using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    private GameManager currentGame;
    public TMP_Text highscore_score;
    public TMP_Text highscore_time;

    public Button Level1Button;
    private bool changingScene = false;

    public Animator ButtonAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHighscore(int score, string time)
    {
        highscore_score.text = $"{score}";
        highscore_time.text = time;

    }

    public void Initialise(GameManager game)
    {
        currentGame = game;
        ButtonAnimator = Level1Button.GetComponent<Animator>();
    }

    public void OnClick(string scene)
    {
        if (!changingScene)
        {
            StartCoroutine(ButtonPressed(scene));
        }
        
    }

    IEnumerator ButtonPressed(string scene)
    {
        changingScene = true;
        AnimatorStateInfo currState = ButtonAnimator.GetCurrentAnimatorStateInfo(0);
        //AnimatorClipInfo currClip = ButtonAnimator.GetCurrentAnimatorClipInfo(0)[0];
        //print($"currstate is {currClip.clip}");
        yield return new WaitForSeconds(currState.length);
        AnimatorStateInfo newState = ButtonAnimator.GetCurrentAnimatorStateInfo(0);
        //AnimatorClipInfo newClip = ButtonAnimator.GetCurrentAnimatorClipInfo(0)[0];
        //print($"nextstate is {newClip.clip}");
        //float duration = (float)newState.length - (float)(newState.length * (Math.Truncate(newState.normalizedTime) - newState.normalizedTime));
        
        yield return new WaitForSeconds(newState.length);
        changingScene = false;
        currentGame.LoadScene(scene);
    }
}
