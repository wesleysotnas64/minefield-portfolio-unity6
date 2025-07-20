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
    [Range(0, 100)]
    public float probabilityBomb;
    private SpriteRenderer spriteRenderer;
    private Field field;

    void Start()
    {
        revealed = false;
        visited = false;
        hasMine = false;


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

    public void Mark()
    {
        if (!revealed)
        {
            spriteRenderer.sprite = sprites[10];
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Line: {line} | Column: {column}");
            if (!revealed)
            {
                field.VerifyAdjascenceFrom(line, column);
            }
            
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Mark();
        }
    }

}
