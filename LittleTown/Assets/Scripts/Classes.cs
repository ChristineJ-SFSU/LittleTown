using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Skills
{
    public PersonSkills skill;
    public  uint experience; 
    
    public Skills(PersonSkills ps, int ex)
    {
        this.skill = ps;

        this.experience = (uint)Math.Abs(ex);
    }

    public int GetLevel()
    {
        return (int)Math.Log(experience,2);
    }
    public void AddExperience(int ex){
        int value = (int)experience + ex;
        if(value >= 0)
        {
            
            experience = (uint)value;
        }else
        {
            experience = 0;
        }
    }


}