using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sascha {
    public class SoundManager {

        private static SoundManager _instance = null;
        private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
        private AudioSource audioSource;

        public static SoundManager instance {
            get {
                if(_instance == null)
                    _instance = new SoundManager();

                return _instance;
            }
        }

        public void PlaySound(string name) {
            if(sounds.ContainsKey(name)) {
                audioSource.PlayOneShot(sounds[name]);
            } else {
                Debug.LogError("Sound not found: " + name + "in folder 'Assets/Resources/sounds'");
            }
        }

        private SoundManager() {

            AudioClip[] audioClips = Resources.LoadAll<AudioClip>("sounds") as AudioClip[];
            for(int i = 0; i < audioClips.Length; i++) {
                sounds.Add(audioClips[i].name, audioClips[i]);
            }

            audioSource = new GameObject().AddComponent<AudioSource>();
        }
    }
}