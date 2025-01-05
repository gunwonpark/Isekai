using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneDirector : MonoBehaviour
{
	void Start()
    {
		Managers.UI.ShowPopupUI<UI_CutScene1Popup>();
		
		//Managers.Resource.Instantiate("Cutscene/after cutscene Variant");
	
	}
}
