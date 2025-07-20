using UnityEngine;

[ExecuteInEditMode]
public class Field : MonoBehaviour
{
    public bool isFlagSelect;
    public int width;
    public int height;
    public float gapTiles;
    private Tile[,] tiles;
    public GameObject tileGameObject;

    void Start()
    {
        tiles = new Tile[height, width];
        InitAllTiles();

        float newPositionX = gapTiles * ((float)width - 1) / 2;
        float newPositionY = gapTiles * ((float)height - 1) / 2;
        GameObject.Find("Main Camera").transform.position = new(newPositionX, -newPositionY, -10);
    }

    private void InitAllTiles()
    {
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                GameObject tileInstance = Instantiate(tileGameObject);

                tileInstance.transform.parent = transform;
                tileInstance.transform.position = Vector3.zero + new Vector3(column * gapTiles, line * (-gapTiles), 0);

                tiles[line, column] = tileInstance.GetComponent<Tile>();
                tiles[line, column].line = line;
                tiles[line, column].column = column;
            }
        }
    }

    public void RevealTilesUponClick(int line, int column)
    {
        if (IsOutOfRange(line, column)) return;
        if (tiles[line, column].visited) return;

        tiles[line, column].visited = true;

        int adjacentMines = VerifyAdjascenceFrom(line, column);
        tiles[line, column].Show(adjacentMines);

        if (tiles[line, column].hasMine || adjacentMines > 0) return;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dy == 0 && dx == 0) continue;
                RevealTilesUponClick(line + dy, column + dx);
            }
        }
    }

    public void SetAllTilesUnvisited()
    {
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                tiles[line, column].visited = false;
            }
        }
    }

    public int VerifyAdjascenceFrom(int line, int column)
    {
        int count = 0;
        if (!IsOutOfRange(line - 1, column - 1))
        {
            if (tiles[line - 1, column - 1].hasMine) count++; //Superior esquerdo
        }

        if (!IsOutOfRange(line - 1, column))
        {
            if (tiles[line - 1, column].hasMine) count++; //Superior
        }

        if (!IsOutOfRange(line - 1, column + 1))
        {
            if (tiles[line - 1, column + 1].hasMine) count++; //Superior direito
        }

        if (!IsOutOfRange(line, column - 1))
        {
            if (tiles[line, column - 1].hasMine) count++; //Esquerdo
        }

        if (!IsOutOfRange(line, column + 1))
        {
            if (tiles[line, column + 1].hasMine) count++; //Direito
        }

        if (!IsOutOfRange(line + 1, column - 1))
        {
            if (tiles[line + 1, column - 1].hasMine) count++; //Inferior esquerdo
        }

        if (!IsOutOfRange(line + 1, column))
        {
            if (tiles[line + 1, column].hasMine) count++; //Inferior
        }

        if (!IsOutOfRange(line + 1, column + 1))
        {
            if (tiles[line + 1, column + 1].hasMine) count++; //inferior direito
        }

        return count;
    }

    private bool IsOutOfRange(int line, int column)
    {
        if (line < 0 ||
            line >= height ||
            column < 0 ||
            column >= width)
        {
            return true;
        }

        return false;
    }
}
