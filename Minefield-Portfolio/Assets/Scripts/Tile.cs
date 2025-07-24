using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int line;
    public int column;
    public List<Sprite> sprites;
    public List<Sprite> numbersSprites;
    private AudioSource audioSource;
    public List<AudioClip> sounds;
    public bool revealed;
    public bool visited;
    public bool hasMine;
    public bool markedWithFlag;
    [Range(0, 100)]
    public float probabilityBomb;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer numberSpriteRender;
    private Field field;
    private SceneController sceneController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    private void DrawBomb()
    {
        float random = Random.Range(0, 100);
        if (random < probabilityBomb)
        {
            hasMine = true;
        }
    }

    public void InitSetUp()
    {
        revealed = false;
        visited = false;
        hasMine = false;
        markedWithFlag = false;


        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, 5)];

        field = GameObject.Find("Field").GetComponent<Field>();

        DrawBomb();
    }

    public void ResetTile(int difficulty = 1)
    {
        revealed = false;
        visited = false;
        hasMine = false;
        markedWithFlag = false;

        spriteRenderer.sprite = sprites[Random.Range(0, 5)];
        numberSpriteRender.sprite = null;

        switch (difficulty)
        {
            case 1:
                probabilityBomb = 10.0f;
                break;

            case 2:
                probabilityBomb = 17.0f;
                break;

            case 3:
                probabilityBomb = 30.0f;
                break;

            default:
                break;
        }

        DrawBomb();
    }

    public void Show(int adjacentQuantity)
    {
        // numberSpriteRender.sprite = numbersSprites[adjacentQuantity-1];
        spriteRenderer.sprite = sprites[Random.Range(5, 8)];
        numberSpriteRender.sprite = adjacentQuantity == 0 ? null : numbersSprites[adjacentQuantity - 1];
        numberSpriteRender.transform.localScale = new(2, 2, 2);
        revealed = true;
    }

    private void MarkWithFlag()
    {
        if (!revealed)
        {
            markedWithFlag = true;
            numberSpriteRender.sprite = numbersSprites[8];
            numberSpriteRender.transform.localScale = new(1, 1, 1);
        }
    }

    private void RemoveFlag()
    {
        if (!revealed)
        {
            markedWithFlag = false;
            numberSpriteRender.sprite = null;
            numberSpriteRender.transform.localScale = new(2, 2, 2);
        }
    }

    void OnMouseDown()
    {
        if (!GetComponent<BoxCollider2D>().enabled) return;

        if (!field.isFlagSelect)
        {
            Debug.Log($"Line: {line} | Column: {column} | HasMine: {hasMine}");
            if (!revealed)
            {
                if (hasMine)
                {
                    sceneController.countingTime = false;
                    sceneController.GameOver(false);
                    StartCoroutine(field.RevealAllBombsEnum());

                }
                else
                {
                    audioSource.clip = sounds[0];
                    audioSource.pitch = 1;
                    audioSource.Play();

                    revealed = true;
                    field.SetAllTilesUnvisited();
                    field.RevealTilesUponClick(line, column);

                    sceneController.VerifyEndGame();
                }
            }
        }
        else
        {
            if (!revealed)
            {
                audioSource.clip = sounds[1];
                audioSource.pitch = 3;

                if (markedWithFlag)
                {
                    RemoveFlag();
                    audioSource.Play();
                    sceneController.RmvFlag();
                }
                else
                {
                    if (sceneController.flagCount <= 0) return;
                    MarkWithFlag();
                    audioSource.Play();
                    sceneController.AddFlag();
                }
            }
        }
    }

}
