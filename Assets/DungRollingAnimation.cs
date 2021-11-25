using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungRollingAnimation : MonoBehaviour
{

    public float speedRotate = 90f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);

    }
}
