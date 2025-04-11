using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
	AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
	Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

	private GameObject _root = null;
	public void Init()
	{
		if(_root != null)
		{
			return;
		}

		_root = GameObject.Find("@SoundRoot");
		if (_root == null)
		{
            _root = new GameObject { name = "@SoundRoot" };
			UnityEngine.Object.DontDestroyOnLoad(_root);

			string[] soundNames = System.Enum.GetNames(typeof(Sound));
			for (int i = 0; i < soundNames.Length - 1; i++)
			{
				GameObject go = new GameObject { name = soundNames[i] };
				_audioSources[i] = go.AddComponent<AudioSource>();
				go.transform.parent = _root.transform;
			}

			_audioSources[(int)Sound.Bgm].loop = true;
		}
	}

	public void Clear()
	{
		foreach (AudioSource audioSource in _audioSources)
		{
			audioSource.Stop();
			audioSource.clip = null;
		}
		_audioClips.Clear();
	}

    public void Play(string key, Sound type, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)type];

        if (type == Sound.Bgm)
        {
            LoadAudioClip(key, (audioClip) =>
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = audioClip;
                audioSource.Play();
            });
        }
        else if(type == Sound.Effect)
        {
            LoadAudioClip(key, (audioClip) =>
            {
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            });
        }
    }

    public void Stop(Sound type)
    { 
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Stop();
    }

    private void LoadAudioClip(string key, Action<AudioClip> callback)
    {
        AudioClip audioClip = null;
        if (_audioClips.TryGetValue(key, out audioClip))
        {
            callback?.Invoke(audioClip);
            return;
        }

        audioClip = Managers.Resource.Load<AudioClip>(key);

        if (!_audioClips.ContainsKey(key))
            _audioClips.Add(key, audioClip);

        callback?.Invoke(audioClip);
    }
}
