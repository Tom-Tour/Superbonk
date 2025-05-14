using UnityEngine;

public class CameraLevel : MonoBehaviour
{
    void Update()
    {
        float totalX = 0f;
        int count = 1;
        foreach (var character in GameManager.instance.characters)
        {
            totalX += character.transform.position.x;
            count++;
        }
        float averageX = count > 0 ? totalX / count : 0f;
        transform.position = new Vector3(averageX, transform.position.y, transform.position.z);
    }
}
