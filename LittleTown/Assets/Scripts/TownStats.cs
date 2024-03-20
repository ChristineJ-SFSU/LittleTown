using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int townlevel = 0;
    public int villagers = 0;
    public int foodResourceCount = 0;
    public int softwoodCount;
    public int hardwoodCount;
    public int rockCount;
    public List<GameObject> villagerList;
    public List<GameObject> resourceList;
    public List<GameObject> daylyResources;
    public List<GameObject> resourceGrounds;
    public int hour = 0;
    public int minute= 0;

    void Start()
    {
        StartCoroutine(time());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator time(){
    while (true)
    {
        minute++;
        if(minute>59)
        {
            minute = 0;
            hour++;
            newHour();
            if(hour>24){
                hour = 0;
                newDay();
            }
        }
        yield return new WaitForSeconds(.1f);
    }
}
 public void newDay()
 {
    foreach(GameObject villager in villagerList)
    {
    villager.GetComponent<NPCScript>().newDay();
    }
    AddDailyResource();
 }
 public void newHour()
 {
foreach(GameObject villager in villagerList){
    villager.GetComponent<NPCScript>().newHour();
}
 }
void AddDailyResource()
{
    GameObject newResource;
    foreach(GameObject ground in resourceGrounds)
    {
        if( Random.Range(0,10) == 9)
        {
            newResource = Instantiate(daylyResources[Random.Range(0,daylyResources.Count)]);
            newResource.transform.position = ground.transform.position;
            AddResource(newResource);
        }
    }
}
public void RemoveResource(GameObject resource)
{
    Resource res = resource.GetComponent<Resource>();
    InteractingWith type = res.type;
    if (type == InteractingWith.peartree||
    type == InteractingWith.appletree||
    type == InteractingWith.plumtree
    )
    {
       res.ToEmpty();
       res.ammount = 0;

       foodResourceCount--; 
    } 
    if(type == InteractingWith.mushroom)
    {
        foodResourceCount--;
        resourceList.Remove(resource);
        Destroy(resource);
    } 
    if(type == InteractingWith.softwoodtree) 
    {
        softwoodCount--;
        resourceList.Remove(resource);
        Destroy(resource);
        }
        
    if(type == InteractingWith.hardwoodtree) 
    {
        hardwoodCount--;
        resourceList.Remove(resource);
        Destroy(resource);
    }
     if(type == InteractingWith.rock) 
    {
        rockCount--;
        resourceList.Remove(resource);
        Destroy(resource);
    }
    
    
}
public void AddResource(GameObject resource)
{
    InteractingWith type = resource.GetComponent<Resource>().type;
    if (type == InteractingWith.peartree||
    type == InteractingWith.appletree||
    type == InteractingWith.plumtree||
    type == InteractingWith.mushroom
    ) foodResourceCount++;
    if(type == InteractingWith.softwoodtree) softwoodCount++;
    if(type == InteractingWith.hardwoodtree) hardwoodCount++;
    if(type == InteractingWith.rock) rockCount++;
    resourceList.Add(resource);
}
}
