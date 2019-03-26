using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float tiberium = 0;

    public TextMeshPro text;

    public GameObject fighterPrefab;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        }
        StartCoroutine(MineTiberium());

        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + tiberium;

        if(tiberium >= 10){
            SpawnShip();
        }
    }

    IEnumerator MineTiberium(){
        while(true){
            yield return new WaitForSeconds(1f);
            tiberium += 1;
        }
    }

    void SpawnShip(){
        Instantiate(fighterPrefab, transform.position, Quaternion.identity);
        tiberium -= 10;
    }


}
