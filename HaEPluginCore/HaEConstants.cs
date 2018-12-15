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
        public static int versionNumber = 1060;  //1.0.6.0

        public const float screenScaleConstant = 1.33333337f;
        public static string pluginFolder => Path.GetDirectoryName(typeof(HaEConstants).Assembly.Location);
        public const string StorageFolder = "Config";
        public const string ScriptFolder = "Scripts";
        public const string AssetFolder = "Assets";

        public static TimeSpan quarterSecTimeOut = TimeSpan.FromMilliseconds(250);
    }
}
