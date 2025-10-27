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
            //�U���̎��AHPCage�͍U���ł����
            HPCage.value = attackNumber;
        }
        else
        {
            //����̎��AHPCage�̓^���N�̗̑�
            HPCage.value = currentHealth;
            //�̗͂��Ȃ��Ȃ�����A�j��
            if (currentHealth <= 0)
            {
                TankDestroy();
            }
        }
    }

    /// <summary>
    /// �U�������i�e�𐶐����A�G�ɑłj
    /// </summary>
    /// <param name="target">�^�[�Q�b�g�̈ʒu���</param>
    public void Attack(Transform target)
    {
        attackNumber--;
        transform.LookAt(target);
        GameObject shell = Instantiate(shellPrefab, shellSpawnPosition.position, Quaternion.identity);
        shell.GetComponent<Shell>().SetTarget(target);
        PlaySoundShortFiring();
    }
    /// <summary>
    /// ���̃^���N��j�����鏈��
    /// </summary>
    public void TankDestroy()
    {
        ParticleSystem explosionEffect = Instantiate(tankExplosion, transform.position, Quaternion.identity);
        explosionEffect.Play();
        PlaySoundTankExplosion();
        Destroy(explosionEffect, 1);
        Destroy(gameObject);
    }
    //�e��ł��̉�
    void PlaySoundShortFiring()
    {
        inGameManager.soundManager.PlayOneShotAudioClip(shotFiring);
    }
    //�^���N���j�󂷂鎞�̉�
    void PlaySoundTankExplosion()
    {
        inGameManager.soundManager.PlayOneShotAudioClip(tankExplosionAu);
    }
    //����Ƃ��̉��𒆎~
    public void StopSoundEngineDriving()
    {
        GetComponent<AudioSource>().Stop();
    }
}
