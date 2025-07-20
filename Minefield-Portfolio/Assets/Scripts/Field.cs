using UnityEngine;

public class Field : MonoBehaviour
{

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
        GameObject.Find("Main Camera").transform.position = new(newPositionX,-newPositionY, -10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitAllTiles()
    {
        for (int line = 0; line < height; line++)
        {
            for (int column = 0; column < width; column++)
            {
                GameObject tileInstance = Instantiate(tileGameObject);

                tileInstance.transform.parent = transform;
                tileInstance.transform.position = Vector3.zero + new Vector3(column*gapTiles, line*(-gapTiles), 0);

                tiles[line, column] = tileInstance.GetComponent<Tile>();
                tiles[line, column].line = line;
                tiles[line, column].column = column;
            }
        }
    }

    public void VerifyAdjascenceFrom(int line, int column)
    {

    }
}
