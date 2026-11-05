using UnityEngine;

public class GravityBody2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject CharacterModel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       // rb.gravityScale = 0; // 关闭默认重力
    }

    void FixedUpdate()
    {
        Vector2 gravityDir = CustomGravity2D.GetGravity(rb.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.down, gravityDir);
        
        Vector2 gravity = CustomGravity2D.GetGravity(rb.position);
        rb.AddForce(gravity * rb.mass);
        
    }

}



