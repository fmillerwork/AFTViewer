using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTViewer.Model
{
    public class TestResultModel
    {
        /// <summary>
        /// Nombre de captures prises lors d'un test.
        /// </summary>
        public int CaptureCount { get; set; }

        /// <summary>
        /// Nombre d'échecs (images de comparaison) générées lors d'un test.
        /// </summary>
        public int FailureCount { get; set; }
        public string TestName { get; set; }

        /// <summary>
        /// Liste des FailureCaptures.
        /// </summary>
        public List<FailureCaptureResultModel> FailureCaptures { get; set; }

        /// <summary>
        /// Liste des Assertions en échec.
        /// </summary>
        public List<FailedAssertResult> FailureAsserts { get; set; }
    }
}
