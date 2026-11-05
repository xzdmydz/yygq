using Cinemachine;
using UnityEngine;

public class trigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Component")]
    public Animator CharacterAni;
    public Rigidbody2D CharacterRig;
    public CinemachineVirtualCamera vcm;
    public GridManager gridManager;
    public stickman stickman;
    public Enemy attacker;


    private void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Yao")
        {
            CharacterAni.SetBool("ISJump", false);
            stickman.ISJump = false;
        }
        if (collision.tag == "CS")
        {
            stickman.Speed = 10.0f;
            stickman.ISJump = false;
            //CharacterRig.transform.position = new Vector3(-52, 63, 0);
        }

        if (collision.tag == "TaiJi")
        {
            CharacterRig.transform.position = new Vector3(0, 0, 0);
            CharacterRig.transform.rotation = Quaternion.Euler(0, 0, 0);
            //CharacterRig.transform.localScale = new Vector3(0, 0, 0);
            CharacterRig.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            vcm.m_Lens.OrthographicSize = 32.0f;
            vcm.transform.localScale = new Vector3(32, 32, 1);

            stickman.HPNow = 100;
            RespawnAllEnemies();

            //Tile.AddQi(3);
            //gridManager.UpdateGrid("111111111111111111111111111111111111111111111111111111111111111111111111111111111");

        }

        if(collision.tag == "weapon")
        {
            // 调整正交尺寸
            vcm.m_Lens.OrthographicSize = 5.0f; // 此值同样表示垂直可见范围的一半[3,8](@ref)
            vcm.transform.localScale = new Vector3(5, 5, 1);
            CharacterAni.SetBool("ISJump", false);
            stickman.ISJump = false;
        }

        if(collision .tag == "QiFei")
        {
            CharacterAni.SetBool("ISJump", false);
            stickman.JumpSpeed = 150f;
            stickman.ISJump = false;
        }

        if (collision.tag == "test")
        {
            CharacterRig.transform.position = new Vector3(-6, 128, 0);
            CharacterRig.transform.rotation = Quaternion.Euler(0, 0, 0);
            string presetData = "111111111111111111111111111111111111111111111111111111111111111111111111111111111";
            gridManager.UpdateGrid(presetData);
            // GridCell.LoadFromString(presetData);
        }

        if (collision.CompareTag("EnemyAttack"))
        {
            // 受击反馈
            CharacterAni.SetTrigger("GetHit");
            stickman.AudioCon.PlayOneShot(stickman.GetHitSound, SoundValue.Audio);
            stickman.CANAttack = false;
            stickman.Invoke("AttackWait", stickman.AttackTime);
            // 通过碰撞体找到实际攻击者的Enemy脚本
            Enemy attacker = collision.GetComponentInParent<Enemy>();
            if (attacker != null)
            {
                TakeDamage(attacker.AttackDamage); // 从攻击者实例获取实时伤害值
            }



        }

    }

    // 新建独立伤害处理方法
    public void TakeDamage(float damage)
    {
        stickman.HPNow -= damage;
        //Debug.Log($"来自 {attacker.name} 的伤害: {damage}");
        


    }
    public void DoHP()
    {
        stickman.HPNow = stickman.HPNow - attacker.AttackDamage;
        

    }


    void OnTriggerStay2D(Collider2D collision)
    {
       

        if (collision.tag == "Yao")
        {
            CharacterAni.SetBool("ISJump", false);
            stickman.ISJump = false;
        }

        if (collision.tag == "QiFei")
        {
            CharacterAni.SetBool("ISJump", false);
            stickman.JumpSpeed = 150f;
            stickman.ISJump = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Yao")
        {
            CharacterAni.SetBool("ISJump", true);
            CharacterAni.SetBool("ISRun", false);
            stickman.ISJump = true;
        }

        if (collision.tag == "TaiJi")
        {
            // 移除所有约束，恢复自由移动和旋转
            CharacterRig.constraints = RigidbodyConstraints2D.None;
            vcm.m_Lens.OrthographicSize = 1.0f;
            vcm.transform.localScale = new Vector3(1, 1, 1);
        }

        if (collision.tag == "CS")
        {
            stickman.Speed = 3.0f;
        }

        if (collision.tag == "weapon")
        {
            // 调整正交尺寸
            vcm.m_Lens.OrthographicSize = 1.0f; // 此值同样表示垂直可见范围的一半[3,8](@ref)
            stickman.transform.position = new Vector3(0, 0, 0);
        }

        if (collision.tag == "QiFei")
        {
            stickman.JumpSpeed = 15.0f;
        }
    }

    public  void RespawnAllEnemies()
    {
        // 查找所有敌人（包括被禁用的）
        Enemy[] enemies = FindObjectsOfType<Enemy>(true);

        foreach (Enemy enemy in enemies)
        {
            enemy.Respawn();
        }

        //Debug.Log($"已复活 {enemies.Length} 个敌人");
    }
}

