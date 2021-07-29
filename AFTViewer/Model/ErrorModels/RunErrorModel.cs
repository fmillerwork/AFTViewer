using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTViewer.Model
{
    public class RunErrorModel
    {
        /// <summary>
        /// Horodatage de la run.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Message d'erreur de la run.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
