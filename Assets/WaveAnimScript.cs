using UnityEngine;

public class WaveAnimScript : MonoBehaviour
{
    [SerializeField] float mag;
    [SerializeField] float speed;
    Vector3 initialPos;
    void Start()
    {
        initialPos = transform.position;
    }
    void Update()
    {
        transform.position = initialPos + new Vector3(Mathf.Sin(Time.time * speed), Mathf.Cos(Time.time * speed), 0) * mag;
    }
}
