using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rot = transform.eulerAngles;
        rot.y = Camera.main.transform.eulerAngles.y;
      transform.eulerAngles = rot;  
    }
}
