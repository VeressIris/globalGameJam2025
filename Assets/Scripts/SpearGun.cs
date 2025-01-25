using UnityEngine;

public class SpearGun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firingPoint;
    [SerializeField] float firingDelay = 2;
    [SerializeField] float reloadDelay = 1;
    [SerializeField] GameObject displaySpear;
    float nextTimeFiring = 0;
    float nextTimeReload = 0;

    public void Fire()
    {
        if (nextTimeFiring > Time.time)
            return;
        nextTimeFiring = Time.time + firingDelay;
        nextTimeReload = Time.time + reloadDelay;
        var clone = Instantiate(bullet);
        clone.GetComponent<Rigidbody2D>().linearVelocity += GetComponentInParent<Rigidbody2D>().linearVelocity;
        clone.transform.position = firingPoint.position;
        clone.transform.localScale = displaySpear.transform.lossyScale;
        var dif = Camera.main.ScreenToWorldPoint(Input.mousePosition) - clone.transform.position;
        clone.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dif.y,dif.x) * Mathf.Rad2Deg);
    }

    void Update()
    {
        displaySpear.SetActive(nextTimeReload < Time.time);
    }
}
