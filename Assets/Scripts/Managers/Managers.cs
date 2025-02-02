using UnityEngine;

public class Managers : MonoBehaviour
{
	static Managers s_instance;
	static Managers Instance { get { Init(); return s_instance; } }

	#region Contents
	GameManagerEx _game = new GameManagerEx();
	HappinessManager _happy = new HappinessManager();
	WorldManager _world = new WorldManager();
	public static GameManagerEx Game { get { return Instance._game; } }
	public static HappinessManager Happy { get { return Instance._happy; } }
	public static WorldManager World { get { return Instance._world; } }
    #endregion

    #region Core
    SceneManagerEx _scene = new SceneManagerEx();
	SoundManager _sound = new SoundManager();
	ResourceManager _resource = new ResourceManager();
	UIManager _ui = new UIManager();
	public static SceneManagerEx Scene { get { return Instance._scene; } }
	public static SoundManager Sound { get { return Instance._sound; } }
	public static ResourceManager Resource { get { return Instance._resource; } }
	public static UIManager UI { get { return Instance._ui; } }
	#endregion

	public static void Init()
	{
		if (s_instance == null)
		{
			GameObject go = GameObject.Find("@Managers");
			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);
			s_instance = go.GetComponent<Managers>();

			s_instance._sound.Init();
			s_instance._happy.Init();
            s_instance._world.Init();
        }
	}

	public static void Clear()
	{
		Scene?.Clear();
		Sound?.Clear();
		UI?.Clear();
	}
}
