namespace FLink.Core.Api.Common.Resources
{
    /// <summary>
    /// Represents CPU resource.
    /// </summary>
    public class CPUResource : Resource
    {
        public const string DefaultName = "CPU";

        public CPUResource(decimal value)
            : base(DefaultName, value)
        {
        }

        public override Resource Create(decimal value) => new CPUResource(value);
    }
}
