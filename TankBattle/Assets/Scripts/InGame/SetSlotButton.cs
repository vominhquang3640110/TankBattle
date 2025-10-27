using UnityEngine;
using UnityEngine.UI;

public enum Player
{
    P1,
    P2
}
public class SetSlotButton : MonoBehaviour
{
    private InGameManager inGameManager;
    private PrefabManager prefabManager;
    private UIManager uiManager;

    public Player player;
    public string buttonName;
    public int id;

    void Start()
    {
        inGameManager = InGameManager.instance;   
        prefabManager = inGameManager.prefabManager;
        uiManager = inGameManager.uiManager;
    }

    void Update()
    {
        //P2��SetSlot���X�V
        if (player == Player.P2)
        {
            buttonName = inGameManager._P2.slotDatas[id];
            if (buttonName != "" && buttonName != null)
            {
                GetComponent<Image>().sprite = prefabManager.GetSprite("O");
            }
            else
            {
                GetComponent<Image>().sprite = prefabManager.GetSprite("X");
            }
        }
    }

    /// <summary>
    /// �摜��buttonName�ɉ����ĕύX(P1��p�̏���)
    /// </summary>
    public void SetSprite()
    {
        if (buttonName == "" || buttonName == null)
        {
            GetComponent<Image>().sprite = prefabManager.GetSprite("X");
        }
        else
        {
            GetComponent<Image>().sprite = prefabManager.GetSprite(buttonName);
        }
    }
}
