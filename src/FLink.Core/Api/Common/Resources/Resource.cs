using FLink.Core.Util;

namespace FLink.Core.Api.Common.Resources
{
    /// <summary>
    /// Base class for resources one can specify.
    /// </summary>
    public abstract class Resource
    {
        public enum ResourceAggregateType
        {
            AggregateTypeSum, 
            AggregateTypeMax
        }

        public string Name;

        public double Value;

        public ResourceAggregateType AggregateType;

        protected Resource(string name, double value, ResourceAggregateType type)
        {
            Name = Preconditions.CheckNotNull(name);
            Value = value;
            AggregateType = Preconditions.CheckNotNull(type);
        }

        /// <summary>
        /// Create a resource of the same resource resourceAggregateType.
        /// </summary>
        /// <param name="value">The value of the resource</param>
        /// <param name="type">The aggregate resourceAggregateType of the resource</param>
        /// <returns>A new instance of the sub resource</returns>
        public abstract Resource Create(double value, ResourceAggregateType type);
    }
}
