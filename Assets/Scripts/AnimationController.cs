using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AnimationController : Singleton<AnimationController>
{
    [SerializeField, Tooltip("角色列表")] private List<SpineCharacter> characterList;
    [Tooltip("当前动画角色索引")] private int curCharacterIndex;

    private void Start()
    {
        //初始化
        foreach (var character in characterList)
        {
            character.Initialize();
        }
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
    }
    #endregion

    #region 随机动画播放
    private void PlayRandomAnim()
    {
        
    }
    #endregion
}
