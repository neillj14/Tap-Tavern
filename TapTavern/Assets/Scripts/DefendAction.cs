using System.Collections;
using UnityEngine;

public class DefendAction : MonoBehaviour
{
    public static bool DefActive = true;



    private IEnumerator DefendDelay(float t)
    {
        print(Time.time);
        yield return new WaitForSeconds(1.5f);
        print(Time.time);
    }
}
