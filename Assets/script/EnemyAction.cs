using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyAction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Character Enemy;
    public float Speed = 3.0f;
    public GameObject Player;
    void Start()
    {
        Follow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Follow()
    {
        Vector2 position = transform.position;
        if (Mathf.Abs(gameObject.transform.position.x - Player.transform.position.x) > 5)//怪外和角色距离大于某个值时才开始朝玩家移动,这样避免了怪物与玩家冲重合
        {
            if (gameObject.transform.position.x < Player.transform.position.x)
            {
                position.x += Speed;
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);//怪物转向
            }
            else if (gameObject.transform.position.x > Player.transform.position.x)
            {
                position.x -= Speed;
                gameObject.transform.localRotation = Quaternion.Euler(0, -180, 0);//怪物转向
            }
            gameObject.transform.position = position;
        }
    }
                        
}
