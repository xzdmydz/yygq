using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using TMPro; // 添加命名空间


public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Location")]
    public GameObject LocationCenter;
    public GameObject LocationLeft;
    public GameObject LocationRight;
    public GameObject LocationTarget;
    public float EnemyDistance = 0;
    public float LeftDistance = 0;
    public float RightDistance = 0;
    public float Speed = 1.0f;

    [Header("伤害显示")]
    public TextMeshProUGUI counterText; // 关联UI文本

    [Header("Component")]
    public Animator EnemyAni;
    public GameObject EnemyObjS;
    public GameObject EnemyModle;
    public Rigidbody2D EnemyRig;
    public stickman stickman;
    public AudioSource AudioCon;

    private GameObject Player;
    public bool CanAttack = true;
    private bool ISDead = false;
    public AudioClip DeadSound;
    public bool ISAttack = false;
    public bool ISMove = true;
    public float Attack = 4.0f;
    public float AttackTime = 3.0f;
    public AudioClip Attack1Sound;
    public AudioClip Attack2Sound;
    public float GetHitTime = 1.0f;
    public AudioClip GetHitSound;
    public float HP = 100;
    public float HPNow = 100;
    public int qi = 1;
    public float AttackDamage;
    private float _currentCooldown;
    private bool _isInAttackAnimation;
    private Coroutine _attackCoroutine;
    // 新增变量：记录初始位置和血量
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private float _initialHP;


    void Start()
    {
        // 记录初始状态
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _initialHP = HP;
        HPNow = HP;
        Player = GameObject.FindWithTag("Player");
        if (Player == null)
        {
            Debug.LogWarning("Enemy: 场景中找不到 Tag 为 Player 的对象", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Modle();
        if (ISDead) return; // 死亡后禁用所有逻辑

        // 玩家可能尚未激活，找不到时每帧重试查找，避免空引用
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
            if (Player == null) return;
        }
        Distance();

        if (EnemyDistance <= 2)
        {
            Moveto();
        }
        else
        {
            EnemyAni.SetBool("ISRun", false);
        }
        if (EnemyDistance <= 0.4)
        {
            if(CanAttack == true)
            {
                RandomAttack();
            }

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack")&& ISDead == false)
        {

            DoHP();
            

        }
    }

    // 启动协程
    public void DisableWithDelay()
    {
        StartCoroutine(DisableAfterDelay(1.5f));
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        EnemyAni.SetTrigger("ISDead");
        AudioCon.PlayOneShot(DeadSound, SoundValue.Audio);
        yield return new WaitForSeconds(delay); // 等待1秒[6](@ref)
        gameObject.SetActive(false);           // 禁用GameObject[2,5](@ref)
        Tile.AddQi(qi);
    }
    public void Modle()
    {
        Vector2 gravityDir = CustomGravity2D.GetGravity(EnemyRig.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.down, gravityDir);
        //CharacterModel.transform.rotation = Quaternion.Euler(0, 0, angle);
        EnemyRig.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (LeftDistance > RightDistance)
        {
            EnemyModle.transform.localScale = new Vector3Int(1, 1, 1);
        }
        else if (LeftDistance < RightDistance)
        {
            EnemyModle.transform.localScale = new Vector3Int(-1, 1, 1);
        }
    }

    public void Distance()
    {
        EnemyDistance = Vector2.Distance(transform.position, Player.transform.position);

        LeftDistance = Vector2.Distance(LocationLeft.transform.position, Player.transform.position);

        RightDistance = Vector2.Distance(LocationRight.transform.position, Player.transform.position);

    }

    public void Moveto()
    {
        // 获取重力方向并旋转角色
        Vector2 gravityDir = CustomGravity2D.GetGravity(EnemyRig.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.down, gravityDir);
        EnemyRig.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (ISMove)
        {
            EnemyAni.SetBool("ISRun", true);

            // 将玩家位置转换到本地坐标系
            Vector3 localPlayerPos = transform.InverseTransformPoint(Player.transform.position);

            // 计算本地移动方向（忽略Y轴变化）
            Vector3 localDirection = new Vector3(localPlayerPos.x, 0, 0).normalized;

            // 转换为本地移动向量
            Vector3 localMovement = localDirection * Speed * Time.deltaTime;

            // 应用本地移动（自动处理坐标系转换）
            transform.Translate(localMovement, Space.Self);

            /* （备选方案：显式计算世界坐标移动）
            // 将本地方向转换为世界方向
            Vector3 worldDirection = transform.TransformDirection(localDirection);
            // 应用移动
            transform.position += worldDirection * Speed * Time.deltaTime;
            */



        }
    }


    // 新增随机攻击方法
    // 使用整型随机避免浮点误差
    private void RandomAttack()
    {
        int rand = Random.Range(0, 100);
        if (rand < 60) // 60% Attack1
        {
            Attack1();
        }
        else // 40% Attack2
        {
            Attack2();
        }
    }

    // 添加动画事件回调
    public void OnAttackAnimationEnd()
    {
        _isInAttackAnimation = false;
    }
    public void Attack1()
    {
        if (CanAttack && !_isInAttackAnimation)
        {
            _isInAttackAnimation = true;
            EnemyAni.SetTrigger("Attack1");
            CanAttack = false;
            ISAttack = true;
            ISMove = false;
            AttackDamage = Attack;
            _currentCooldown = AttackTime;
            Invoke("AttackWait", _currentCooldown);
            Invoke("MoveWait", GetHitTime);
            AudioCon.PlayOneShot(Attack1Sound, SoundValue.Audio);
        }
        
    }

    public void Attack2()
    {
        if (CanAttack && !_isInAttackAnimation)
        {
            _isInAttackAnimation = true;
            EnemyAni.SetTrigger("Attack2");
            CanAttack = false;
            ISAttack = true;
            ISMove = false;
            AttackDamage = 2 * Attack;
            _currentCooldown = 2 * AttackTime;
            Invoke("AttackWait", _currentCooldown);
            Invoke("MoveWait", GetHitTime);
            AudioCon.PlayOneShot(Attack2Sound, SoundValue.Audio);
        }
            
    }

    public void DoHP()
    {
        float damage = stickman.AttackDamage;
        HPNow = HPNow - stickman.AttackDamage;
        //Debug.Log($"敌人现在血量是：{HPNow}");
        if (HPNow <= 0)
        {

            ISDead = true;
            ISAttack = false;
            ISMove = false;
            CanAttack = false;


            //Destroy(EnemyObjS,2.0f);
            // 改为禁用而非销毁
            DisableWithDelay(); // 禁用整个敌人对象
            
        }
        else
        {
            EnemyAni.SetTrigger("GetHit");
            AudioCon.PlayOneShot(GetHitSound, SoundValue.Audio);
            EnemyAni.SetBool("ISRun", false);
            ISMove = false;
            CanAttack = false;
            Invoke("MoveWait", GetHitTime);
            //Debug.Log($"敌人被打了现在血量是{HPNow}");
        }

        

        //Debug.Log($"现在血量是：{HPNow}");

    }

    public void Respawn()
    {
        // 重置状态
        HPNow = _initialHP;
        ISDead = false;
        ISAttack = false;
        ISMove = true;
        CanAttack = true;

       

        // 重置位置和旋转
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;

        // 重置动画
        EnemyAni.Rebind(); // 重置所有动画参数
        EnemyAni.Update(0f);

        // 重新激活对象
        gameObject.SetActive(true);
    }

 

    public void AttackWait()
    {
        CanAttack = true;
        ISAttack = false;
        OnAttackAnimationEnd();
    }

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        CanAttack = true;
        ISAttack = false;
    }

    public void MoveWait()
    {
        ISMove = true;
    }
}
