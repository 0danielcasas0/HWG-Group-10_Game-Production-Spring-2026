using UnityEngine;

public class TrainShake : MonoBehaviour
{
    #region === Rotations ===
    public GameObject Train;
    public float ShakeSpeed = 6f;
    public float ShakeAngle = 2.5f;
    public bool FlipShake;
    private Quaternion startRotation;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Train != null) // Check that the Train reference exists
        {
            startRotation = Train.transform.localRotation; // Store the starting rotation of the Train
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shake(); // Call the Shake method to apply the shake effect
    }

    public void Shake() // Public method that can be called by other scripts to make the Train object shake
    {
        if (Train != null && FlipShake == false) // Check that the Train reference exists and that we are not flipping the shake direction
        {
            // Oscillate between -ShakeAngle and ShakeAngle on the Z axis
            float zRotation = Mathf.Sin(Time.time * ShakeSpeed) * ShakeAngle; // Calculate a sine wave based rotation that changes over time, scaled by ShakeSpeed and ShakeAngle

            Train.transform.localRotation = startRotation * Quaternion.Euler(0, 0, zRotation); // Apply the calculated Z rotation to the Train relative to its starting rotation
        }
        else if (Train != null && FlipShake == true) // Check that the Train exists and that the shake direction should be flipped
        {
            float zRotation = -Mathf.Sin(Time.time * ShakeSpeed) * ShakeAngle; // Calculate the same sine wave rotation but invert it to flip the shaking direction

            Train.transform.localRotation = startRotation * Quaternion.Euler(0, 0, zRotation); // Apply the flipped Z rotation to the Train relative to its starting rotation
        }
    }

}
