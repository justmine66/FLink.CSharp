using System;
using System.Collections.Generic;
using System.Linq;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.FS;
using FLink.Core.Util;
using FLink.Metrics.Core.Groups;
using FLink.Runtime.Execution;
using FLink.Runtime.State;
using FLink.Runtime.State.TTL;
using FLink.Runtime.Test.Operators.Utils;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace FLink.Runtime.FunctionalTest.State
{
    /// <summary>
    /// Tests for the <see cref="IKeyedStateBackend{TKey}"/> and <see cref="IOperatorStateBackend"/> as produced by various <see cref="IStateBackend"/>s.
    /// </summary>
    /// <typeparam name="TBackend"></typeparam>
    public abstract class StateBackendTestBase<TBackend> where TBackend : AbstractStateBackend
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private ICheckpointStreamFactory _checkpointStreamFactory;

        protected StateBackendTestBase(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        protected abstract TBackend StateBackend { get; }

        protected abstract bool IsSerializerPresenceRequiredOnRestore { get; }

        protected ICheckpointStreamFactory CreateStreamFactory() => _checkpointStreamFactory ?? StateBackend
                                                                        .CreateCheckpointStorage(new JobId())
                                                                        .ResolveCheckpointStorageLocation(1L,
                                                                            CheckpointStorageLocationReference.Default);

        protected AbstractKeyedStateBackend<TKey> CreateKeyedBackend<TKey>(TypeSerializer<TKey> keySerializer) =>
            CreateKeyedBackend(keySerializer, new DummyEnvironment());

        protected AbstractKeyedStateBackend<TKey> CreateKeyedBackend<TKey>(TypeSerializer<TKey> keySerializer, IEnvironment env) => CreateKeyedBackend(
            keySerializer,
            10,
            new KeyGroupRange(0, 9),
            env);

        protected AbstractKeyedStateBackend<TKey> CreateKeyedBackend<TKey>(
            TypeSerializer<TKey> keySerializer,
            int numberOfKeyGroups,
            KeyGroupRange keyGroupRange,
            IEnvironment env) => StateBackend.CreateKeyedStateBackend(
            env,
            new JobId(),
            "test_op",
            keySerializer,
            numberOfKeyGroups,
            keyGroupRange,
            env.TaskKvStateRegistry,
            TtlTimeProvider.Default,
            new UnRegisteredMetricsGroup(),
            new List<IKeyedStateHandle>(),
            new CloseableRegistry());

        protected AbstractKeyedStateBackend<TKey> RestoreKeyedBackend<TKey>(TypeSerializer<TKey> keySerializer,
            IKeyedStateHandle state) => RestoreKeyedBackend(keySerializer, state, new DummyEnvironment());

        protected AbstractKeyedStateBackend<TKey> RestoreKeyedBackend<TKey>(
            TypeSerializer<TKey> keySerializer,
            IKeyedStateHandle state,
            IEnvironment env) => RestoreKeyedBackend(
            keySerializer,
            10,
            new KeyGroupRange(0, 9),
            SingletonList<IKeyedStateHandle>.Instance,
            env);

        protected AbstractKeyedStateBackend<TKey> RestoreKeyedBackend<TKey>(
            TypeSerializer<TKey> keySerializer,
            int numberOfKeyGroups,
            KeyGroupRange keyGroupRange,
            IList<IKeyedStateHandle> state,
            IEnvironment env) => StateBackend.CreateKeyedStateBackend(
            env,
            new JobId(),
            "test_op",
            keySerializer,
            numberOfKeyGroups,
            keyGroupRange,
            env.TaskKvStateRegistry,
            TtlTimeProvider.Default,
            new UnRegisteredMetricsGroup(),
            state,
            new CloseableRegistry());

        [Fact]
        public void TestGetKeys()
        {
            var namespace1ElementsNum = 1000;
            var namespace2ElementsNum = 1000;
            var fieldName = "get-keys-test";

            var backend = CreateKeyedBackend(IntSerializer.Instance);
            try
            {
                var ns1 = "ns1";
                var keyedState1 = backend.GetPartitionedState(ns1, StringSerializer.Instance,
                    new ValueStateDescriptor<int>(fieldName, IntSerializer.Instance));

                for (var key = 0; key < namespace1ElementsNum; key++)
                {
                    backend.CurrentKey = key;
                    keyedState1.Value = key * 2;
                }

                var ns2 = "ns2";
                var keyedState2 = backend.GetPartitionedState(ns1, StringSerializer.Instance,
                    new ValueStateDescriptor<int>(fieldName, IntSerializer.Instance));

                for (var key = 0; key < namespace2ElementsNum; key++)
                {
                    backend.CurrentKey = key;
                    keyedState2.Value = key * 2;
                }

                // valid for namespace1
                try
                {
                    using var keys = backend.GetKeys(fieldName, ns1).OrderBy(it => it).GetEnumerator();
                    for (var expectedKey = 0; expectedKey < namespace1ElementsNum; expectedKey++)
                    {
                        Assert.True(keys.MoveNext());
                        Assert.Equal(expectedKey, keys.Current);
                    }

                    Assert.False(keys.MoveNext());
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                }

                // valid for namespace2
                try
                {
                    using var keys = backend.GetKeys(fieldName, ns2).OrderBy(it => it).GetEnumerator();
                    for (var expectedKey = 0; expectedKey < namespace1ElementsNum; expectedKey++)
                    {
                        Assert.True(keys.MoveNext());
                        Assert.Equal(expectedKey, keys.Current);
                    }

                    Assert.False(keys.MoveNext());
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                }
            }
            finally
            {
                IOUtils.CloseQuietly(backend);
                backend.Dispose();
            }
        }
    }
}
