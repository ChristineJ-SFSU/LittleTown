using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    
    public TownStats townStats; //info about the town
    public Dictionaries dictionaries;
    
    //currect action infomation
    public Actions action;
    public GameObject destination;
    public bool canAct,acting;
    public GameObject resource;
    public Resource resourceInfo;
    private int lastTime;
    private int check = 0;
    [SerializeField] private int ponderTime=0;
    private Dictionary <Items,int> inventory;

    //NPCstats
    public int Hunger 
    { 
        get
        {
        return hunger;
        }
        set{
        
         hunger = value;
         if (hunger < 1) hunger = 0; 

        }
    }
    public int hunger;
    public int Sleep 
    { 
        get
        {
        return sleep;
        }
        set{
        sleep = value;
        if(sleep < 1) sleep = 0;

        }
    }
    public int sleep;
    public int Comfort  { 
        get
        {
        return comfort;
        }
        set{
         comfort = value;
         if(comfort < 1) comfort =0;

        }
    }
    public int comfort;
    public GameObject bed;

    public GameObject house;
    public Dictionary<PersonSkills,int> skills;
    GameObject note;

    //invetroy
    public int poundsOfFood = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(!townStats)
        {
            townStats = GameObject.FindWithTag("town").GetComponent<TownStats>();
        }
        if(!dictionaries)
        {
            dictionaries = GameObject.FindWithTag("GameController").GetComponent<Dictionaries>();
        }
        townStats.villagers++;
        townStats.villagerList.Add(gameObject);
        skills = new Dictionary<PersonSkills, int>();
        skills.Add(PersonSkills.Gathering,0);
        skills.Add(PersonSkills.Logging,0);
        skills.Add(PersonSkills.Mining,0);
        note = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        inventory = new Dictionary<Items, int>(); 
        foreach( Items item in Enum.GetValues(typeof (Items))){
            inventory.Add(item,0);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (!destination && action == Actions.None){
            action = NextAction();
            destination = NextDestination(action);
            
        }
        if(destination)
        {
          var step =  .5f * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step); 
        
            if (Vector3.Distance(transform.position, destination.transform.position) < .5f)
            {
                print("at des");
            resource = destination;
            resourceInfo = resource.GetComponent<Resource>();
            destination = null;
            canAct = true;
            
            }
        }
        if (canAct)
        {
            lastTime = townStats.minute;
            note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[action];
            acting = true;
            canAct = false;
        }
        if(!destination &&!canAct & !acting & action != Actions.None){
            action = Actions.None;
        }
        if(acting){
          if(lastTime != townStats.minute){
            lastTime = townStats.minute;
            check = doAction(action);
            if(check == -1){
                action = Actions.None;
                resource = null;
                resourceInfo = null;
                acting =false;
            }
          }
        }
    }

    private Actions NextAction(){
        if (hunger<100 && poundsOfFood > 0) return Actions.Eat;
        if (poundsOfFood < 10 && townStats.foodResourceCount>0) return Actions.Gathering;
        if( sleep<50 || (sleep < hunger)) return Actions.Sleep;
        if(sleep >75 && hunger > 75 )
        {
            if(0 == UnityEngine.Random.Range(0,2))
            {
                return Actions.Logging;
            }else
            {
                return Actions.Mining;
            }
        }
        return Actions.Ponder;
    }
    private GameObject NextDestination(Actions nextAction)
    {
        GameObject nextDestination = null;
        if( nextAction == Actions.Gathering)
        {
            foreach (GameObject resource in townStats.resourceList)
            {
                if( resource.tag == "food" && resource.GetComponent<Resource>().ammount > 0)
                {
                    if(!nextDestination)
                    {
                        nextDestination = resource;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, resource.transform.position) < Vector3.Distance(transform.position,nextDestination.transform.position))
                        {
                            nextDestination = resource;
                        }
                    }
                }
            }
        }
        if(nextAction == Actions.Sleep){
            if(bed) return bed;
            
            if(house)return house;
            canAct = true;
        }
        if (nextAction == Actions.Ponder)
        {
            canAct = true;
            
            if(house)return house;
        }
        if(nextAction == Actions.Eat){
            canAct = true;
        }
        if(nextAction == Actions.Logging)
        {
            int skillLevel =(int) Math.Log(skills[PersonSkills.Logging],2);
            foreach (GameObject resource in townStats.resourceList)
            {

                Resource res = resource.GetComponent<Resource>();
                if( resource.tag == "wood" && res.ammount > 0)
                {
                    if(res.type == InteractingWith.hardwoodtree && skillLevel < 5) {}
                    else {
                    if(!nextDestination)
                    {
                        nextDestination = resource;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, resource.transform.position) < Vector3.Distance(transform.position,nextDestination.transform.position))
                        {
                            nextDestination = resource;
                        }
                    }
                    }
                }
            }
        }
         if(nextAction == Actions.Mining)
        {
            foreach (GameObject resource in townStats.resourceList)
            {

                Resource res = resource.GetComponent<Resource>();
                if( resource.tag == "rock" && res.ammount > 0)
                {
                    
                    if(!nextDestination)
                    {
                        nextDestination = resource;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, resource.transform.position) < Vector3.Distance(transform.position,nextDestination.transform.position))
                        {
                            nextDestination = resource;
                        }
                    }
                }
            }
        }
        return nextDestination;
        
    }

 
    public int doAction(Actions action){
        //print("doaction");
        if(action == Actions.Gathering) return Gathering();
        if(action == Actions.Eat) return Eating();
        if(action == Actions.Ponder) return Pondering();
        if(action == Actions.Sleep) return Sleeping();
        if(action == Actions.Logging) return Logging();
        if(action == Actions.Mining) return Mining();
        return -1;
    }

    public int Gathering()
    {
       // print("gathering");
        //print(resourceInfo.ammount);
        if(resourceInfo.ammount > 0)
        {
            resourceInfo.ammount --;
            if(skills.ContainsKey(PersonSkills.Gathering)){
            skills[PersonSkills.Gathering] += 1;
            }
         else{
            skills.Add(PersonSkills.Gathering,0);
         }
            if( resourceInfo.type == InteractingWith.mushroom || 
                resourceInfo.type == InteractingWith.appletree || 
                resourceInfo.type == InteractingWith.peartree ||
                resourceInfo.type == InteractingWith.plumtree)
            {
                poundsOfFood ++;
                if(skills.ContainsKey(PersonSkills.GatheringFood))
                {
                    skills[PersonSkills.GatheringFood] += 1;
                }
                else
                {
                    skills.Add(PersonSkills.GatheringFood,0);
                }
                if( resourceInfo.type == InteractingWith.mushroom ) inventory[Items.mushroom] ++;
                if( resourceInfo.type == InteractingWith.appletree ) inventory[Items.apple] ++;
                if( resourceInfo.type == InteractingWith.peartree ) inventory[Items.pear] ++;
                if( resourceInfo.type == InteractingWith.plumtree ) inventory[Items.plum] ++;
            }
            return 0;
        }else
        {
            townStats.RemoveResource(resource);
            note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[Actions.None];
            return -1;
        }
    }

    public int Eating(){
        //print("eating");
        if(poundsOfFood>0 && hunger < 100){
            poundsOfFood--;
            if(inventory[Items.apple]>0) inventory[Items.apple]--;
            else if(inventory[Items.plum]>0) inventory[Items.plum]--;
            else if(inventory[Items.pear]>0) inventory[Items.pear]--;
            else if(inventory[Items.mushroom]>0) inventory[Items.mushroom]--;
            hunger += 5;
            return 0;
        }
        note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[Actions.None];
        return -1;
    }
    public int Pondering(){
        //print("pondering");
        ponderTime++;
        int random = UnityEngine.Random.Range(0,100);
        int test = 0;
        if(ponderTime < 60){
          return 0;  
        } 
        if(!house &&!skills.ContainsKey(PersonSkills.Logging)){
            test = 100 - comfort;
            if(test > random){
               skills.Add(PersonSkills.Logging,0); 
               print("learned logging!");
            }

        }
        else if(!house &&!skills.ContainsKey(PersonSkills.Construction))
        {
            test = (int) Math.Log(skills[PersonSkills.Logging],2);
            test = 100-(10-test)^2;
            if(test > random){
               skills.Add(PersonSkills.Construction,0); 
               print("learned Construction!");
            }
        }
        else if(skills.ContainsKey(PersonSkills.Gathering) && (!skills.ContainsKey(PersonSkills.GatheringFood) || !skills.ContainsKey(PersonSkills.GatheringHerbs)))
        {
            
            test = (int) Math.Log(skills[PersonSkills.Gathering],2);
            test = 100-(10-test)^2;
            if(test>random){
                if(!skills.ContainsKey(PersonSkills.GatheringFood)){
                    skills.Add(PersonSkills.GatheringFood,0);
                    print("learned GatheringFood!");
                }
                else if(!skills.ContainsKey(PersonSkills.GatheringHerbs)){
                    skills.Add(PersonSkills.GatheringHerbs,0);
                    print("learned GatheringHerbs!");
                }
            }
        }
        note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[Actions.None];
        ponderTime = 0;
        return -1;
    }

    public int Sleeping(){
        if(sleep < 100){
            if(!resource){
                comfort--;
            }else{
                sleep += resourceInfo.ammount;
                comfort += resourceInfo.ammount;
            }
            sleep++;
            return 0;
        }
        note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[Actions.None];
        return -1;
    }

    public int Logging(){
        print("in Logging");
        print(resourceInfo);
        if(resourceInfo.ammount > 0)
        {
            resourceInfo.ammount --;
            if(skills.ContainsKey(PersonSkills.Logging)){
            skills[PersonSkills.Logging] += 1;
            }
         else{
            return -1;
         }
            if( resourceInfo.type == InteractingWith.softwoodtree )
            {
                inventory[Items.softwood]++;
                print($"{inventory[Items.softwood]} count");
            }
            if( resourceInfo.type == InteractingWith.hardwoodtree )
            {
                inventory[Items.hardwood]++;
            }
            return 0;
        }else
        {
            townStats.RemoveResource(resource);
            note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[Actions.None];
            return -1;
        }
    }
    public int Mining(){
        print("in mining");
        print(resourceInfo);
        if(resourceInfo.ammount > 0)
        {
            resourceInfo.ammount --;
            if(skills.ContainsKey(PersonSkills.Mining)){
            skills[PersonSkills.Mining] += 1;
            }
         else{
            return -1;
         }
            inventory[Items.rock]++;
            print($"{inventory[Items.rock]} count");
            return 0;
        }else
        {
            townStats.RemoveResource(resource);
            note.GetComponent<MeshRenderer>().material = dictionaries.actionSprite[Actions.None];
            return -1;
        }
    }
   public void newDay()
    {
        if(!house) comfort -=5;
        hunger -= 25;
    }
    public void newHour()
    {
        
        sleep --;
    }
}
