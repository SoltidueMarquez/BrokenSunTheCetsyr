using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SkeletonAnimation))]
public class SpineCharacter : MonoBehaviour
{
    [SerializeField, Tooltip("动画角色骨骼")] protected SkeletonAnimation character;
    [SerializeField, Tooltip("用于随机动画播放的动画列表")] protected List<SpineAnimation> randomAnimationList;
    
    public void Initialize()
    {
        character = this.GetComponent<SkeletonAnimation>();
        character.AnimationState.Complete += CompleteEvent;//注册动画回调事件函数
    }
    
    #region 动画播放
    protected float PlayAnimation(string animName, bool loop)
    {
        character.AnimationState.ClearTracks();
        character.AnimationState.SetAnimation(0, animName, loop);
        return character.skeleton.Data.FindAnimation(animName).Duration;
    }
    
    public virtual float PlayAnimationAppear() { return 0; }

    public virtual float PlayAnimationDisAppear() { return 0; }
    
    public virtual float PlayAnimationIdle() { return 0; }
    
    public virtual float PlayAnimationClick() { return 0; }
    
    public virtual float PlayAnimationDrag() { return 0; }

    public void PlayRandomAnimation()
    {
        var index = Random.Range(0, randomAnimationList.Count);
        PlayAnimGroup(randomAnimationList[index].animName);
    }
    private void PlayAnimGroup(List<string> animGroup)//播放动画序列
    {
        StartCoroutine(PlayAnimationsInSequence(animGroup));
    }
    private IEnumerator PlayAnimationsInSequence(List<string> animationNames)
    {
        for (int i = 0; i < animationNames.Count; i++)//循环播放动画
        {
            // 播放动画并获取其持续时间
            float duration = PlayAnimation(animationNames[i], false);

            // 等待动画播放完毕
            yield return new WaitForSeconds(duration);
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
    //public float animTime;
}

