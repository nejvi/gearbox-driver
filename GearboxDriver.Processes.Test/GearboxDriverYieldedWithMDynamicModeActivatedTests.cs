﻿using GearboxDriver.Gearshift;
using GearboxDriver.Hardware.ACL;
using GearboxDriver.Seedwork;
using Moq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GearboxDriver.Gearshift.Shifting;
using GearboxDriver.PublishedLanguage.VehicleMotion;

namespace GearboxDriver.Processes.Test
{
    public class GearboxDriverYieldedWithMDynamicModeActivatedTests
    {
        [Test]
        public void WhenVehicleIsSlippingAbstainFromChangingGears()
        {
            var serviceMock = new Mock<IGearshiftService>();

            var processManager = new MDynamicSlippingDetectionProcess(serviceMock.Object);

            processManager.ApplyEvent(new VehicleStartedSlipping());

            serviceMock.Verify(x => x.AbstainFromChangingGears(), Times.Once);
        }

        [Test]
        public void WhenVehicleStoppedSlippingStopAbstainingFromChangingGears()
        {
            var serviceMock = new Mock<IGearshiftService>();

            var processManager = new MDynamicSlippingDetectionProcess(serviceMock.Object);

            processManager.ApplyEvent(new VehicleStoppedSlipping()); // todo I guess it has to start slipping first ??

            serviceMock.Verify(x => x.StopAbstainingFromChangingGears(), Times.Once);
        }
    }
}
