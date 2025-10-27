using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private Animator moveScene;
    [SerializeField] private Button backButton;

    int counterTimer;
    bool isMatching = default;

    void Start()
    {
        //初期化
        isMatching = true;
        counterTimer = 0;
        soundManager = SoundManager.instance;

        //タイマーを起動
        StartCoroutine(TimerCounter());
    }

    void Update()
    {
        //相手が見つかった時点InGameへ移動
        if (PhotonNetwork.playerList.Length == 2 && isMatching)
        {
            backButton.gameObject.SetActive(false);
            isMatching = false;
            StartCoroutine(MoveInGame());
        }
    }

    IEnumerator MoveInGame()
    {
        soundManager.PlayMatchingSuccess();
        //マッチング成功時、時間をReadyへ変換
        timer.text = "Ready!!";
        timer.GetComponent<Animator>().SetBool("isPlay", true);

        yield return new WaitForSeconds(3);

        //画面が暗くなる演出
        moveScene.SetBool("isMoveScene", true);

        yield return new WaitForSeconds(1);

        //InGameシーンへ移動
        PhotonNetwork.LoadLevel("InGame");
    }

    IEnumerator TimerCounter()
    {
        yield return new WaitForSeconds(1);
        counterTimer++;

        //3秒になった時点にルームを探し始める
        if (isMatching && counterTimer == 3)
        {
            PhotonNetwork.JoinRandomRoom();
        }

        if (!isMatching)
        {
            
        }
        else
        {
            //ルームを探す時間を表示させる
            timer.text = ShowTimer(counterTimer);
        }

        StartCoroutine(TimerCounter());
    }

    string ShowTimer(int timerValue)
    {
        //時間の値をテキストへ変換
        int min = timerValue / 60;
        int sec = timerValue % 60;
        string minStr = min >= 10 ? min.ToString() : "0" + min;
        string secStr = sec >= 10 ? sec.ToString() : "0" + sec;
        return minStr + ":" + secStr;
    }

    public void OnClickCancelButton()
    {
        soundManager.PlayClickButton();
        //photonの接続を切断する
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Title");
    }
}
