using UnityEngine;

public class PlayerInfor : MonoBehaviour
{
    [Header("Photon")]
    public PhotonView _photonView;

    [Header("UserDatas")]
    public bool isWinGame = false;  //このプレイヤーが勝者であるか
    public bool isSurrender = false;    //降参
    public int HP;      //プレイヤーのHP
    public int randomNumberTurn = 0;    //先攻・後攻を決める乱数
    public string stepName = "";    //現在のターンの進行
    public bool isTurn = false;     //自分のターンであるか
    public int currentCoin = 0;     //所有コイン
    public int handNumber = 0;      //使用可能タンクの数
    public string[] slotDatas = new string[5];      //出陣するタンクの名前

    void Awake()
    {
        randomNumberTurn = Random.Range(1, 100);
        HP = 5;

        //slotDatasを初期化
        for (int i = 0; i < slotDatas.Length; i++)
        {
            slotDatas[i] = "";
        }
    }

    /// <summary>
    /// Photonを使ってお互いのユーザーのデータを送受信する
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //このプレイヤーが勝者であるか
            stream.SendNext(isWinGame);
            //降参
            stream.SendNext(isSurrender);
            //プレイヤーのHP
            stream.SendNext(HP);
            //先攻・後攻を決める値
            stream.SendNext(randomNumberTurn);
            //ターン
            stream.SendNext(stepName);
            stream.SendNext(isTurn);
            //現在の所有コイン
            stream.SendNext(currentCoin);
            //現在の手札
            stream.SendNext(handNumber);
            //出陣するタンクの名前
            for (int i = 0; i < slotDatas.Length; i++)
            {
                stream.SendNext(slotDatas[i]);
            }
        }
        else
        {
            //このプレイヤーが勝者であるか
            isWinGame = (bool)stream.ReceiveNext();
            //降参
            isSurrender = (bool)stream.ReceiveNext();
            //プレイヤーのHP
            HP = (int)stream.ReceiveNext();
            //先攻・後攻を決める値
            randomNumberTurn = (int)stream.ReceiveNext();
            //ターン
            stepName = (string)stream.ReceiveNext();
            isTurn = (bool)stream.ReceiveNext();
            //現在の所有コイン
            currentCoin = (int)stream.ReceiveNext();
            //現在の手札
            handNumber = (int)stream.ReceiveNext();
            //出陣するタンクの名前
            for (int i = 0; i < slotDatas.Length; i++)
            {
                slotDatas[i] = (string)stream.ReceiveNext();
            }
        }
    }
}
