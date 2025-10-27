using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    [Header("InGame")]
    public Dictionary<string, int> buyButtonDatas;      
    public Dictionary<string, int> trapButtonDatas;
    public bool isTutorialMode;     //チュートリアルであるか

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        buyButtonDatas = new Dictionary<string, int>();
        trapButtonDatas = new Dictionary<string, int>();

        buyButtonDatas.Add("Tank1", 10);
        buyButtonDatas.Add("Tank2", 5);
        buyButtonDatas.Add("Tank3", 3);
        buyButtonDatas.Add("Tank4", 3);

        trapButtonDatas.Add("Trap1", 3);
        trapButtonDatas.Add("Trap2", 2);
        trapButtonDatas.Add("Trap3", 1);
    }
}
