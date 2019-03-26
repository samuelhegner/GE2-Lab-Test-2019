using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float tiberium = 0;

    public TextMeshPro text;

    public GameObject fighterPrefab;

    Color colToAssign;

    public List<GameObject> otherBases = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        colToAssign = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = colToAssign;
            
        }

        GameObject[] _allBases = GameObject.FindGameObjectsWithTag("base");

        foreach(GameObject _base in _allBases){
            if(_base != gameObject){
                otherBases.Add(_base);
            }
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
        GameObject newShip = Instantiate(fighterPrefab, transform.position, Quaternion.identity);
        newShip.GetComponent<ShipController>().ShipSetup(colToAssign, otherBases[Random.Range(0, 3)]);
        tiberium -= 10;
    }


}
