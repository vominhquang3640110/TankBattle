using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public bool isP1;
    public Slider HPCage;

    private void Update()
    {
        if (!isP1)
        {
            HPCage.value = InGameManager.instance._P2.HP;
            if (!InGameManager.instance._P1.isWinGame && InGameManager.instance._P2.HP <= 0)
            {
                InGameManager.instance._P1.isWinGame = true;
            }
        }
    }

    public void SetPlayerHP(int damage)
    {
        if (isP1)
        {
            InGameManager.instance._P1.HP -= damage;
            HPCage.value = InGameManager.instance._P1.HP;
        }
        //チュートリアルである
        if (!isP1 && UserManager.instance.isTutorialMode)
        {
            InGameManager.instance._P2.HP -= damage;
        }
    }
}
