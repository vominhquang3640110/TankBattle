using UnityEngine;

public class Shell : MonoBehaviour
{
    private float speed = 20f;
    public ParticleSystem shellExplosion;
    public AudioClip shellExplosionAu;
    private Vector3 moveDirection;

    // �G���^�[�Q�b�g�Ƃ��ăZ�b�g
    public void SetTarget(Transform enemy)
    {
        // �G�ւ̕������v�Z
        moveDirection = (enemy.position - transform.position).normalized;
        moveDirection.y = 0;

        // ���������킹��
        transform.rotation = Quaternion.LookRotation(moveDirection);

        shellExplosion.Play();  //�e�ۂ�ł̃G�t�F�N�g
    }

    void Update()
    {
        // ��ɓ��������֐i�ށi�G�������Ă���͋O����ς��Ȃ��j
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = "";
        if (InGameManager.instance._P1.isTurn)
        {
            tag = "Tank2";
        }
        else
        {
            tag = "Tank1";
        }

        if (other.gameObject.CompareTag(tag))
        {
            ParticleSystem effect = Instantiate(shellExplosion, transform.position, Quaternion.identity);   //�����������̃G�t�F�N�g�𐶐�����
            effect.Play();  //�G�t�F�N�g���Đ�
            Destroy(effect.gameObject, 1);  //�Đ����I�������A�G�t�F�N�g��Destroy
            Destroy(gameObject);    //���̒e�ۂ�Destroy
            PlaySoundShellExplosion();  //����炷
            other.GetComponent<Tank>().currentHealth--; //���������^���N��HP��-1
        }
        else if (other.gameObject.CompareTag("PlayerHP"))
        {
            ParticleSystem effect = Instantiate(shellExplosion, transform.position, Quaternion.identity);   //�����������̃G�t�F�N�g�𐶐�����
            effect.Play();  //�G�t�F�N�g���Đ�
            Destroy(effect.gameObject, 1);  //�Đ����I�������A�G�t�F�N�g��Destroy
            Destroy(gameObject);    //���̒e�ۂ�Destroy
            PlaySoundShellExplosion();  //����炷
            other.GetComponent<PlayerHP>().SetPlayerHP(1); //���������v���C���[��HP��-1
        }
        Debug.Log(other.name);
    }
    //�^���N�ɓ����������̉�
    void PlaySoundShellExplosion()
    {
        SoundManager.instance.PlayOneShotAudioClip(shellExplosionAu);
    }
}
