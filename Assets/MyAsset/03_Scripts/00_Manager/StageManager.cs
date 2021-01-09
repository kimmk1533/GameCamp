using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // Start is called before the first frame update
    int stage;
    
    public struct Stage_0
    {
        public bool havekey;
        public bool havefire;
        public bool clear;

        Stage_0(bool key,bool fire,bool clear)
        {
            this.havekey = key;
            this.havefire = fire;
            this.clear = clear;
        }
    }
    public Stage_0 stage_zero;
    void Start()
    {
        
    }

    public override void __Initialize()
    {
        stage = 0;
    }

    // Update is called once per frame


}
