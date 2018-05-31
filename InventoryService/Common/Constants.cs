using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Common
{
    public class Constants
    {
        public static string PROJECT_NAME = "Client Service";

    
        //Zone mapping Table key : zone number, value : location number
        public static int ZONE_CODE_LEN_LIMIT = 3;
        public static int ZONE_CODE_1 = 1;
        public static int ZONE_CODE_2 = 2;
        public static int ZONE_CODE_3 = 3;
        public static int ZONE_CODE_4 = 4;
        public static int ZONE_CODE_5 = 5;
        public static int ZONE_CODE_6 = 6;
        public static int ZONE_CODE_7 = 7;

        //Zone 1 range
        public static int ZONE_CODE_1_MIN = 0;
        public static int ZONE_CODE_1_MAX = 69;
        //Zone 2 range
        public static int ZONE_CODE_2_MIN = 701;
        public static int ZONE_CODE_2_MAX = 992;

        //Zone 3 range
        public static int ZONE_CODE_3_A = 881;
        public static int ZONE_CODE_3_B = 891;
        public static int ZONE_CODE_3_C = 901;
        public static int ZONE_CODE_3_D = 911;

        //Return hide range
        public static int ZONE_CODE_RETURN = 333;
        //Zone 4 range
        public static int ZONE_CODE_4_ONE = 888;

        //Rework range
        public static int ZONE_CODE_5_ONE = 555;

        //QC range
        public static int ZONE_CODE_6_ONE = 666;

        //Scrapped range
        public static int ZONE_CODE_7_ONE = 777;

        //Shipping
        public static int ZONE_CODE_SHIPPING = 999;
    }
}