using UnityEngine;
using TMPro;

public class BillboardText : MonoBehaviour
{
    private Transform mainCamTransform;

    private void Start()
    {
        if (Camera.main != null)
        {
            mainCamTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Main camera not found. Make sure to tag your camera as Main Camera");
            this.enabled = false;
        }
    }

    // LateUpdate is to make sure the text updates when players move
    private void LateUpdate()
    {
        if (mainCamTransform != null) 
        {
            // Makes the text face the main camera's current position
            transform.LookAt(mainCamTransform.position);

            transform.RotateAround(transform.position, transform.up, 180f);
        }
    }

}
