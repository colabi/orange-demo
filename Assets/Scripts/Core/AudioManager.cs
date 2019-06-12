//FADER COMES FROM ANOTHE PROJECT
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Application {
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager shared;
        public AudioClip[] clips;
        private AudioSource[] _player;
        private AudioSource[] _fx;
        private IEnumerator[] fader = new IEnumerator[2];
        private int ActivePlayer = 0;

        private int volumeChangesPerSecond = 15;

        public float fadeDuration = 10.0f;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _volume = 1.0f;
        public float volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
            }
        }

        public bool mute
        {
            set
            {
                foreach (AudioSource s in _player)
                {
                    s.mute = value;
                }
            }
            get
            {
                return _player[ActivePlayer].mute;
            }
        }

        private void Awake()
        {
            AudioManager.shared = this;
            _player = new AudioSource[2]{
                gameObject.AddComponent<AudioSource>(),
                gameObject.AddComponent<AudioSource>()
            };
            _fx = new AudioSource[2]{
                gameObject.AddComponent<AudioSource>(),
                gameObject.AddComponent<AudioSource>()
            };

            //Set default values
            foreach (AudioSource s in _player)
            {
                s.loop = true;
                s.playOnAwake = false;
                s.volume = 0.0f;
            }
        }

        public void PlayEffect(string fxname, int channelid, float volume = 0.5F)
        {
            foreach (AudioClip clip in clips)
            {
                if (clip.name == fxname)
                {
                    Debug.Log("Audio play: " + fxname);
                    _fx[channelid].clip = clip;
                    _fx[channelid].volume = volume;
                    _fx[channelid].Play();
                }
            }
        }

        public void Play(string cuename)
        {
            //foreach (AudioClip clip in clips)
            //{
            //    if (clip.name == cuename)
            //    {
            //        Debug.Log("Audio play: " + cuename);
            //        Play(clip);
            //    }
            //}
        }

        public void Play(AudioClip clip)
        {
            ////Prevent fading the same clip on both players 
            //if (clip == _player[ActivePlayer].clip)
            //{
            //    return;
            //}
            ////Kill all playing
            //foreach (IEnumerator i in fader)
            //{
            //    if (i != null)
            //    {
            //        StopCoroutine(i);
            //    }
            //}

            ////Fade-out the active play, if it is not silent (eg: first start)
            //if (_player[ActivePlayer].volume > 0)
            //{
            //    fader[0] = FadeAudioSource(_player[ActivePlayer], fadeDuration, 0.0f, () => { fader[0] = null; });
            //    StartCoroutine(fader[0]);
            //}

            ////Fade-in the new clip
            //int NextPlayer = (ActivePlayer + 1) % _player.Length;
            //_player[NextPlayer].clip = clip;
            //_player[NextPlayer].Play();
            //fader[1] = FadeAudioSource(_player[NextPlayer], fadeDuration, volume, () => { fader[1] = null; });
            //StartCoroutine(fader[1]);

            ////Register new active player
            //ActivePlayer = NextPlayer;
        }
     
        //IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, System.Action finishedCallback)
        //{
        //    //Calculate the steps
        //    int Steps = (int)(volumeChangesPerSecond * duration);
        //    float StepTime = duration / Steps;
        //    float StepSize = (targetVolume - player.volume) / Steps;

        //    //Fade now
        //    for (int i = 1; i < Steps; i++)
        //    {
        //        player.volume += StepSize;
        //        yield return new WaitForSeconds(StepTime);
        //    }
        //    //Make sure the targetVolume is set
        //    player.volume = targetVolume;

        //    //Callback
        //    if (finishedCallback != null)
        //    {
        //        finishedCallback();
        //    }
        //}

        public void Configure() {

        }
    }

}
