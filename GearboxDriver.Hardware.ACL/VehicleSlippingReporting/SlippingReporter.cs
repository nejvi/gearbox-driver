﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GearboxDriver.Hardware.ACL.VehicleSlippingReporting
{
    public class SlippingReporter
    {
        private readonly IEventBus _eventBus;
        private readonly ISlippingProvider _angularProvider;
        private bool LastReportedSlipping { get; set; }
        private bool isEverReported { get; set; }

        public SlippingReporter(IEventBus eventBus, ISlippingProvider slippingProvider)
        {
            _eventBus = eventBus;
            _angularProvider = slippingProvider;
        }

        public void TryToReport()
        {
            var currentSlipping = _angularProvider.IsCurrentlySlipping();
            if (LastReportedSlipping == currentSlipping && isEverReported) // VehicleStartedSlipping, VehicleStoppedSlipping
                return;

            if (currentSlipping)
                _eventBus.SendEvent(new VehicleStartedSlipping());
            else
                _eventBus.SendEvent(new VehicleStoppedSlipping());

            LastReportedSlipping = currentSlipping;
            isEverReported = true;
        }
    }
}
