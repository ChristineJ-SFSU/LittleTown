using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateGround : MonoBehaviour
{
    public GameObject lotGrass;
    public GameObject treeSoft;
    public GameObject treeHard;

    public List<GameObject> food;
    public TownStats townStats;
    // Start is called before the first frame update
    void Start()
    {
        if(!townStats)
        {
            townStats = GameObject.FindWithTag("town").GetComponent<TownStats>();
        }
        int rand = -1;
        GameObject newResource;
        for(int x =-11; x < 11; x += 2){
            for(int z = -11; z<11; z += 2){
                newResource = Instantiate(lotGrass, gameObject.transform);
                newResource.transform.position = new Vector3(x,0,z);
                townStats.resourceGrounds.Add(newResource);
                rand = Random.Range(0,11);
                
                if( rand == 10)
                {
                   newResource = Instantiate(treeHard, gameObject.transform);
                   newResource.transform.position = new Vector3(x,0,z);
                   townStats.AddResource(newResource);
                }
                if (rand == 9)
                {
                    newResource = Instantiate(treeSoft, gameObject.transform);
                    newResource.transform.position = new Vector3(x,0,z);
                    townStats.AddResource(newResource);
                }if(rand <2){
                    newResource = Instantiate(food[Random.Range(0,food.Count)], gameObject.transform);
                    newResource.transform.position = new Vector3(x,0,z);
                    townStats.AddResource(newResource);
                }
        }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
