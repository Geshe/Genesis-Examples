using System;

namespace Company.Device
{
    public enum SerialSessionXXEventType
    {
        DataReceived = 0x3fff2035,
        Break = 0x3fff2023,
        ClearToSend = 0x3fff2029,
        DataCarrierDetect = 0x3fff202c,
        DataSetReady = 0x3fff202a,
        RingIndicator = 0x3fff202e,
    }
}
