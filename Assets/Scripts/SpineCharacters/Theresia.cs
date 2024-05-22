namespace SpineCharacters
{
    public class Theresia : SpineCharacter
    {
        public override void PlayAnimationAppear()
        {
            character.AnimationState.ClearTracks();
            character.AnimationState.SetAnimation(0, "Skill", false);
        }

        public override void PlayAnimationIdle() { }
    
        public override void PlayAnimationClick() { }
    
        public override void PlayAnimationDrag() { }
    
        public override void PlayAnimationDisAppear() { }
    }
}
