using System;
using FLink.Metrics.Core;

namespace FLink.Streaming.Runtime.Metrics
{
    public class MinWatermarkGauge : IGauge<long>
    {
        private readonly WatermarkGauge _watermarkGauge1;
        private readonly WatermarkGauge _watermarkGauge2;

        public MinWatermarkGauge(WatermarkGauge watermarkGauge1, WatermarkGauge watermarkGauge2)
        {
            _watermarkGauge1 = watermarkGauge1;
            _watermarkGauge2 = watermarkGauge2;
        }

        public long Value => Math.Min(_watermarkGauge1.Value, _watermarkGauge2.Value);
    }
}
