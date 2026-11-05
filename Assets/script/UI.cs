using Cinemachine;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public stickman stickman;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI counterText; // 关联UI文本
    public CinemachineVirtualCamera vcm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counterText != null)
        {
            counterText.text =
     $"当前生命值: {stickman.HPNow}\n当前攻击力: {stickman.Attack}\n当前气: {Tile.qi}";
        }
        
    }

    public void zuo()
    {
        stickman.transform.position = new Vector3(6, 128, 0);
        // 调整正交尺寸
        vcm.m_Lens.OrthographicSize = 5.0f;
        vcm.transform.localScale = new Vector3(5, 5, 1);
    }

    public void run()
    {
        stickman.transform.position = new Vector3(0, 0, 0);
    }

}
