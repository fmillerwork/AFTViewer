

namespace AFTViewer.Model
{
    /// <summary>
    /// Représente une capture d'échec avec son chemin et son état (vérifiée, non vérifiée ou faux positif)
    /// </summary>
    public class FailureCaptureResultModel : FailureModel
    {
        public string FailurePath { get; set; }
        public string SpecCapturePath { get; set; }
        public string FailureCaptureName { get; set; }
        public string CapturePath { get; set; }

    }
}
