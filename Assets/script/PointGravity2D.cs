using UnityEngine;

public class PointGravity2D : MonoBehaviour
{
    [SerializeField] float gravityStrength = 9.8f; // 引力强度
    [SerializeField] float maxRadius = 100f;        // 作用范围

    // 计算引力方向与强度
    public Vector2 GetGravityAt(Vector2 position)
    {
        Vector2 direction = (Vector2)transform.position - position;
        float distance = direction.magnitude;
        if (distance > maxRadius) return Vector2.zero;
        return direction.normalized * gravityStrength;
    }

    void Start()
    {
        CustomGravity2D.RegisterSource(this); // 注册自身为全局重力源
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

}

public static class CustomGravity2D
{
    private static PointGravity2D mainSource; // 唯一重力源

    public static void RegisterSource(PointGravity2D source) => mainSource = source;
    public static Vector2 GetGravity(Vector2 position)
    {
        return mainSource != null ? mainSource.GetGravityAt(position) : Vector2.zero;
    }
}


