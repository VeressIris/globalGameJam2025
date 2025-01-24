using UnityEngine;

public class SpearGun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firingPoint;
    [SerializeField] float firingDelay = 1;

    float nextTimeFiring = 0;

    public void Fire()
    {
        if (nextTimeFiring < Time.time)
            return;
        nextTimeFiring = Time.time + firingDelay;
        var clone = Instantiate(bullet, firingPoint);
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localRotation = Quaternion.identity;
    }
}
