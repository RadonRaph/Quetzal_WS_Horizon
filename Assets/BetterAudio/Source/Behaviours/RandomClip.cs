using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterAudio
{
    [System.Serializable]
    public partial class RandomClip : BetterAudioBehaviour, IStartBehaviour, IPlayBehaviour, IEndBehaviour
    {
        [SerializeField]
        [SerializeReference]
        BetterArrayAudioClip Clips;

        public override void GUI(Rect rect)
        {
#if UNITY_EDITOR

            Clips.OnGUI(Main);

            
#endif
        }

        

        public override void Init(BetterAudioSource m)
        {
            base.Init(m);
            Name = "Random Clip";
            Priority = 10;
            Clips = new BetterArrayAudioClip(false);
            

        }

        public void Action(BetterAudioSource caller)
        {
            if (Clips.items.Count > 0)
                caller.source.clip = (AudioClip)Clips.items[Random.Range(0, Clips.items.Count)];
        }

        public void OnEnd(BetterAudioSource caller)
        {
            Action(caller);
        }

        public void OnPlay(BetterAudioSource caller)
        {
            Action(caller);
        }

        public void OnStart(BetterAudioSource caller)
        {
            Action(caller);
        }
    }
}
