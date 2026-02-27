using UnityEngine;

public class TrainShake : MonoBehaviour
{
    public GameObject Train;
    public float ShakeSpeed = 2.0f;
    public float ShakeAngle = 5.0f;
    public bool FlipShake;
    private Quaternion startRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Train != null)
        {
            startRotation = Train.transform.localRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shake();
    }

    public void Shake()
    {
        if (Train != null && FlipShake == false)
        {
            // Oscillate between -ShakeAngle and ShakeAngle on the Z axis
            float zRotation = Mathf.Sin(Time.time * ShakeSpeed) * ShakeAngle;
            Train.transform.localRotation = startRotation * Quaternion.Euler(0, 0, zRotation);
        }
        else if (Train != null && FlipShake == true)
        {
            float zRotation = -Mathf.Sin(Time.time * ShakeSpeed) * ShakeAngle;
            Train.transform.localRotation = startRotation * Quaternion.Euler(0, 0, zRotation);
        }
    }

}
