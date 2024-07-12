﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SkeletonAnimation))]
public class SpineCharacter : MonoBehaviour
{
    [SerializeField, Tooltip("动画角色骨骼")] protected SkeletonAnimation character;
    [SerializeField, Tooltip("用于随机动画播放的动画列表")] protected List<SpineAnimation> randomAnimationList;
    [Header("固定动画设置")]
    [SerializeField, Tooltip("出现动画列表")] protected List<string> appearAnimNames;
    [SerializeField, Tooltip("消失动画列表")] protected List<string> disappearAnimNames;
    
    
    public void Initialize()
    {
        character = this.GetComponent<SkeletonAnimation>();
        character.AnimationState.Complete += CompleteEvent;//注册动画回调事件函数
    }
    
    #region 动画播放
    private float PlayAnimation(string animName, bool loop)
    {
        character.AnimationState.ClearTracks();
        character.AnimationState.SetAnimation(0, animName, loop);
        return character.skeleton.Data.FindAnimation(animName).Duration;
    }

    public float PlayAnimationAppear()
    {
        //return PlayGroupAnim(appearAnimNames);
        return PlayAnimation(appearAnimNames[0], false);
    }

    public float PlayAnimationDisAppear()
    {
        //return PlayGroupAnim(disappearAnimNames);
        return PlayAnimation(disappearAnimNames[0], false);
    }
    public float PlayAnimationIdle() { return PlayAnimation("Idle", false); }
    
    public virtual float PlayAnimationClick() { return 0; }
    
    public virtual float PlayAnimationDrag() { return 0; }

    public void PlayRandomAnimation()
    {
        var index = Random.Range(0, randomAnimationList.Count);
        PlayGroupAnim(randomAnimationList[index].animName);
    }
    private float PlayGroupAnim(List<string> animationNames)
    {
        var duration = appearAnimNames.Sum(animName => character.skeleton.Data.FindAnimation(animName).Duration);
        StartCoroutine(PlayAnimationsInSequence(animationNames));
        return duration;
    }
    private IEnumerator PlayAnimationsInSequence(List<string> animationNames)//播放动画序列协程
    {
        foreach (var animName in animationNames)
        {
            // 播放动画并获取其持续时间
            float duration = PlayAnimation(animName, false);
            //走动动画的特殊效果
            if (animName == "Move") { StartCoroutine(MoveAnim(duration)); }
            // 等待动画播放完毕
            yield return new WaitForSeconds(duration);
        }
    }
    private IEnumerator MoveAnim(float duration)
    {
        Debug.Log(duration);
        for (int i = 0; i < 4; i++)
        {
            
            yield return new WaitForSeconds(duration/4);
        }
    }
    #endregion
    
    /*定义动画回调事件函数*/
    protected virtual void CompleteEvent(Spine.TrackEntry trackEntry)
    {
        //TODO:需要修改，不是点击动画播放完成后切换回待机动画
        character.AnimationState.SetAnimation(0, "Idle", true);
    }
}

[Serializable]
public class SpineAnimation
{
    public List<string> animName;
}

