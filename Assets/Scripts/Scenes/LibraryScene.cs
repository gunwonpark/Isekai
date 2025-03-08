using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LibraryScene : BaseScene
{
    [SerializeField] Material material;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] GameObject[] books;

	[SerializeField] PlayableDirector startTimeLine;

    protected override void Init()
	{
		base.Init();

		SceneType = Scene.LibraryScene;
		//Managers.UI.ShowPopupUI<UI_BookPopup>();
		//Managers.World.CurrentWorldType = WorldType.Vinter;

		Managers.DB.SetPlayerData(
			new PlayerData
			{
				moveSpeed = new List<float> { 1f, 1f, 1f }
			});


        startTimeLine.stopped += LibrayLightSwitch;
    }

    public void LibrayLightSwitch(PlayableDirector director)
    {
        meshRenderer.material = material;
        BookSwitch();
        
    }

    private void BookSwitch()
    {
        WorldType currentWorldType = Managers.World.CurrentWorldType;
        
        switch (currentWorldType)
        {
            case WorldType.Vinter:
                books[0].GetComponent<SpriteClickHandler>().StartBlink();
                books[0].GetComponent<BoxCollider2D>().enabled = true;
                break;
            case WorldType.Chaumm:
                books[1].GetComponent<SpriteClickHandler>().StartBlink();
                books[1].GetComponent<BoxCollider2D>().enabled = true;
                break;
            case WorldType.Gang:
                books[2].GetComponent<SpriteClickHandler>().StartBlink();
                books[2].GetComponent<BoxCollider2D>().enabled = true;
                break;
            case WorldType.Pelmanus:
                books[3].GetComponent<SpriteClickHandler>().StartBlink();
                books[3].GetComponent<BoxCollider2D>().enabled = true;
                break;
        }
    }

    public override void Clear()
	{
		Managers.DB.ResetPlayerData();
        startTimeLine.stopped -= LibrayLightSwitch;
    }
}
