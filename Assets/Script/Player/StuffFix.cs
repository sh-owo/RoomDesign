using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffFix : MonoBehaviour
{
    StuffPlace stuffPlace;
    private int moveMode = 1; //1: 기본, 2: 물건이동, 3: 물건회전
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Keypad1)) { moveMode = 1; }
        if(Input.GetKey(KeyCode.Keypad2)) { moveMode = 2; }
        if(Input.GetKey(KeyCode.Keypad3)) { moveMode = 3; }
        if(moveMode == 1) { DefaultMode(); }
        if(moveMode == 2) { MoveMode(); }
        if(moveMode == 3) { RotateMode(); }
    }

    void DefaultMode()
    {
        stuffPlace = GetComponent<StuffPlace>();
        
    }
    
    void MoveMode()
    {
        // stuffPlace = GetComponent<StuffPlace>();
        // stuffPlace.PutStuff();
    }
    
    void RotateMode()
    {
        // stuffPlace = GetComponent<StuffPlace>();
        // stuffPlace.RotateStuff();
    }
}
