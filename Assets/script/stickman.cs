using UnityEngine;
using System.Collections;

public class stickman : MonoBehaviour
{
    [Header("Character")]
    public float Speed = 3.0f;
    public float JumpSpeed = 15.0f;
    public float AttackTime = 0.5f;
    public float GetHitTime = 0.3f;
    public int attackMultiplier = 1; // 攻击倍率系数
    private int baseAttack = 0; // 基础攻击力
    public float Attack = 0;
    public float AttackDamage;
    public float HP = 100.0f;
    public float HPNow = 100.0f;

    [Header("Component")]
    public Rigidbody2D CharacterRig;
    public Animator CharacterAni;
    public GameObject CharacterModel;
    public AudioSource AudioCon;
    public bool ISRun = false;
    public bool ISJump = false;
    public bool ISAttack = false;
    public bool ISDead = false;
    public AudioClip Attack1Sound;
    public AudioClip Attack2Sound;
    public bool CANAttack = true;
    public AudioClip GetHitSound;
    public AudioClip DeadSound;
    public bool cs = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPNow = HP;
    }

    void OnEnable()
    {
        GridManager.OnBlackTileCountChanged += HandleAttackUpdate;
    }

    void OnDisable()
    {
        GridManager.OnBlackTileCountChanged -= HandleAttackUpdate;
    }

    public void HandleAttackUpdate(int blackCount)
    {
        int finalAttack = baseAttack + (blackCount * attackMultiplier);
        //Debug.Log($"当前攻击力: {finalAttack}");

        // 更新角色属性
        Attack = finalAttack;
    }
    // Update is called once per frame

 
    void Update()
    {
        Move();
        Jump();
        Attack1();
        Attack2();
    }

    void FixedUpdate()
    {
        if (HPNow <= 0 && !ISDead)
        {

            ISDead = true;
            ISAttack = false;
            ISRun = false;
            CANAttack = false;
            
            //Destroy(EnemyObjS,2.0f);
            // 改为禁用而非销毁
            // 延迟传送
            StartCoroutine(RespawnAfterDeath());// 回家
        }
    }

    IEnumerator RespawnAfterDeath()
    {
        CharacterAni.SetTrigger("ISDead");
        AudioCon.PlayOneShot(DeadSound, SoundValue.Audio);
        // 等待动画播放时间（根据实际动画长度调整）
        yield return new WaitForSeconds(1.2f);
        transform.position = new Vector3(0, 0, 0);
        ISDead = false;
        CANAttack = true;
        // 其他复活逻辑...
    }


    public void Move()
    {
        //获取输入
        Vector2 gravityDir = CustomGravity2D.GetGravity(CharacterRig.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.down, gravityDir);
        //CharacterModel.transform.rotation = Quaternion.Euler(0, 0, angle);
        CharacterRig.transform.rotation = Quaternion.Euler(0, 0, angle);


        float horizontal = Input.GetAxis("Horizontal");

        if (ISAttack == true)
        {
            //horizontal  = 0;
        }

        //左右移动控制：只重写切向（沿地面方向）的速度分量，
        //保留径向（沿重力方向）的分量，避免每帧覆盖跳跃/下落速度
        float radialSpeed = Vector2.Dot(CharacterRig.linearVelocity, gravityDir); // 当前径向速度（下落为正、跳跃上升为负）
        Vector2 tangentMove = (Vector2)transform.right * horizontal * Speed;      // 本地X轴即地面切向
        CharacterRig.linearVelocity = tangentMove + gravityDir * radialSpeed;
        //动画
        if (horizontal  > 0)
        {
            CharacterAni.SetBool("ISRun", true);
            CharacterModel.transform.localScale = new Vector3Int(1, 1, 1);
            //CharacterRig.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal  < 0)
        {
            CharacterAni.SetBool("ISRun", true);
            CharacterModel.transform.localScale = new Vector3Int(-1, 1, 1);
           // CharacterRig.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            CharacterAni.SetBool("ISRun", false);
        }
    }//移动函数

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ISJump == false)
        {
            Vector2 gravityDir = CustomGravity2D.GetGravity(CharacterRig.position).normalized;
            Vector2 jumpDir = -gravityDir;
            CharacterAni.SetBool("ISRun", false);
            // 施加与重力相反的力
            CharacterRig.AddForce(jumpDir * JumpSpeed, ForceMode2D.Impulse);
            //CharacterRig.linearVelocity = new Vector2(CharacterRig.linearVelocity.x, JumpSpeed);
            //CharacterRig.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
            //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //Vector2 jumpDirection = (transform.right * moveInput.x + transform.up * moveInput.y).normalized;
            //CharacterRig.linearVelocity = jumpDirection * JumpSpeed;
            //Vector2 jumpDirection = transform.up.normalized;
            //CharacterRig.AddForce(jumpDirection * JumpSpeed, ForceMode2D.Impulse);
        }
    }//跳跃函数

    public void Attack1()
    {
        if(CANAttack)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CharacterAni.SetTrigger("Attack1");
                CANAttack = false;
                ISAttack = true;
                AttackDamage = Attack;
                Invoke("AttackWait", AttackTime);

                AudioCon.PlayOneShot(Attack1Sound, SoundValue.Audio);
            }
        }
        
    }

    public void Attack2()
    {
        if(CANAttack)
        {
            if(Input .GetKeyDown (KeyCode.Mouse1))
            {
                CharacterAni.SetTrigger("Attack2");
                CANAttack = false;
                ISAttack = true;
                AttackDamage = 2 * Attack;
                Invoke("AttackWait", 2*AttackTime);

                AudioCon.PlayOneShot(Attack2Sound, SoundValue.Audio);
            }
        }
    }

    public void AttackWait()
    {
        CANAttack = true;
        ISAttack = false;
    }
    
}
