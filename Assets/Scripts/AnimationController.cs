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

    #region 动画播放
    

    #endregion

    #region 角色切换
    public void SwitchCharacterRight()
    {
        curCharacterIndex = (curCharacterIndex + 1 >= characterList.Count) ? 0 : curCharacterIndex + 1;
        SwitchCharacter();
    }
    public void SwitchCharacterLeft()
    {
        curCharacterIndex = (curCharacterIndex - 1 < 0) ? characterList.Count - 1 : 0;
        SwitchCharacter();
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
}
