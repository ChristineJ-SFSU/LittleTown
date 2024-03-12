using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
 public InteractingWith type;
 public bool isEmpty = false;
 public int ammount; //count for food items value/ bonus for interative items
 public int growthAmmount;

 public void ToEmpty(){
    gameObject.transform.GetChild(0).gameObject.SetActive(true);
    gameObject.transform.GetChild(1).gameObject.SetActive(false);
    isEmpty = true;
 }
 public void ToFull(){
    gameObject.transform.GetChild(0).gameObject.SetActive(false);
    gameObject.transform.GetChild(1).gameObject.SetActive(true);
    isEmpty = false;
 }
 public void AddAmount(){
     if(isEmpty) ToFull();
     ammount += growthAmmount;
 }
}
