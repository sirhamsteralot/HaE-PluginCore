using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HaEPluginCore
{
    public static class HaEConstants
    {
        public const float screenScaleConstant = 1.33333337f;
        public static string pluginFolder => Path.GetDirectoryName(typeof(HaEConstants).Assembly.Location);
        public static string StorageFolder = "Config";

        public static TimeSpan quarterSecTimeOut = TimeSpan.FromMilliseconds(250);
    }
}
