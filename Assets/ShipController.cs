using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveAtBase: State{

    public override void Enter(){
        owner.GetComponent<Arrive>().enabled = true;
        owner.GetComponent<Arrive>().targetGameObject = owner.GetComponent<ShipController>().targetBase;
    }

    public override void Think()
    {
        float distance = Vector3.Distance(owner.transform.position, owner.GetComponent<ShipController>().targetBase.transform.position);

        if(distance <= owner.GetComponent<ShipController>().stoppingDistance){
            owner.GetComponent<StateMachine>().ChangeState(new ShootAtBase());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<Arrive>().enabled = false;
    }
}

public class ShootAtBase : State{
    public override void Enter()
    {
        owner.GetComponent<Arrive>().enabled = true;
        owner.GetComponent<Arrive>().targetGameObject = owner.gameObject;
        owner.GetComponent<ShipController>().StartCoroutine("ShootAtBase");

    }

    public override void Think()
    {

        if(owner.GetComponent<ShipController>().tiberium == 0){
            owner.GetComponent<StateMachine>().ChangeState(new ShootAtBase());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<ShipController>().StopCoroutine("ShootAtBase");

        owner.GetComponent<Arrive>().enabled = false;
    }
}


public class ShipController : MonoBehaviour
{
    public GameObject bullet;

    public GameObject targetBase;

    public float stoppingDistance;


    public int tiberium;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new ArriveAtBase());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShipSetup(Color col, GameObject target, int numberOfTiberium){
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = col;
        }

        targetBase = target;

        tiberium += numberOfTiberium;
    }

    public IEnumerator ShootAtBase(){
        while(true){
            yield return new WaitForSeconds(1f);
            if(tiberium > 0){
                GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
                newBullet.SendMessage("SetColour", GetComponent<Renderer>().material.color);
                tiberium--;
            }
        }
    }
}
