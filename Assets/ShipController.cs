﻿using System.Collections;
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
        owner.GetComponent<ShipController>().StartCoroutine("ShootAtBase");
    }

    public override void Think()
    {

        if(owner.GetComponent<ShipController>().tiberium == 0){
            owner.GetComponent<StateMachine>().ChangeState(new ReturnToBase());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<ShipController>().StopCoroutine("ShootAtBase");

    }
}

public class ReturnToBase : State{
    public override void Enter()
    {
        owner.GetComponent<Arrive>().enabled = true;
        owner.GetComponent<Arrive>().targetGameObject = owner.GetComponent<ShipController>().myBase;
    }

    public override void Think()
    {
        float distance = Vector3.Distance(owner.transform.position, owner.GetComponent<Arrive>().targetGameObject.transform.position);
        if (distance <= 15f)
        {
            owner.GetComponent<StateMachine>().ChangeState(new ExtendHose());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<Arrive>().enabled = false;
    }

}

public class RefuelShip : State{
    public override void Enter()
    {

    }

    public override void Think()
    {

        if(owner.GetComponent<ShipController>().myBase.GetComponent<Base>().tiberium >= 7){
            owner.GetComponent<ShipController>().myBase.GetComponent<Base>().tiberium -= 7;
            owner.GetComponent<ShipController>().tiberium += 7;
            owner.GetComponent<StateMachine>().ChangeState(new RetractHose());
        }
    }

    public override void Exit()
    {
       
    }
}

public class ExtendHose : State{
    public override void Enter()
    {
        owner.GetComponent<ShipController>().lrPoint = owner.transform.position;
        owner.GetComponent<ShipController>().StartCoroutine("LerpLrTo");

        owner.GetComponent<LineRenderer>().enabled = true;
        owner.GetComponent<LineRenderer>().startColor = owner.transform.GetChild(0).GetComponent<Renderer>().material.color;
        owner.GetComponent<LineRenderer>().endColor = owner.transform.GetChild(0).GetComponent<Renderer>().material.color;
    }

    public override void Think()
    {
        if(Vector3.Distance(owner.GetComponent<ShipController>().lrPoint, owner.GetComponent<ShipController>().myBase.transform.position) < 0.5f){
            owner.GetComponent<StateMachine>().ChangeState(new RefuelShip());
        }
    }
    public override void Exit()
    {
        owner.GetComponent<ShipController>().StopCoroutine("LerpLrTo");
    }
}

public class RetractHose : State
{
    public override void Enter()
    {
        owner.GetComponent<ShipController>().StartCoroutine("LerpLrBack");
    }

    public override void Think()
    {
        if (Vector3.Distance(owner.GetComponent<ShipController>().lrPoint, owner.transform.position) < 0.5f)
        {
            owner.GetComponent<StateMachine>().ChangeState(new ArriveAtBase());
        }
    }
    public override void Exit()
    {
        owner.GetComponent<LineRenderer>().enabled = false;
        owner.GetComponent<ShipController>().StopCoroutine("LerpLrBack");
    }
}


public class ShipController : MonoBehaviour
{
    public GameObject bullet;

    public GameObject targetBase;

    public GameObject myBase;

    public float stoppingDistance;


    public int tiberium;

    LineRenderer lr;

    public Vector3 lrPoint;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new ArriveAtBase());
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPositions(new Vector3[] { transform.position, lrPoint });
    }

    public void ShipSetup(Color col, GameObject target, int numberOfTiberium, GameObject home){
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = col;
        }

        targetBase = target;

        tiberium += numberOfTiberium;
        myBase = home;
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

    public IEnumerator LerpLrTo(){
        while (true){
            lrPoint = Vector3.Lerp(lrPoint, myBase.transform.position, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator LerpLrBack()
    {
        while (true)
        {
            lrPoint = Vector3.Lerp(lrPoint, transform.position, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
