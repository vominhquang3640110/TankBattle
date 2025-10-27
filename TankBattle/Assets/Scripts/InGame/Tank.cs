using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    private InGameManager inGameManager;

    [Header("Tank Data")]
    public string tankName;
    public int power;
    private int maxHealth;
    public bool isAttack = false;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public int attackNumber;

    [SerializeField] Slider HPCage;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] Transform shellSpawnPosition;
    [SerializeField] ParticleSystem tankExplosion;

    [Header("Sound")]
    [SerializeField] AudioClip shotFiring;
    [SerializeField] AudioClip tankExplosionAu;

    void Start()
    {
        inGameManager = InGameManager.instance;

        maxHealth = power;
        currentHealth = maxHealth;
        attackNumber = power;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            //攻撃の時、HPCageは攻撃できる回数
            HPCage.value = attackNumber;
        }
        else
        {
            //守備の時、HPCageはタンクの体力
            HPCage.value = currentHealth;
            //体力がなくなったら、破壊
            if (currentHealth <= 0)
            {
                TankDestroy();
            }
        }
    }

    /// <summary>
    /// 攻撃処理（弾を生成し、敵に打つ）
    /// </summary>
    /// <param name="target">ターゲットの位置情報</param>
    public void Attack(Transform target)
    {
        attackNumber--;
        transform.LookAt(target);
        GameObject shell = Instantiate(shellPrefab, shellSpawnPosition.position, Quaternion.identity);
        shell.GetComponent<Shell>().SetTarget(target);
        PlaySoundShortFiring();
    }
    /// <summary>
    /// このタンクを破棄する処理
    /// </summary>
    public void TankDestroy()
    {
        ParticleSystem explosionEffect = Instantiate(tankExplosion, transform.position, Quaternion.identity);
        explosionEffect.Play();
        PlaySoundTankExplosion();
        Destroy(explosionEffect, 1);
        Destroy(gameObject);
    }
    //弾を打つ時の音
    void PlaySoundShortFiring()
    {
        inGameManager.soundManager.PlayOneShotAudioClip(shotFiring);
    }
    //タンクが破壊する時の音
    void PlaySoundTankExplosion()
    {
        inGameManager.soundManager.PlayOneShotAudioClip(tankExplosionAu);
    }
    //走るときの音を中止
    public void StopSoundEngineDriving()
    {
        GetComponent<AudioSource>().Stop();
    }
}
