using System.Collections.Generic;

namespace FLink.Core.Configurations.Descriptions
{
    public class DescriptionBuilder
    {
        private readonly List<IBlockElement> _blocks = new List<IBlockElement>();

        public DescriptionBuilder Text(string format, params IInlineElement[] elements)
        {
            _blocks.Add(TextElement.Text(format, elements));
            return this;
        }

        public Description Build() => new Description(_blocks);
    }
}
