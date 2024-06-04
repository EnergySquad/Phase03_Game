using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ColiderCalculator : MonoBehaviour
{
    public int trial;

    // Start is called before the first frame update
 
    void Start()
    {
      trial = 0;
    }
    // Update is called once per frame
    void Update()
    {
    
      if (trial == 2)
      {
        UnityEngine.Debug.Log("Trial is 2");
      }

    }

    public void setTrial(int trial)
    {
      this.trial = trial;
    }
}
