using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Start is called before the first frame update
    public enum Actions
    {
        None,
        Gathering,
        Hunting,
        Logging,
        Mining,
        Construction,
        Sleep,
        Ponder,
        Eat
    }

    public enum PersonSkills
    {
        Gathering,
        GatheringHerbs,
        GatheringFood,
        Hunting,
        Logging,
        Mining,
        Construction
    } 

    public enum InteractingWith{
        mushroom,
        appletree,
        peartree,
        plumtree,
        bed,
        goodbed,
        softwoodtree,
        hardwoodtree,
        rock

    }
    public enum CreateItems{
        hammer,
        axe,
        house


    }

    public enum Items{
        softwood,
        hardwood,
        hammer,
        axe,
        apple,
        pear,
        plum,
        mushroom,
        rock
    }
