using System.Collections.Generic;
using FLink.Core.Api.Dag;
using FLink.Core.Util;

namespace FLink.Streaming.Api.Transformations
{
    public class SideOutputTransformation<T> : Transformation<T>
    {
        public Transformation<T> Input { get; }
        public OutputTag<T> Tag { get; }

        public SideOutputTransformation(Transformation<T> input, OutputTag<T> tag)
            : base("SideOutput", tag.TypeInfo, input.Parallelism)
        {
            Input = input;
            Tag = tag;
        }

        public override IList<Transformation<dynamic>> TransitivePredecessors
        {
            get
            {
                var result = new List<Transformation<dynamic>> { this as Transformation<dynamic> };
                result.AddRange(Input.TransitivePredecessors);
                return result;
            }
        }
    }
}
