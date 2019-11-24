namespace FLink.Core.Api.Common.Resources
{
    public class GPUResource : Resource
    {
        public const string DefaultName = "GPU";

        public GPUResource(decimal value) 
            : base(DefaultName, value)
        {
        }

        public override Resource Create(decimal value) => new GPUResource(value);
    }
}
