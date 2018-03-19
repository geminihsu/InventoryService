using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Common
{
    public class LocationHelper
    {
        public static int MapZoneCode(String location)
        {
            int code = -1;
            int locNum = Int32.Parse(location);

            if (locNum == Constants.ZONE_CODE_4_ONE)
                code = Constants.ZONE_CODE_4;
            else if (locNum == Constants.ZONE_CODE_5_ONE)
                code = Constants.ZONE_CODE_5;
            else if (locNum == Constants.ZONE_CODE_6_ONE)
                code = Constants.ZONE_CODE_6;
            else if (locNum == Constants.ZONE_CODE_7_ONE)
                code = Constants.ZONE_CODE_7;
            else if (locNum >= Constants.ZONE_CODE_1_MIN && locNum <= Constants.ZONE_CODE_1_MAX)
                code = Constants.ZONE_CODE_1;
            else if (locNum >= Constants.ZONE_CODE_2_MIN && locNum <= Constants.ZONE_CODE_2_MAX
                    && locNum != Constants.ZONE_CODE_3_A && locNum != Constants.ZONE_CODE_3_B
                    && locNum != Constants.ZONE_CODE_3_C && locNum != Constants.ZONE_CODE_3_D
                    && locNum != Constants.ZONE_CODE_4)
            {
                if (locNum % 10 == 1 || locNum % 10 == 2)
                    code = Constants.ZONE_CODE_2;
            }
            else if (locNum == Constants.ZONE_CODE_3_A || locNum == Constants.ZONE_CODE_3_B
                  || locNum == Constants.ZONE_CODE_3_C || locNum == Constants.ZONE_CODE_3_D)
            {
                code = Constants.ZONE_CODE_3;
            }

            return code;
        }
    }
}