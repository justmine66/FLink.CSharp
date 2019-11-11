using FLink.Core.Api.Common.Operators;

namespace FLink.Core.Api.Common.Resources
{
    public class GPUResource : Resource
    {
        public GPUResource(double value) : this(value, ResourceAggregateType.AggregateTypeSum) { }

        private GPUResource(double value, ResourceAggregateType type)
            : base(ResourceSpec.GpuName, value, type) { }

        public override Resource Create(double value, ResourceAggregateType type) => new GPUResource(value, type);
    }
}
