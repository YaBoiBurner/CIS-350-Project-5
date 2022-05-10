/*
 * Jaden Pleasants, Gerard Lamoureux
 * Project 5
 * Handles Scene management, scores
 */
using System;
using System.Collections;
using System.Collections.Generic;
using SubmarineBigfish;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Stack<string> levelStack = new Stack<string>();

    public int humanHealth = 5;

    public int score = 0;

    public int level = 0;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject levelOneButton;
    [SerializeField] private GameObject levelTwoButton;
    [SerializeField] private GameObject levelThreeButton;

    public void LoadLevel(string name)
    {
        var ao = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError($"[GameManager] Unable to load level {name}");
            return;
        }
        levelStack.Push(name);
    }

    public void UnloadCurrentLevel()
    {
        SceneManager.UnloadSceneAsync(levelStack.Pop());
        DeleteClones();
    }

    public string GetCurrentLevel() => levelStack.Peek();

    public void BossLevelGameOver(bool win)
    {
        gameOverUI.SetActive(true);
        levelOneButton.SetActive(true);
        levelTwoButton.SetActive(false);
        levelThreeButton.SetActive(false);
        humanHealth = 5;
        score = 0;
        Pause();
        if(win)
        {
            gameOverUI.transform.Find("Panel").gameObject.transform.Find("GameOverText").gameObject.GetComponent<Text>().text =
                "You Win!\nYou Killed Big Covid!";
        }
        else
        {
            gameOverUI.transform.Find("Panel").gameObject.transform.Find("GameOverText").gameObject.GetComponent<Text>().text =
                "You Lose!\nRemember to Shoot the Covid Cells!";
        }
    }

    public void LevelOneGameOver()
    {
        gameOverUI.SetActive(true);
        levelOneButton.SetActive(false);
        if(level == 1)
        {
            levelTwoButton.SetActive(true);
            levelThreeButton.SetActive(false);
        }
        else
        {
            levelTwoButton.SetActive(false);
            levelThreeButton.SetActive(true);
        }
        humanHealth = 5;
        Pause();
        gameOverUI.transform.Find("Panel").gameObject.transform.Find("GameOverText").gameObject.GetComponent<Text>().text =
                "Game Over!\nYou killed " + score + " Cells!";
        score = 0;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        SceneManager.UnloadSceneAsync(levelStack.Peek());
        DeleteClones();
        LoadLevel(levelStack.Peek());
    }

    public void DeleteClones()
    {
        var clones = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var clone in clones)
        {
            if (clone.name == "Covid cell(Clone)")
                Destroy(clone);
        }
    }

    public void SetLevel(int i)
    {
        level = i;
    }

}
