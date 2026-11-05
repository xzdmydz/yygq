using System.Collections;
using TMPro;
using UnityEngine;
public class jiguan : MonoBehaviour
{
    public GameObject QiFei;
    public float HP = 100;
    public float HPNow = 100;
    public AudioSource AudioCon;
    public stickman stickman;
    public bool CanAttack = true;
    public AudioClip GetHitSound;
    public AudioClip DeadSound;
    private bool ISDead = false;
    public TextMeshProUGUI counterText; // 关联UI文本
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPNow = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (counterText != null)
        {
            if(HPNow >0)
            {
                counterText.text = $"加把劲啊，机关还剩: {HPNow}";
            }
            else
            {
                counterText.text = "对应的阵法启动";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {

            DoHP();


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
            //Destroy(EnemyObjS,2.0f);
            // 改为禁用而非销毁
            DisableWithDelay(); // 禁用整个敌人对象

        }
        else
        {
            //EnemyAni.SetTrigger("GetHit");
            AudioCon.PlayOneShot(GetHitSound, SoundValue.Audio);
            //EnemyAni.SetBool("ISRun", false);
            //Debug.Log($"敌人被打了现在血量是{HPNow}");
        }



        //Debug.Log($"现在血量是：{HPNow}");

    }

    public void DisableWithDelay()
    {
        StartCoroutine(DisableAfterDelay(2f));
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        //EnemyAni.SetTrigger("ISDead");
        AudioCon.PlayOneShot(DeadSound, SoundValue.Audio);
        yield return new WaitForSeconds(delay); // 等待2秒[6](@ref)
        gameObject.SetActive(false);           // 禁用GameObject[2,5](@ref)
        //Tile.AddQi(1);
        QiFei.SetActive(true);
    }
}
