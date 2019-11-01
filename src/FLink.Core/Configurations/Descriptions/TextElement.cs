using System.Collections.Generic;
using System.Linq;

namespace FLink.Core.Configurations.Descriptions
{
    public class TextElement : IBlockElement, IInlineElement
    {
        private readonly string _format;
        private readonly List<IInlineElement> _elements;

        public TextElement(string format, List<IInlineElement> elements)
        {
            _format = format;
            _elements = elements;
        }

        public void Format(Formatter formatter)
        {
            throw new System.NotImplementedException();
        }

        public static TextElement Text(string format, params IInlineElement[] elements)
        {
            return new TextElement(format, elements.ToList());
        }
    }
}
