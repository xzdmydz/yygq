using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;
    private int state = 0;

    public static int qi = 0;
    private const int MIN_QI = 0;
    private const int QI_CHANGE_AMOUNT = 1;

    public System.Action<Tile> OnStateChanged;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        UpdateComponents();
    }

    void OnMouseDown()
    {
        if (CanInteract())
        {
            ToggleState();
        }
    }

    public void SetState(int newState)
    {
        state = Mathf.Clamp(newState, 0, 1);
        UpdateComponents();
        OnStateChanged?.Invoke(this);
    }

    public int GetState() => state;
    private void ToggleState()
    {
        int oldState = state;
        state = 1 - state;

        qi += (state == 1) ? -QI_CHANGE_AMOUNT : QI_CHANGE_AMOUNT;
        qi = Mathf.Max(qi, MIN_QI);

        UpdateComponents();
        OnStateChanged?.Invoke(this);
    }

    private bool CanInteract()
    {
        return (state == 0 && qi > MIN_QI) || state == 1;
    }

    private void UpdateComponents()
    {
        // 更新颜色和碰撞器触发状态
        sr.color = new Color(0, 0, 0, state);

        if (col != null)
        {
            // 修改为切换isTrigger而不是启用/禁用碰撞器
            col.isTrigger = state != 1;

            // 保持碰撞器始终启用
            if (!col.enabled) col.enabled = true;
        }
    }

    public static void AddQi(int amount) => qi = Mathf.Max(qi + amount, MIN_QI);
    public static void ResetQi() => qi = MIN_QI;
}