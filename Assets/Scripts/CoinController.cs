using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        transform.Rotate(speed, 0, 0);
    }
}
