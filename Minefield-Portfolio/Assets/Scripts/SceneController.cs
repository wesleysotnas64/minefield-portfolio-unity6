using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public bool isGameOver;
    public bool countingTime;
    public int totalSeconds;
    public int flagCount;
    public TMP_Text textCurrentTime;
    public TMP_Text textFlagCount;
    public TMP_Text textEndGame;
    private Field field;
    public GameObject newGamePanel;
    public Image btnFlagImage;
    public List<Texture2D> cursorTextures; 
    public List<Sprite> btnFlagSprites;
    private Coroutine timeCoroutine;

    void Start()
    {
        field = GameObject.Find("Field").GetComponent<Field>();
        field.InitSetUp();
        field.ResetGame(1);
        flagCount = field.CountAllMines();
        textFlagCount.text = $"{flagCount}";

        timeCoroutine = StartCoroutine(CountTimeEnum());
        textEndGame.text = "score - game studio";
    }

    public IEnumerator CountTimeEnum()
    {
        countingTime = true;
        totalSeconds = 0;

        while (countingTime)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            // Formata os valores com dois dígitos (ex: 01:05)
            textCurrentTime.text = $"{minutes:D2}:{seconds:D2}";

            yield return new WaitForSeconds(1.0f);
            totalSeconds++;
        }
    }

    public void GameOver(bool win = true)
    {
        isGameOver = true;
        if (win)
        {
            string msg = "you win!";
            if (totalSeconds < PlayerPrefs.GetInt("TotalSeconds"))
            {
                PlayerPrefs.SetInt("TotalSeconds", totalSeconds);
                msg += " new record";
            }
            textEndGame.text = msg;
            field.DisableAllBoxColliders();
            GetComponent<AudioSource>().Play();
            countingTime = false;
        }
        else
        {
            textEndGame.text = "you loose!";
        }
    }

    public void VerifyEndGame()
    {
        int totalTiles = field.width * field.height;

        if (field.CountAllRevealedTiles() == totalTiles - field.CountAllMines()) GameOver();
    }

    public void AddFlag()
    {
        flagCount--;
        textFlagCount.text = $"{flagCount}";
    }

    public void RmvFlag()
    {
        flagCount++;
        textFlagCount.text = $"{flagCount}";
    }

    public void ActiveFlag()
    {
        field.isFlagSelect = !field.isFlagSelect;
        if (field.isFlagSelect)
        {
            btnFlagImage.GetComponent<Image>().sprite = btnFlagSprites[1];
            Cursor.SetCursor(
                cursorTextures[1],
                new(cursorTextures[1].width / 2, cursorTextures[1].height / 2),
                CursorMode.Auto
            );
        }
        else
        {
            btnFlagImage.GetComponent<Image>().sprite = btnFlagSprites[0];
            Cursor.SetCursor(cursorTextures[0], new(0, 0), CursorMode.Auto);
        }
    }

    public void ResetGame(int difficulty)
    {
        isGameOver = false;
        //Aqui tenho que parar aquela coroutine
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }

        field.ResetGame(difficulty);
        field.DestroyAllMines();
        DisableNewGamePanel();

        timeCoroutine = StartCoroutine(CountTimeEnum());
        flagCount = field.CountAllMines();
        textFlagCount.text = $"{flagCount}";
        textEndGame.text = "score - game studio";
    }

    public void ActiveNewGamePanel()
    {
        if (field.revealingMines) return;

        newGamePanel.SetActive(true);
        field.DisableAllBoxColliders();
    }

    public void DisableNewGamePanel()
    {
        newGamePanel.SetActive(false);
        if (!isGameOver) field.EnableAllBoxColliders();
    }
}
