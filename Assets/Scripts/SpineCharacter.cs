using System;
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
    [SerializeField, Tooltip("移动时间偏差值")] private float moveOffset = 2.6f;
    [SerializeField, Tooltip("移动速度")] private int moveSpeed = 1;
    private Vector2Int walkDir = new Vector2Int(1, 0);
    
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

    public float PlayAnimationIdle()
    {
        return PlayAnimation("Idle", false);
    }
    
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
    
    /// <summary>
    /// 播放动画序列协程
    /// </summary>
    /// <param name="animationNames"></param>
    /// <returns></returns>
    private IEnumerator PlayAnimationsInSequence(List<string> animationNames)
    {
        foreach (var animName in animationNames)
        {
            //走动动画的特殊效果，这边的协程可以替换
            if (animName == "Move")
            {
                StopCoroutine(MoveAnim_02());
                Debug.Log("开始");
                StartCoroutine(MoveAnim_02());
            }
            else
            {
                float duration = PlayAnimation(animName, false);
                yield return new WaitForSeconds(duration);// 等待动画播放完毕
            }
        }
    }
    
    /// <summary>
    /// 走路协程1:来回折返走
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveAnim_01()
    {
        float duration = PlayAnimation("Move", false) - moveOffset;
        Vector2Int direction = new Vector2Int(1,0)* moveSpeed;
        for (int i = 0; i < 2; i++)
        {
            float counter = 0f;
            this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            direction *= -1;
            while (counter < duration/2)
            {
                BackGroundSet.Instance.ChangeWindowPosition(direction);
                counter += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    
    /// <summary>
    /// 走路协程2:一直走路，直到碰到屏幕边缘自动转向
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveAnim_02()
    {
        float duration = PlayAnimation("Move", false) - moveOffset;
        Vector2Int direction = walkDir* moveSpeed;
        
        //取一个随机数，有50%的概率往反方向走
        if (Random.Range(0.0f, 1.0f) >= 0.5f) { TurnAround(); }
        
        //执行走路
        float counter = 0f;
        while (counter < duration)
        {
            BackGroundSet.Instance.ChangeWindowPosition(direction);
            if (BackGroundSet.Instance.CheckIfEdge())//如果碰壁就转向
            {
                TurnAround();
            }
            counter += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        //内嵌转向函数
        void TurnAround()
        {
            this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            direction *= -1;
            walkDir *= -1;
        }
    }
    #endregion
    
    /// <summary>
    /// 动画回调事件函数
    /// </summary>
    /// <param name="trackEntry"></param>
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

