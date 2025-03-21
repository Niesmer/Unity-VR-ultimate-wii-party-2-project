using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Pieces = 0;
    public TMP_Text PiecesText;

    void Start()
    {
        PiecesText.text = Pieces.ToString();
    }

    public void CalculatePieces(int score)
    {
        Pieces += score / 10;
        PiecesText.text = Pieces.ToString();
    }
}