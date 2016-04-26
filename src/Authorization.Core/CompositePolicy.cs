namespace Authorization
{
    public abstract class CompositePolicy : Policy
    {
        public CompositePolicy(PoliciesService policiesService)
        {
            PoliciesService = policiesService;
        }

        protected virtual PoliciesService PoliciesService { get; private set; }
    }
}
