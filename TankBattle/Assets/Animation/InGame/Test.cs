using DG.Tweening;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TMP_Text label;
    public int finalValue = 777; // �ŏI�I�ɕ\������������
    public float duration = 2f;  // �A�j���[�V��������

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayRandomAnimation();   
        }
    }

    public void PlayRandomAnimation()
    {
        float dummy = 0;

        DOTween.To(() => dummy, x => {
            dummy = x;

            // �����_���Ȑ��l��\��
            int randomNum = Random.Range(0, 999);
            label.text = randomNum.ToString();

        }, 1f, duration)
        .OnComplete(() => {
            // �Ō�ɖړI�̐��l��\�����ďI��
            label.text = finalValue.ToString();
        });
    }
}
