namespace SpineCharacters
{
    public class Cetsyr : SpineCharacter
    {
        public override float PlayAnimationAppear()
        {
            character.AnimationState.ClearTracks();
            character.AnimationState.SetAnimation(0, "Start", false);
            return character.skeleton.Data.FindAnimation("Start").Duration;
        }

        public override float PlayRandomAnimation()
        {
            return 0;
        }

        public override float PlayAnimationIdle() { return 0; }
    
        public override float PlayAnimationClick() { return 0; }
    
        public override float PlayAnimationDrag() { return 0; }
    
        public override float PlayAnimationDisAppear()
        {
            character.AnimationState.ClearTracks();
            character.AnimationState.SetAnimation(0, "Die", false);
            return character.skeleton.Data.FindAnimation("Die").Duration;
        }
    }
}
