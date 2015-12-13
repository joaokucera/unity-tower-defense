using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Sound Manager")]
	public class SoundManager : Singleton<SoundManager>
	{
		private Dictionary<string, AudioClip> m_sfxDictionary;

		[Header("Audio Sources")]
		[SerializeField] private AudioSource m_musicSource;
		[SerializeField] private AudioSource m_sfxSource;

		[Header("Sound Effects Collection")]
		[SerializeField] private AudioClip[] sfxClips;

		void Start()
		{
			if (m_sfxDictionary == null)
			{
				CreateSoundDictionary();
				
				StartCoroutine(PlayMusic());
			}
		}

		public static void PlaySoundEffect(string clipName)
		{
			AudioClip originalClip;
			
			if (Instance.m_sfxDictionary.TryGetValue(clipName, out originalClip))
			{
				Instance.MakeSoundEffect(originalClip);
			}
		}
		
		private void CreateSoundDictionary()
		{
			m_sfxDictionary = new Dictionary<string, AudioClip>();

			for (int i = 0; i < sfxClips.Length; i++)
			{
				m_sfxDictionary.Add(sfxClips[i].name, sfxClips[i]);
			}
		}
		
		private IEnumerator PlayMusic()
		{
			var originalVolume = m_musicSource.volume;

			m_musicSource.volume = 0f;
			m_musicSource.loop = true;
			m_musicSource.Play();
			
			while (m_musicSource.volume < originalVolume)
			{
				m_musicSource.volume += Time.deltaTime;
				
				yield return null;
			}
		}
		
		private void MakeSoundEffect(AudioClip originalClip)
		{
			m_sfxSource.PlayOneShot(originalClip);
		}
	}
}