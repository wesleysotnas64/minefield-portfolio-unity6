using System.Collections;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isFlagSelect;
    public int width;
    public int height;
    public float gapTiles;
    public bool revealingMines;
    private Tile[,] tiles;
    public GameObject tileGameObject;
    public GameObject mineGameObject;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

                tiles[line, column].InitSetUp();
            }
        }
    }

    public int CountAllMines()
    {
        int count = 0;
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                if (tiles[line, column].hasMine) count++;
            }
        }
        return count;
    }

    public int CountAllRevealedTiles()
    {
        int count = 0;
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                if (tiles[line, column].revealed) count++;
            }
        }
        return count;
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

    public IEnumerator RevealAllBombsEnum()
    {
        revealingMines = true;
        DisableAllBoxColliders();
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                if (tiles[line, column].hasMine)
                {
                    yield return new WaitForSeconds(0.05f);
                    GameObject mineInstance = Instantiate(mineGameObject);
                    mineInstance.transform.position = tiles[line, column].transform.position;
                }

            }
        }
        audioSource.Play();
        revealingMines = false;
    }

    public void DestroyAllMines()
    {
        GameObject[] mines = GameObject.FindGameObjectsWithTag("Mine");

        foreach (GameObject mine in mines)
        {
            Destroy(mine);
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

    public void InitSetUp()
    {
        tiles = new Tile[height, width];
        InitAllTiles();

        float newPositionX = gapTiles * ((float)width - 1) / 2;
        // float newPositionY = gapTiles * ((float)height - 1) / 2;
        float newPositionY = 12.5f;
        GameObject.Find("Main Camera").transform.position = new(newPositionX, -newPositionY, -10);
    }

    public void ResetGame(int difficulty)
    {
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                tiles[line, column].ResetTile(difficulty);
            }
        }
        isFlagSelect = false;
        GameObject.Find("SceneController").GetComponent<SceneController>().DisableNewGamePanel();
    }

    public void DisableAllBoxColliders()
    {
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                tiles[line, column].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    public void EnableAllBoxColliders()
    {
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                tiles[line, column].gameObject.GetComponent<BoxCollider2D>().enabled = true;
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
