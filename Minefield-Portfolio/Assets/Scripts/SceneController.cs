using UnityEngine;

public class SceneController : MonoBehaviour
{
    private Field field;
    public GameObject newGamePanel;

    void Start()
    {
        field = GameObject.Find("Field").GetComponent<Field>();
    }

    public void ActiveFlag()
    {
        field.isFlagSelect = !field.isFlagSelect;
    }

    public void ResetGame(int difficulty)
    {
        field.ResetGame(difficulty);
        field.DestroyAllMines();
        DisableNewGamePanel();
        
    }

    public void ActiveNewGamePanel()
    {
        newGamePanel.SetActive(true);
        field.DisableAllBoxColliders();
    }

    public void DisableNewGamePanel()
    {
        newGamePanel.SetActive(false);
        field.EnableAllBoxColliders();
    }
}
