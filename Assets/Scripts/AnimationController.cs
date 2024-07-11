using System.Collections.Generic;
using UnityEngine;

public class AnimationController : Singleton<AnimationController>
{
    [SerializeField, Tooltip("角色列表")] private List<SpineCharacter> characterList;
    [Tooltip("当前动画角色索引")] private int curCharacterIndex;
    [Header("随机动画播放")] 
    [SerializeField, Tooltip("时间间隔")] private float timeInterval;
    [SerializeField, Tooltip("是否随机播放动画")] private bool ifPlayRandomAnim = true;
    [SerializeField, Tooltip("计时器")] private float animCounter;

    private void Start()
    {
        //初始化
        foreach (var character in characterList)
        {
            character.Initialize();
        }
        characterList[curCharacterIndex].PlayAnimationAppear();
    }
    
    private void Update()
    {
        PlayRandomAnim();
    }
    
    #region 角色切换
    public void SwitchCharacterRight()
    {
        var time = characterList[curCharacterIndex].PlayAnimationDisAppear();
        curCharacterIndex = (curCharacterIndex + 1 >= characterList.Count) ? 0 : curCharacterIndex + 1;
        Invoke(nameof(SwitchCharacter),time);
    }
    public void SwitchCharacterLeft()
    {
        var time = characterList[curCharacterIndex].PlayAnimationDisAppear();
        curCharacterIndex = (curCharacterIndex - 1 < 0) ? characterList.Count - 1 : 0;
        Invoke(nameof(SwitchCharacter),time);
    }

    private void SwitchCharacter()
    {
        foreach (var character in characterList)
        {
            character.gameObject.SetActive(false);
        }
        characterList[curCharacterIndex].gameObject.SetActive(true);
        characterList[curCharacterIndex].PlayAnimationAppear();
        animCounter = 0f;//重置随机动画计时器
    }
    #endregion

    #region 随机动画播放
    public void SetIfPlayRandomAnim(bool flag)
    {
        ifPlayRandomAnim = flag;
    }
    private void PlayRandomAnim()
    {
        if (ifPlayRandomAnim)
        {
            animCounter += Time.deltaTime;
            if (animCounter > timeInterval)
            {
                characterList[curCharacterIndex].PlayRandomAnimation();
                animCounter = 0f;
            }
        }
    }
    #endregion
}
