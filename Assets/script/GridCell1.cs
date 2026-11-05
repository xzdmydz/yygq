// GridCell.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridCell1 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int gridIndex; // 0-80
    private Image image;
    private bool currentState;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = Color.clear; // ³õÊ¼Í¸Ã÷[1](@ref)
    }

    public void OnPointerClick(PointerEventData eventData) => ToggleState();

    public void ToggleState()
    {
        currentState = !currentState;
        image.color = currentState ? Color.black : Color.clear;
    }

    public void SetState(bool state)
    {
        currentState = state;
        image.color = currentState ? Color.black : Color.clear;
    }

    public void SetGridIndex(int index) => gridIndex = index;
}