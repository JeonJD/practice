using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed;

    private Vector3 direction;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            float xValue = Input.GetAxisRaw("Vertical");
            float zValue = Input.GetAxisRaw("Horizontal");
            direction = xValue * Vector3.forward + zValue * Vector3.right;

            this.transform.rotation = Quaternion.LookRotation(direction);
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}