﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GearboxDriver.Hardware.API;
using GearboxDriver.PublishedLanguage.Gearbox;
using GearboxDriver.PublishedLanguage.Pedals;
using GearboxDriver.Seedwork;

namespace GearboxDriver.SampleApplication
{
    // Class only for application demonstration and is out of scope of the model, contains very naive implementations
    class EngineEmulator : IEventListener
    {
        private readonly ExternalSystems _systems;
        private PedalPressure GasPressure { get; set; }
        private PedalPressure BrakePressure { get; set; }
        private Gear CurrentGear { get; set; }

        public EngineEmulator(ExternalSystems systems)
        {
            _systems = systems;
            GasPressure = new PedalPressure(0);
            BrakePressure = new PedalPressure(0);
            CurrentGear = new Gear(0);

            Task.Run(Run);
        }

        // Supposed to be very simple and naive
        public void HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case GasPressureChanged gasPressureChanged:
                    GasPressure = gasPressureChanged.PedalPressure;
                    return;
                case BrakePressureChanged brakePressureChanged:
                    BrakePressure = brakePressureChanged.PedalPressure;
                    return;
                case GearChanged gearChanged:
                    CurrentGear = gearChanged.EnteredGear;

                    if (gearChanged.EnteredGear.Value > gearChanged.PreviousGear.Value)
                        _systems.setCurrentRpm(Math.Max(_systems.getCurrentRpm() - 1500, 0));

                    if (gearChanged.EnteredGear.Value < gearChanged.PreviousGear.Value)
                        _systems.setCurrentRpm(Math.Max(_systems.getCurrentRpm() + 1500, 0));

                    break;
            }
        }

        private void Run()
        {
            while (true)
            {
                _systems.setCurrentRpm(Math.Min(_systems.getCurrentRpm() + GetRpmIncreaseForGasPressure(GasPressure), 15000));
                _systems.setCurrentRpm(Math.Max(_systems.getCurrentRpm() - BrakePressure.Value * 10, 0));
                Task.Delay(TimeSpan.FromMilliseconds(30)).Wait();
            }
        }


        private double GetRpmIncreaseForGasPressure(PedalPressure pressure)
        {
            switch (CurrentGear.Value)
            {
                case 0:
                    return 10 + pressure.Value * 35;
                case 1:
                    return pressure.Value * 30 - 2;
                case 2: 
                    return pressure.Value * 25 - 2;
                case 3: 
                    return pressure.Value * 20 - 3;
                case 4: 
                    return pressure.Value * 15 - 5;
                case 5:
                    return pressure.Value * 10 - 7;
                case 6:
                    return pressure.Value * 5 - 9;
            }

            return 0;
        }
    }
}
