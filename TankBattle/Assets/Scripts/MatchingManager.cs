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
        //������
        isMatching = true;
        counterTimer = 0;
        soundManager = SoundManager.instance;

        //�^�C�}�[���N��
        StartCoroutine(TimerCounter());
    }

    void Update()
    {
        //���肪�����������_InGame�ֈړ�
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
        //�}�b�`���O�������A���Ԃ�Ready�֕ϊ�
        timer.text = "Ready!!";
        timer.GetComponent<Animator>().SetBool("isPlay", true);

        yield return new WaitForSeconds(3);

        //��ʂ��Â��Ȃ鉉�o
        moveScene.SetBool("isMoveScene", true);

        yield return new WaitForSeconds(1);

        //InGame�V�[���ֈړ�
        PhotonNetwork.LoadLevel("InGame");
    }

    IEnumerator TimerCounter()
    {
        yield return new WaitForSeconds(1);
        counterTimer++;

        //3�b�ɂȂ������_�Ƀ��[����T���n�߂�
        if (isMatching && counterTimer == 3)
        {
            PhotonNetwork.JoinRandomRoom();
        }

        if (!isMatching)
        {
            
        }
        else
        {
            //���[����T�����Ԃ�\��������
            timer.text = ShowTimer(counterTimer);
        }

        StartCoroutine(TimerCounter());
    }

    string ShowTimer(int timerValue)
    {
        //���Ԃ̒l���e�L�X�g�֕ϊ�
        int min = timerValue / 60;
        int sec = timerValue % 60;
        string minStr = min >= 10 ? min.ToString() : "0" + min;
        string secStr = sec >= 10 ? sec.ToString() : "0" + sec;
        return minStr + ":" + secStr;
    }

    public void OnClickCancelButton()
    {
        soundManager.PlayClickButton();
        //photon�̐ڑ���ؒf����
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Title");
    }
}
