﻿using GearboxDriver.Gearshift;
using GearboxDriver.Hardware.ACL;
using GearboxDriver.Seedwork;

namespace GearboxDriver.Processes
{
    public class SmoothBrakingWithTrailerAttached : IProcessManager
    {
        private bool CarMovingDownhill { get; set; }
        private int CurrentGear { get; set; }
        private GearshiftService _service;

        public SmoothBrakingWithTrailerAttached(/*GearshiftService service*/)
        {
            //_service = service;
            CarMovingDownhill = false;
        }

        public void ApplyEvent(IEvent @event)
        {
            switch (@event)
            {
                //case GearUpshifted // todo
                case VehicleTiltedDownhill _:
                    if (!CarMovingDownhill)
                    {
                        CarMovingDownhill = true;
                        //_service.TargetGear(CurrentGear - 1); // todo bardziej rozbudowane
                    }
                    
                    break;
                case VehicleTiledToStraightPosition _:
                case VehicleTiltedUphill _:
                    CarMovingDownhill = false;
                    _service.StopTargetingGear();
                    break;
            }
        }
    }
}
