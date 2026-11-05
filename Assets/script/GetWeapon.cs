using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public GameObject button;
    public GameObject BUTTON;
    public Enemy Boss;
    public static int weapon = 0; // 已获得的武器数（八个卦位实例共用，集齐8件触发最终按钮）
    private bool counted = false; // 当前Boss是否已计入（防止Boss死亡后每帧重复计数）
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Boss != null && Boss.HPNow <= 0)
        {
            if (button != null) button.SetActive(true);
            if (!counted)
            {
                counted = true;
                weapon++;
            }
        }
        if (weapon >= 8 && BUTTON != null)
        {
            BUTTON.SetActive(true);
        }
    }
}
