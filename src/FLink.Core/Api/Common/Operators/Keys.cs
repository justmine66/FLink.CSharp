using FLink.Core.Api.Common.TypeInfos;

namespace FLink.Core.Api.Common.Operators
{
    public abstract class Keys<T>
    {
        public class ExpressionKeys<T> : Keys<T>
        {
            public static readonly string SelectAllChar = "*";

            public ExpressionKeys(int[] keyPositions, TypeInformation<T> type)
                : this(keyPositions, type, false)
            {
            }

            public ExpressionKeys(int[] keyPositions, TypeInformation<T> type, bool allowEmpty)
            {

            }

            public ExpressionKeys(string[] keyExpressions, TypeInformation<T> type)
            {

            }
        }
    }
}
