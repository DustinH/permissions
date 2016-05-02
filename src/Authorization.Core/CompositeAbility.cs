namespace Authorization
{
    public abstract class CompositeAbility : Ability
    {
        public CompositeAbility(AbilitiesService abilitiesService)
        {
            AbilitiesService = abilitiesService;
        }

        protected virtual AbilitiesService AbilitiesService { get; private set; }
    }
}
