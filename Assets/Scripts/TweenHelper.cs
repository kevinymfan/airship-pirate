using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TweenHelper
{
    [SerializeField]
    public float animateDuration = 1f;
    [SerializeField]
    public float delay = 0f;
    [SerializeField]
    public AnimationCurve curve;
    [SerializeField]
    public bool loop = false;
    private float animTime = 0f;

    [HideInInspector]
    public Vector3 targetVec = Vector3.zero;
    [HideInInspector]
    public Vector3 startVec = Vector3.zero;
    [HideInInspector]
    public float targetVal = 0f;
    [HideInInspector]
    public float startVal = 0f;

    public bool isAnimating()
    {
        return animTime < (animateDuration + delay);
    }

    public bool isDone()
    {
        return !this.isAnimating();
    }

    public void reset()
    {
        this.animTime = 0f;
    }

    public void addTime(float deltaTime)
    {
        this.animTime += deltaTime;
        if (this.loop && this.animTime > this.animateDuration) {
            this.animTime -= this.animateDuration;
        }
    }

    public Vector3 getTweenVec()
    {
        if (this.animTime <= this.delay)
        {
            return new Vector3(this.startVec.x, this.startVec.y, this.startVec.z);
        }
        float time = (this.animTime - this.delay) / this.animateDuration;
        return Vector3.LerpUnclamped(
            this.startVec,
            this.targetVec,
            curve.Evaluate(time)
        );
    }

    public float getTweenVal()
    {
        if (this.animTime <= this.delay)
        {
            return this.startVal;
        }
        float time = (this.animTime - this.delay) / this.animateDuration;
        return Mathf.LerpUnclamped(
            this.startVal,
            this.targetVal,
            curve.Evaluate(time)
        );
    }
}