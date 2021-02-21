using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfter : MonoBehaviour
{
    [SerializeField] private float timeUntilDeath = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeUntilDeath);
    }
}
