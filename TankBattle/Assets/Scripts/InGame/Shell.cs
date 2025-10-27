using UnityEngine;

public class Shell : MonoBehaviour
{
    private float speed = 20f;
    public ParticleSystem shellExplosion;
    public AudioClip shellExplosionAu;
    private Vector3 moveDirection;

    // 敵をターゲットとしてセット
    public void SetTarget(Transform enemy)
    {
        // 敵への方向を計算
        moveDirection = (enemy.position - transform.position).normalized;
        moveDirection.y = 0;

        // 向きを合わせる
        transform.rotation = Quaternion.LookRotation(moveDirection);

        shellExplosion.Play();  //弾丸を打つのエフェクト
    }

    void Update()
    {
        // 常に同じ方向へ進む（敵が動いても矢は軌道を変えない）
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
            ParticleSystem effect = Instantiate(shellExplosion, transform.position, Quaternion.identity);   //当たった時のエフェクトを生成する
            effect.Play();  //エフェクトを再生
            Destroy(effect.gameObject, 1);  //再生が終わったら、エフェクトをDestroy
            Destroy(gameObject);    //この弾丸をDestroy
            PlaySoundShellExplosion();  //音を鳴らす
            other.GetComponent<Tank>().currentHealth--; //当たったタンクのHPを-1
        }
        else if (other.gameObject.CompareTag("PlayerHP"))
        {
            ParticleSystem effect = Instantiate(shellExplosion, transform.position, Quaternion.identity);   //当たった時のエフェクトを生成する
            effect.Play();  //エフェクトを再生
            Destroy(effect.gameObject, 1);  //再生が終わったら、エフェクトをDestroy
            Destroy(gameObject);    //この弾丸をDestroy
            PlaySoundShellExplosion();  //音を鳴らす
            other.GetComponent<PlayerHP>().SetPlayerHP(1); //当たったプレイヤーのHPを-1
        }
        Debug.Log(other.name);
    }
    //タンクに当たった時の音
    void PlaySoundShellExplosion()
    {
        SoundManager.instance.PlayOneShotAudioClip(shellExplosionAu);
    }
}
