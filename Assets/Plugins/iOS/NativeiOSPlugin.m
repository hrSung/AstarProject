//
//  NativeiOSPlugin.mm
//  Unity-iPhone
//
//  Created by wwforeverMAC on 13. 8. 27..
//
//
/*
 #import "NativeiOSPlugin.h"
 
 @implementation NativeiOSPlugin
 
 @end
 */
//extern "C"
//{
    void iOSPluginStandard(const char* _str)
    {
        UnitySendMessage("iOSManager", "BackiOSFunc", _str);
    }
//}