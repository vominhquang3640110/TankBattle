using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        while (true)
        {
            float yAngle = transform.eulerAngles.y;

            // -90“x‚æ‚è‘å‚«‚¯‚ê‚Î‰ñ“]‚µ‘±‚¯‚é
            if (yAngle > 90f)
            {
                transform.Rotate(0, -100 * Time.deltaTime, 0);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                break;
            }
        }
        while (true)
        {
            if (transform.position.x < target.position.x)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                transform.position = new Vector3(x + (30 * Time.deltaTime), y, z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                transform.position = new Vector3(target.position.x, y, z);
                break;
            }
        }
        while (true)
        {
            float yAngle = transform.eulerAngles.y;

            // 0“x‚æ‚è¬‚³‚¯‚ê‚Î‰ñ“]‚µ‘±‚¯‚é
            if (yAngle < 180f)
            {
                transform.Rotate(0, +100 * Time.deltaTime, 0);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            }
        }
        while (true)
        {
            if (transform.position.z > target.position.z)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                transform.position = new Vector3(x, y, z - (30 * Time.deltaTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                transform.position = new Vector3(x, y, target.position.z);
                break;
            }
        }
        yield return null;
    }
}
