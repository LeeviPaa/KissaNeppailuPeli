using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidFuckingAnimationEventShit : MonoBehaviour {

	public void FadeOutComplete()
    {
        LevelInstance li = Toolbox.RegisterComponent<LevelInstance>();
        li.FadeOutComplete();
    }
}
