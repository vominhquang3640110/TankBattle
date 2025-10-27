using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TankSlotMove : MonoBehaviour
{
    public float startPositionY;
    public float endPositionY;

    bool isPlay = false;

    IEnumerator Start()
    {
        transform.position = new Vector3(transform.position.x, startPositionY, transform.position.z);
        yield return new WaitForSeconds(1);
        isPlay = true;
    }

    void Update()
    {
        if (transform.position.y < endPositionY && isPlay)
        {
            float position = transform.position.y;
            position += 0.04f;
            transform.position = new Vector3(transform.position.x, position, transform.position.z);
        }
    }
}
