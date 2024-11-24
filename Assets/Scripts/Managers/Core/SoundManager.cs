using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
	AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
	Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

	public void Init()
	{
		GameObject root = GameObject.Find("@Sound");
		if (root == null)
		{
			root = new GameObject { name = "@Sound" };
			Object.DontDestroyOnLoad(root);

			string[] soundNames = System.Enum.GetNames(typeof(Sound));
			for (int i = 0; i < soundNames.Length - 1; i++)
			{
				GameObject go = new GameObject { name = soundNames[i] };
				_audioSources[i] = go.AddComponent<AudioSource>();
				go.transform.parent = root.transform;
			}

			_audioSources[(int)Sound.Bgm].loop = true;
		}
	}

	public void Clear()
	{
		foreach (AudioSource audioSource in _audioSources)
		{
			audioSource.clip = null;
			audioSource.Stop();
		}
		_audioClips.Clear();
	}

	public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
	{
		AudioClip audioClip = GetOrAddAudioClip(path, type);
		Play(audioClip, type, pitch);
	}

	public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
	{
		if (audioClip == null)
			return;

		if (type == Sound.Bgm)
		{
			AudioSource audioSource = _audioSources[(int)Sound.Bgm];
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.pitch = pitch;
			audioSource.clip = audioClip;
			audioSource.Play();
		}
		else
		{
			AudioSource audioSource = _audioSources[(int)Sound.Effect];
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(audioClip);
		}
	}

	AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
	{
		if (path.Contains("Sounds/") == false)
			path = $"Sounds/{path}";

		AudioClip audioClip = null;

		if (type == Sound.Bgm)
		{
			audioClip = Resources.Load<AudioClip>(path);
		}
		else
		{
			if (_audioClips.TryGetValue(path, out audioClip) == false)
			{
				audioClip = Resources.Load<AudioClip>(path);
				_audioClips.Add(path, audioClip);
			}
		}

		if (audioClip == null)
			Debug.Log($"AudioClip Missing ! {path}");

		return audioClip;
	}
}
