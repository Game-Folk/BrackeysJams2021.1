using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommands : MonoBehaviour
{
    public static PlayerCommands instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PlayerCommands in scene!");
            return;
        }
        instance = this;
    }
    
    public static event Action<Transform> OnPrisonersRecalled;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("PlayerCommands: prisoners recalling");
            OnPrisonersRecalled?.Invoke(this.transform);
        }
    }
}
