using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform target; // 需要跟踪的对象
    public Camera miniMapCamera;

    void Update()
    {
        miniMapCamera.transform.position = new Vector3(
            target.position.x,
            target.position.y,
            -10 // 保持正交摄像机Z轴距离
        );
    }

    void Start()
    {
        
    }

    // Update is called once per frame

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 标记当前对象及其子对象为跨场景保留
    }

}
