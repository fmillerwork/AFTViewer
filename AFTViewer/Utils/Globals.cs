using System.Configuration;
using System.Collections.Specialized;
using System;

namespace AFTViewer.Utils
{
    public static class Globals
    {
        /// <summary>
        /// Chemin de base
        /// </summary>
        public static string BASE_PATH = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings.Get("basePath"));

        /// <summary>
        /// "Scripts\"
        /// </summary>
        public static string SCRIPTS_PATH =             BASE_PATH + @"Scripts\";

        /// <summary>
        /// "CapturesTFA\"
        /// </summary>
        public static string CAPTURE_PATH =             BASE_PATH + @"TFA\";

        /// <summary>
        /// "CapturesTFA\Specs\{0}\"
        /// </summary>
        public static string TEST_SPECS_PATH =          BASE_PATH + @"TFA\Specs\{0}\";

        /// <summary>
        /// "CapturesTFA\Runs\"
        /// Ne pas confondre avec RUN_PATH !
        /// </summary>
        public static string RUNS_PATH =                BASE_PATH + @"TFA\Runs\";

        /// <summary>
        /// "CapturesTFA\Runs\{0}\"
        /// Ne pas confondre avec RUNS_PATH !
        /// </summary>
        public static string RUN_PATH =                 BASE_PATH + @"TFA\Runs\{0}\";

        /// <summary>
        /// "CapturesTFA\Runs\{0}\{1}\"
        /// </summary>
        public static string TESTSUITE_PATH =           BASE_PATH + @"TFA\Runs\{0}\{1}\";

        /// <summary>
        /// "CapturesTFA\Runs\{0}\{1}\{2}\"
        /// </summary>
        public static string TEST_PATH =                BASE_PATH + @"TFA\Runs\{0}\{1}\{2}\";

        /// <summary>
        /// "CapturesTFA\Runs\{0}\{1}\{2}\_Asserts\\"
        /// </summary>
        public static string ASSERT_FAILURES_PATH =    BASE_PATH + @"TFA\Runs\{0}\{1}\{2}\_Asserts\";

        /// <summary>
        /// "CapturesTFA\Runs\{0}\{1}\{2}\_Failures\"
        /// </summary>
        public static string CAPTURE_FAIURES_PATH =     BASE_PATH + @"TFA\Runs\{0}\{1}\{2}\_Failures\";

        public const string DATE_FORMAT = "yyyy-MM-dd HH'h'mm'min'ss's'";
        public const string FAILURE_EXTENSION = ".fail.png";
        public const string SPEC_EXTENSION = ".spec.png";
        public const string CLASSIC_EXTENSION = ".png";
        public const string SIDE_EXTENSION = ".side";
    }
}
