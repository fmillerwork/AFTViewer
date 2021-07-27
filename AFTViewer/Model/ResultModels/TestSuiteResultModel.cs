using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFAScriptTool.Models
{
    public class TestSuiteResultModel
    {
        /// <summary>
        /// Nombre de captures prises lors d'une suite de tests.
        /// </summary>
        public int CaptureCount
        {
            get;set;
            //get
            //{
            //    var cpt = 0;
            //    foreach (var test in TestResults)
            //    {
            //        cpt += test.CaptureCount;
            //    }
            //    return cpt;
            //}
        }

        /// <summary>
        /// Nombre d'échecs (images de comparaison) générées lors d'une suite de tests.
        /// </summary>
        public int FailureCount
        {
            get;set;
            //get
            //{
            //    var cpt = 0;
            //    foreach (var test in TestResults)
            //    {
            //        cpt += test.FailureCount;
            //    }
            //    return cpt;
            //}
        }

        /// <summary>
        /// Liste des résultats individuels des tests.
        /// </summary>
        public List<TestResultModel> TestResults { get; set; }
        public string TestSuiteName { get; set; }
    }
}
