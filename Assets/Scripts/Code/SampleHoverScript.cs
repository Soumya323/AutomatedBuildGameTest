using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleHoverScript : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float height = 2.0f;
    private Vector3 initPos = default;
    private float timer = 0.0f;
    private float sinResult = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        initPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;
        sinResult = Mathf.Sin(timer);
        this.transform.position = initPos + Vector3.right * sinResult * height;
    }
}
