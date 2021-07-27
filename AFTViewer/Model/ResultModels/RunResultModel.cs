using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFAScriptTool.Models
{
    public class RunResultModel
    {
        public List<TestSuiteResultModel> TestSuiteResults { get; set; }

        /// <summary>
        /// Nombre de captures prises lors d'une run.
        /// </summary>
        public int CaptureCount
        {
            get; set;
        }

        /// <summary>
        /// Nombre d'échecs (images de comparaison) générées lors d'une run.
        /// </summary>
        public int FailureCount
        {
            get; set;
        }

        /// <summary>
        /// Horodatage de la run.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Nombre d'échecs non vérifiés.
        /// </summary>
        public int UnVerifiedFailuresCount { get; set; }

    }
}
