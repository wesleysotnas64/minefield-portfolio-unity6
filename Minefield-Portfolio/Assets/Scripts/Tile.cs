using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    public int line;
    public int column;
    public List<Sprite> sprites;
    public bool revealed;
    public bool visited;
    public bool hasMine;
    public bool marked;
    [Range(0, 100)]
    public float probabilityBomb;
    private SpriteRenderer spriteRenderer;
    private Field field;

    void Start()
    {
        revealed = false;
        visited = false;
        hasMine = false;
        marked = false;


        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[9];

        field = GameObject.Find("Field").GetComponent<Field>();

        DrawBomb();
    }

    private void DrawBomb()
    {
        float random = Random.Range(0, 100);
        if (random < probabilityBomb)
        {
            hasMine = true;
        }
    }

    public void Show(int adjacentQuantity)
    {
        spriteRenderer.sprite = sprites[adjacentQuantity];
        revealed = true;
    }

    private void Mark()
    {
        if (!revealed)
        {
            spriteRenderer.sprite = sprites[10];
        }
    }

    void OnMouseDown()
    { 
        if (!field.isFlagSelect)
        {
            Debug.Log($"Line: {line} | Column: {column} | HasMine: {hasMine}");
            if (!revealed)
            {
                if (hasMine)
                {
                    // Revela a bomba
                    // Chama o campo para revelar todas as bombas e determinar fim de jogo.
                }
                else
                {
                    revealed = true;
                    field.SetAllTilesUnvisited();
                    field.RevealTilesUponClick(line, column);
                    // Show(field.VerifyAdjascenceFrom(line, column));
                }
            }
        }
        else
        {
            Mark();
        }
    }

}
