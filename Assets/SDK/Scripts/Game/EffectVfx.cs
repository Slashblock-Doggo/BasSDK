﻿using UnityEngine;
using System;
using UnityEngine.VFX;

namespace BS
{
    [ExecuteInEditMode]
    public class EffectVfx : Effect
    {
        public VisualEffect vfx;
        public float lifeTime = 5;
        public Transform targetTransform;

        protected bool stopping;
        protected bool hasTarget;
        protected int positionId;
        protected int positionAngles;
        protected int positionScale;

        private void OnValidate()
        {
            vfx = this.GetComponent<VisualEffect>();
            SetTarget(targetTransform);
        }

        private void Awake()
        {
            vfx = this.GetComponent<VisualEffect>();
            if (!vfx) vfx = this.gameObject.AddComponent<VisualEffect>();
            vfx.enabled = false;
        }

        public override void Play()
        {
            vfx.enabled = true;
            vfx.Play();
            if (step != Step.Loop)
            {
                Invoke("Despawn", lifeTime);
            }
        }

        public override void Stop()
        {
            vfx.Stop();
            stopping = true;
        }

        public override void SetIntensity(float value)
        {
            vfx.SetFloat("Intensity", value);
        }

        public override void SetMainGradient(Gradient gradient)
        {
            if (gradient != null && vfx.HasGradient("MainGradient")) vfx.SetGradient("MainGradient", gradient);
            else vfx.ResetOverride("MainGradient");
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            if (gradient != null && vfx.HasGradient("SecondaryGradient")) vfx.SetGradient("SecondaryGradient", gradient);
            else vfx.ResetOverride("SecondaryGradient");
        }

        public override void SetMesh(Mesh mesh)
        {
            vfx.SetMesh("Mesh", mesh);
        }

        public override void SetRenderer(Renderer renderer, bool secondary)
        {
            //vfx.SetMesh("Mesh", renderer.GetComponent<MeshFilter>().sharedMesh);
            // Todo (waiting Mesh sampling) https://forum.unity.com/threads/visual-effect-graph-spawn-particles-from-skinned-mesh-surface.651748/
        }

        public override void SetTarget(Transform target)
        {
            targetTransform = target;
            if (target && vfx.HasVector3("Target_position"))
            {
                positionId = Shader.PropertyToID("Target_position");
                positionAngles = Shader.PropertyToID("Target_angles");
                positionScale = Shader.PropertyToID("Target_scale");
                hasTarget = true;
                Update();
            }
            else
            {
                hasTarget = false;
            }
        }

        public void Update()
        {
            if (hasTarget)
            {
                vfx.SetVector3(positionId, targetTransform.position);
                vfx.SetVector3(positionAngles, targetTransform.eulerAngles);
                vfx.SetVector3(positionScale, targetTransform.localScale);
            }
            if (stopping && vfx.aliveParticleCount == 0)
            {
                stopping = false;
                Despawn();
            }
        }

        public override void Despawn()
        {
            CancelInvoke();
            vfx.Stop();
            vfx.enabled = false;
#if ProjectCore
            if (Application.isPlaying)
            {
                EffectInstance orgEffectInstance = effectInstance;
                effectInstance = null;
                EffectModuleVfx.Despawn(this);
                orgEffectInstance.OnEffectDespawn();
            }
#endif
        }
    }
}
