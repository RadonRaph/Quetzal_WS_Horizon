using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterAudio
{
    [System.Serializable]
    public partial class Follow : BetterAudioBehaviour, IUpdateBehaviour, IStartBehaviour, IPlayBehaviour, IEndBehaviour
    {
        [SerializeField]
        public SceneTransform transform;

        [SerializeField]
        public bool KeepOffset;

        Vector3 offset = Vector3.zero;

        bool init = false;

        public override void GUI(Rect rect)
        {
#if UNITY_EDITOR
            transform.DrawGUI();

            KeepOffset = EditorGUILayout.Toggle("Keep offset", KeepOffset);
#endif
        }

        public override void Init(BetterAudioSource m)
        {
            base.Init(m);
            Name = "Follow";
            Priority = 100;
            transform = new SceneTransform("Target Transform");
        }

        public void Action(BetterAudioSource caller)
        {
            if (!init && KeepOffset)
            {
                offset = transform.target.position - caller.transform.position;
                init = true;
            }

            caller.transform.position = transform.target.position + offset;
        }

        public void OnUpdate(BetterAudioSource caller, bool playing, bool clip)
        {
            Action(caller);
        }

        public void OnStart(BetterAudioSource caller)
        {
            Action(caller);
        }

        public void OnPlay(BetterAudioSource caller)
        {
            Action(caller);
        }

        public void OnEnd(BetterAudioSource caller)
        {
            Action(caller);
        }
    }
}
