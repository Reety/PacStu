using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayableCharacter 
{
    int CurrentState { get; }
    // Start is called before the first frame update
    public void Initialise();
}
