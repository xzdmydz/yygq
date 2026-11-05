using TMPro;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Enemy Enemy;
    public TextMeshProUGUI counterText; // 关联UI文本
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counterText != null)
        {
            counterText.text =
     $"生命值: {Enemy.HPNow}\n攻击力: {Enemy.Attack}";
        }
    }
}
