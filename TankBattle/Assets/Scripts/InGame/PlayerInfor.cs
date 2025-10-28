using UnityEngine;

public class PlayerInfor : MonoBehaviour
{
    [Header("Photon")]
    public PhotonView _photonView;

    [Header("UserDatas")]
    public bool isWinGame = false;  //���̃v���C���[�����҂ł��邩
    public bool isSurrender = false;    //�~�Q
    public int HP;      //�v���C���[��HP
    public int randomNumberTurn = 0;    //��U�E��U�����߂闐��
    public string stepName = "";    //���݂̃^�[���̐i�s
    public bool isTurn = false;     //�����̃^�[���ł��邩
    public int currentCoin = 0;     //���L�R�C��
    public int handNumber = 0;      //�g�p�\�^���N�̐�
    public string[] slotDatas = new string[5];      //�o�w����^���N�̖��O

    void Awake()
    {
        randomNumberTurn = Random.Range(1, 100);
        HP = 5;

        //slotDatas��������
        for (int i = 0; i < slotDatas.Length; i++)
        {
            slotDatas[i] = "";
        }
    }

    /// <summary>
    /// Photon���g���Ă��݂��̃��[�U�[�̃f�[�^�𑗎�M����
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //���̃v���C���[�����҂ł��邩
            stream.SendNext(isWinGame);
            //�~�Q
            stream.SendNext(isSurrender);
            //�v���C���[��HP
            stream.SendNext(HP);
            //��U�E��U�����߂�l
            stream.SendNext(randomNumberTurn);
            //�^�[��
            stream.SendNext(stepName);
            stream.SendNext(isTurn);
            //���݂̏��L�R�C��
            stream.SendNext(currentCoin);
            //���݂̎�D
            stream.SendNext(handNumber);
            //�o�w����^���N�̖��O
            for (int i = 0; i < slotDatas.Length; i++)
            {
                stream.SendNext(slotDatas[i]);
            }
        }
        else
        {
            //���̃v���C���[�����҂ł��邩
            isWinGame = (bool)stream.ReceiveNext();
            //�~�Q
            isSurrender = (bool)stream.ReceiveNext();
            //�v���C���[��HP
            HP = (int)stream.ReceiveNext();
            //��U�E��U�����߂�l
            randomNumberTurn = (int)stream.ReceiveNext();
            //�^�[��
            stepName = (string)stream.ReceiveNext();
            isTurn = (bool)stream.ReceiveNext();
            //���݂̏��L�R�C��
            currentCoin = (int)stream.ReceiveNext();
            //���݂̎�D
            handNumber = (int)stream.ReceiveNext();
            //�o�w����^���N�̖��O
            for (int i = 0; i < slotDatas.Length; i++)
            {
                slotDatas[i] = (string)stream.ReceiveNext();
            }
        }
    }
}
