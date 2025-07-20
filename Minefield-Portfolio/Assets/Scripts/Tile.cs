using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int line;
    public int column;
    public List<Sprite> sprites;
    public bool revealed;
    public bool visited;
    public bool hasMine;
    public bool markedWithFlag;
    [Range(0, 100)]
    public float probabilityBomb;
    private SpriteRenderer spriteRenderer;
    private Field field;

    void Start()
    {
        
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
        spriteRenderer.sprite = sprites[9];

        field = GameObject.Find("Field").GetComponent<Field>();

        DrawBomb();
    }

    public void ResetTile(int difficulty = 1)
    {
        revealed = false;
        visited = false;
        hasMine = false;
        markedWithFlag = false;

        spriteRenderer.sprite = sprites[9];

        switch (difficulty)
        {
            case 1:
                probabilityBomb = 10.0f;
                break;

            case 2:
                probabilityBomb = 25.0f;
                break;

            case 3:
                probabilityBomb = 65.0f;
                break;

            default:
                break;
        }

        DrawBomb();
    }

    public void Show(int adjacentQuantity)
    {
        spriteRenderer.sprite = sprites[adjacentQuantity];
        revealed = true;
    }

    private void MarkWithFlag()
    {
        if (!revealed)
        {
            markedWithFlag = true;
            spriteRenderer.sprite = sprites[10];
        }
    }

    private void RemoveFlag()
    {
        if (!revealed)
        {
            markedWithFlag = false;
            spriteRenderer.sprite = sprites[9];
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
                    StartCoroutine(field.RevealAllBombsEnum());
                }
                else
                {
                    revealed = true;
                    field.SetAllTilesUnvisited();
                    field.RevealTilesUponClick(line, column);
                }
            }
        }
        else
        {
            if (!revealed)
            {
                if (markedWithFlag) RemoveFlag();
                else MarkWithFlag();
            }
        }
    }

}
