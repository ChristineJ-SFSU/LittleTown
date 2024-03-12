using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dictionaries : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int rrr;
 
[System. Serializable]
    public class ActionSprites{
        public Actions action;
        public Material sprite;
    }
    [SerializeField] private List<ActionSprites> actionSpritesList;

    public Dictionary<Actions,Material> actionSprite;
    void Awake(){
        actionSprite = new Dictionary<Actions, Material>();
        foreach( ActionSprites entry in actionSpritesList){
            actionSprite.Add(entry.action,entry.sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
