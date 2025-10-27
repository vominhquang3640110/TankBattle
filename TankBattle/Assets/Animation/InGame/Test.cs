using DG.Tweening;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TMP_Text label;
    public int finalValue = 777; // 最終的に表示したい数字
    public float duration = 2f;  // アニメーション時間

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

            // ランダムな数値を表示
            int randomNum = Random.Range(0, 999);
            label.text = randomNum.ToString();

        }, 1f, duration)
        .OnComplete(() => {
            // 最後に目的の数値を表示して終了
            label.text = finalValue.ToString();
        });
    }
}
