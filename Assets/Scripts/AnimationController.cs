using Spine.Unity;
using UnityEngine;

public enum CharacterAnimation
{
    Idle,
    Move,
    Skill,
    Die,
    Default
}
public class AnimationController : Singleton<AnimationController>
{
    [SerializeField, Tooltip("动画角色骨骼")] private SkeletonAnimation character;

    private void Start()
    {
        character.AnimationState.Complete += CompleteEvent;//注册动画回调事件函数
    }

    #region 动画播放
    public void PlayAnimationOnce(CharacterAnimation anim)
    {
        character.loop = false;
        character.AnimationState.SetAnimation(0, anim.ToString(), false);
    }
    public void PlayAnimationLoop(CharacterAnimation anim)
    {
        character.loop = true;
        character.AnimationState.SetAnimation(0, anim.ToString(), true);
    }
    #endregion
    
    /*定义动画回调事件函数*/
    private void CompleteEvent(Spine.TrackEntry trackEntry)
    {
        //点击动画播放完成后切换回待机动画
        character.AnimationState.SetAnimation(0, "Idle", true);
    }
}
