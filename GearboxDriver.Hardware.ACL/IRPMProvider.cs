﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GearboxDriver.Hardware.ACL
{
    public interface IRPMProvider
    {
        RPM GetCurrentRpm();
    }
}
