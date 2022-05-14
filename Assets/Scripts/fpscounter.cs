using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fpscounter : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (1.0f / Time.deltaTime).ToString("0");
    }
}
