using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GridManager gridManager;
    public InputField inputField;

    public void OnUpdateButtonClick()
    {
        gridManager.UpdateGrid(inputField.text);
    }

    public void jiantou()
    {
        gridManager.UpdateGrid("000010000000001000000000100000000010111111101000000010000000100000001000000010000");
    }
    public void futou()
    {
        gridManager.UpdateGrid("000000000000000000000000110000001111111111111000000001000000000000000000000000000");
    }
    public void liandao()
    {
        gridManager.UpdateGrid("000001000000000100000000010000000101111111111000000001000000001000000000000000000");
    }
    public void chazi()
    {
        gridManager.UpdateGrid("000000000000000000000000111000000100111111110000000100000000111000000000000000000");
    }

    public void feibiao()
    {
        gridManager.UpdateGrid("000000000000011000000010000010010000011101110000010010000010000000110000000000000");
    }
    public void chuizi()
    {
        gridManager.UpdateGrid("000000000000000000000000011000000011011111111000000011000000011000000000000000000");
    }
    public void fazhang()
    {
        gridManager.UpdateGrid("000000000000000000000001100000010010111111101000010010000001100000000000000000000");
    }
    public void chilun()
    {
        gridManager.UpdateGrid("000000000000000000000000000010101010111111111001010100000000000000000000000000000");
    }
}