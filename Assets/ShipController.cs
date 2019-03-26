using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState: State{
    public override void Enter(){
    }
}


public class ShipController : MonoBehaviour
{
    public GameObject bullet;

    GameObject targetBase;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShipSetup(Color col, GameObject target){
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = col;
        }

        targetBase = target;
    }
}
