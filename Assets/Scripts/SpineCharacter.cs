using System;
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
    /*protected bool FindAnim(string name)
    {
        return animationList.Any(anim => anim.animName == name);
    }*/

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

    public float PlayRandomAnimation()
    {
        var index = Random.Range(0, randomAnimationList.Count);
        PlayAnimation(randomAnimationList[index].animName, false);
        return randomAnimationList[index].animTime;
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
    public string animName;
    public float animTime;
}

