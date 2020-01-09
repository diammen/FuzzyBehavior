using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    [SerializeField]
    private float speed;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.localScale *= 1 + Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            transform.localScale /= 1 + Time.deltaTime;
        }
    }
}
