using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    /// <summary>
    /// TypeInformation for "Java Beans"-style types. Flink refers to them as POJOs, since the conditions are slightly different from Java Beans.
    /// A type is considered a FLink POJO type, if it fulfills the conditions below:
    /// 1. It is a public class, and standalone (not a non-static inner class).
    /// 2. It has a public no-argument constructor.
    /// 3. All fields are either public, or have public getters and setters.
    /// </summary>
    /// <typeparam name="T">The type represented by this type information.</typeparam>
    public class PojoTypeInfo<T> : CompositeType<T>
    {
        public PojoTypeInfo(Type typeClass) 
            : base(typeClass) { }

        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity { get; }
        public override int TotalFields { get; }
        public override Type TypeClass { get; }
        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override void GetFlatFields(string fieldExpression, int offset, IList<CompositeTypeFlatFieldDescriptor<object>> result)
        {
            throw new NotImplementedException();
        }

        public override TypeInformation<TX> GetTypeAt<TX>(string fieldExpression)
        {
            throw new NotImplementedException();
        }

        public override TypeInformation<TX> GetTypeAt<TX>(int pos)
        {
            throw new NotImplementedException();
        }

        protected override ICompositeTypeComparatorBuilder<T> CreateTypeComparatorBuilder()
        {
            throw new NotImplementedException();
        }
    }
}
