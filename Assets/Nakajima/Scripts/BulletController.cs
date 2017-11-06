using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    GameObject _bullet;

    public GameObject bulletSpot;

    public float _bulletSpeed;
    public float Interval;
    float time;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(time > Interval)
            {

                GameObject _bornBullet = Instantiate(_bullet,bulletSpot.transform.position,transform.rotation);

                _bornBullet.GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;

                Destroy(_bornBullet, 1.0f);

                time = 0;
            }
        }
	}
}
