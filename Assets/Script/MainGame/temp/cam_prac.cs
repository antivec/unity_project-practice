using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_prac : MonoBehaviour
{
    [SerializeField]
    public Transform _target;

    private float _yrot = 0.0f;
    private float _xrot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion.Euler(30.0f, 60.0f, 30.0f);
    }
}
