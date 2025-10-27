using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [Header("GameObject")]
    [Header("Player 1")]
    public GameObject Tank1_P1;
    public GameObject Tank2_P1;
    public GameObject Tank3_P1;
    public GameObject Tank4_P1;
    public GameObject Trap1_P1;
    public GameObject Trap2_P1;
    public GameObject Trap3_P1;
    [Header("Player 2")]
    public GameObject Tank1_P2;
    public GameObject Tank2_P2;
    public GameObject Tank3_P2;
    public GameObject Tank4_P2;
    public GameObject Trap1_P2;
    public GameObject Trap2_P2;
    public GameObject Trap3_P2;

    [Header("Button")]
    //Buy Button
    public GameObject Tank1_button;
    public GameObject Tank2_button;
    public GameObject Tank3_button;
    public GameObject Tank4_button;
    //Trap Button
    public GameObject Trap1_button;
    public GameObject Trap2_button;
    public GameObject Trap3_button;
    //My Hand Button
    public GameObject Tank1_myHand;
    public GameObject Tank2_myHand;
    public GameObject Tank3_myHand;
    public GameObject Tank4_myHand;

    [Header("Sprite")]
    public Sprite X_setSlot;
    public Sprite O_setSlot;
    public Sprite Tank1_setSlot;
    public Sprite Tank2_setSlot;
    public Sprite Tank3_setSlot;
    public Sprite Tank4_setSlot;
    public Sprite Trap1_setSlot;
    public Sprite Trap2_setSlot;
    public Sprite Trap3_setSlot;

    /// <summary>
    /// タンクプレファブを取得する
    /// </summary>
    /// <param name="prefabName">取得したいタンクプレファブの名前</param>
    /// <returns>GameObject型のタンクプレファブ</returns>
    public GameObject GetTankPrefab(string prefabName)
    {
        //P1
        if (prefabName == "Tank1_P1")
        {
            return Tank1_P1;
        }
        else if (prefabName == "Tank2_P1")
        {
            return Tank2_P1;
        }
        else if (prefabName == "Tank3_P1")
        {
            return Tank3_P1;
        }
        else if (prefabName == "Tank4_P1")
        {
            return Tank4_P1;
        }
        else if (prefabName == "Trap1_P1")
        {
            return Trap1_P1;
        }
        else if (prefabName == "Trap2_P1")
        {
            return Trap2_P1;
        }
        else if (prefabName == "Trap3_P1")
        {
            return Trap3_P1;
        }
        //P2
        else if (prefabName == "Tank1_P2")
        {
            return Tank1_P2;
        }
        else if (prefabName == "Tank2_P2")
        {
            return Tank2_P2;
        }
        else if (prefabName == "Tank3_P2")
        {
            return Tank3_P2;
        }
        else if (prefabName == "Tank4_P2")
        {
            return Tank4_P2;
        }
        else if (prefabName == "Trap1_P2")
        {
            return Trap1_P2;
        }
        else if (prefabName == "Trap2_P2")
        {
            return Trap2_P2;
        }
        else if (prefabName == "Trap3_P2")
        {
            return Trap3_P2;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// ボタンプレファブを取得する
    /// </summary>
    /// <param name="prefabName">取得したいボタンプレファブの名前</param>
    /// <returns>GameObject型のボタンプレファブ</returns>
    public GameObject GetButtonPrefab(string prefabName)
    {
        //Buy Button
        if (prefabName == "Tank1")
        {
            return Tank1_button;
        }
        else if (prefabName == "Tank2")
        {
            return Tank2_button;
        }
        else if (prefabName == "Tank3")
        {
            return Tank3_button;
        }
        else if (prefabName == "Tank4")
        {
            return Tank4_button;
        }
        //Trap Button
        else if (prefabName == "Trap1")
        {
            return Trap1_button;
        }
        else if (prefabName == "Trap2")
        {
            return Trap2_button;
        }
        else if (prefabName == "Trap3")
        {
            return Trap3_button;
        }
        //My Hand Button
        else if (prefabName == "Tank1_myHand")
        {
            return Tank1_myHand;
        }
        else if (prefabName == "Tank2_myHand")
        {
            return Tank2_myHand;
        }
        else if (prefabName == "Tank3_myHand")
        {
            return Tank3_myHand;
        }
        else if (prefabName == "Tank4_myHand")
        {
            return Tank4_myHand;
        }
        else
        {
            return null;
        }
    }

    public Sprite GetSprite(string spriteName)
    {
        if (spriteName == "X")
        {
            return X_setSlot;
        }
        else if (spriteName == "O")
        {
            return O_setSlot;
        }
        else if (spriteName == "Tank1")
        {
            return Tank1_setSlot;
        }
        else if (spriteName == "Tank2")
        {
            return Tank2_setSlot;
        }
        else if (spriteName == "Tank3")
        {
            return Tank3_setSlot;
        }
        else if (spriteName == "Tank4")
        {
            return Tank4_setSlot;
        }
        else if (spriteName == "Trap1")
        {
            return Trap1_setSlot;
        }
        else if (spriteName == "Trap2")
        {
            return Trap2_setSlot;
        }
        else if (spriteName == "Trap3")
        {
            return Trap3_setSlot;
        }
        else
        {
            return null;
        }
    }
}
