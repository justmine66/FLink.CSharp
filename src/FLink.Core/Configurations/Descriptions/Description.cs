using System.Collections.Generic;

namespace FLink.Core.Configurations.Descriptions
{
    /// <summary>
    /// Description for <see cref="ConfigOption{T}"/>. Allows providing multiple rich formats.
    /// </summary>
    public class Description
    {
        private readonly List<IBlockElement> _blocks;

        public Description(List<IBlockElement> blocks)
        {
            _blocks = blocks;
        }

        public static DescriptionBuilder Builder() => new DescriptionBuilder();
    }
}
