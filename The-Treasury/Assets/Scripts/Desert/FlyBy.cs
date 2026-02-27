using UnityEngine;

public class FlyBy : MonoBehaviour
{
    public float Speed = 100f;
    public float zLimit = 500f;
    public float zReset = -500f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * Speed * Time.deltaTime;

        if (transform.position.z >= zLimit)
        {
            Vector3 pos = transform.position;
            pos.z = zReset;
            transform.position = pos;
        }
    }
}
