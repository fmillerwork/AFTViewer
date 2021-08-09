using AFTViewer.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using AFTViewer.Model;
using static AFTViewer.ViewModel.FailureBaseViewModel;
using System.Windows;

namespace AFTViewer.ViewModel
{
    public class RunViewModel : Observable
    {
        private readonly RunResultModel model;

        private readonly List<FailureBaseViewModel> FailureList;

        public RunViewModel(RunResultModel runResultModel)
        {
            model = runResultModel;
            TestSuiteViewModels = new ObservableCollection<TestSuiteViewModel>();
            foreach (var testSuite in model.TestSuiteResults)
            {
                TestSuiteViewModels.Add(new TestSuiteViewModel(testSuite, this));
            }

            FailureList = GetFailureCapturesList();
            SelectedFailure = GetFirstFailure();
        }
        public RunViewModel()
        {
            
        }

        #region Properties
        public ObservableCollection<TestSuiteViewModel> TestSuiteViewModels { get; set; }
        public int UnVerifiedFailuresCount
        {
            get => model.UnVerifiedFailuresCount;
            set { model.UnVerifiedFailuresCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(ForegroundColor)); }
        }

        public int FailureCount
        {
            get => model.FailureCount;
            set { model.FailureCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(ForegroundColor)); }
        }

        public RunResultModel Model { get => model; }

        public string RunName
        {
            get => model.TimeStamp;
        }

        public Brush ForegroundColor
        {
            get
            {
                if (FailureCount == 0 && UnVerifiedFailuresCount == 0)
                    return Brushes.Green;
                else
                    return Brushes.Black;
            }
        }

        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get => mainViewModel;
            set { SetProperty(ref mainViewModel, value); }
        }

        private FailureBaseViewModel selectedFailure;
        public FailureBaseViewModel SelectedFailure
        {
            get => selectedFailure;
            set
            {
                if (selectedFailure != null)
                {
                    selectedFailure.BackgroundColor = Brushes.Transparent;
                }
                SetProperty(ref selectedFailure, value);
                UpdateExpand();

                if (selectedFailure != null)
                {
                    //if (selectedCapture != null) selectedCapture.Background = Brushes.DodgerBlue; // bleu de selection
                    //if (selectedCapture != null) selectedCapture.Background = new SolidColorBrush(Color.FromRgb(238, 111, 36)); // couleur n2f
                    //if (selectedCapture != null) selectedCapture.Background = Brushes.Orange;
                    selectedFailure.BackgroundColor = new SolidColorBrush(Color.FromRgb(154, 154, 154)); // gris
                }
            }
        }


        #endregion

        #region Methodes de navigation
        /// <summary>
        /// Passe à l'échec suivant en fonction de la capture selectionnée. Pour se basé sur une autre capture, selectedCaptureIndex être égal à l'indice de lla capture dans FailureCaptureList.
        /// </summary>
        /// <param name="getNextUnVerified"></param>
        public void SetNextSelectedCapture(bool getNextUnVerified)
        {
            if (SelectedFailure == null)
                return;
            if (FailureList.Count == 0)
                SelectedFailure = null;
            else
            {
                int selectedCaptureIndex = FailureList.IndexOf(SelectedFailure);

                if (getNextUnVerified)
                {
                    int indexCount = 0;
                    do
                    {
                        indexCount++;
                        // Si la boucle à fait le tour des captures.
                        if (indexCount > FailureList.Count)
                        {
                            SelectedFailure = FailureList.First();
                            return;
                        }

                        // Si dernier indice
                        if (selectedCaptureIndex == FailureList.Count - 1)
                            selectedCaptureIndex = 0;
                        else
                            selectedCaptureIndex++;
                    } while (FailureList.ElementAt(selectedCaptureIndex).State != FailureState.UnVerified);

                    SelectedFailure = FailureList.ElementAt(selectedCaptureIndex);
                }
                else
                {
                    if (selectedCaptureIndex == FailureList.Count - 1)
                        SelectedFailure = FailureList.First();
                    else
                        SelectedFailure = FailureList.ElementAt(selectedCaptureIndex + 1);
                }
            }
        }

        /// <summary>
        /// Passe à l'échec précédent.
        /// </summary>
        /// <param name="getPrevUnVerified"></param>
        public void SetPreviousSelectedCapture(bool getPrevUnVerified)
        {
            if (SelectedFailure == null)
                return;
            if (FailureList.Count == 0)
                SelectedFailure = null;
            else
            {
                var selectedCaptureIndex = FailureList.IndexOf(SelectedFailure);
                if (getPrevUnVerified)
                {
                    int indexCount = 0;
                    do
                    {
                        // Si la boucle à fait le tour des captures, on renvoie la première capture.
                        indexCount++;
                        if (indexCount > FailureList.Count)
                        {
                            SelectedFailure = FailureList.First();
                            return;
                        }

                        // Si premier  indice
                        if (selectedCaptureIndex == 0)
                            selectedCaptureIndex = FailureList.Count - 1;
                        else
                            selectedCaptureIndex--;
                    } while (FailureList.ElementAt(selectedCaptureIndex).State != FailureState.UnVerified);

                    SelectedFailure = FailureList.ElementAt(selectedCaptureIndex);
                }
                else
                {
                    if (selectedCaptureIndex == 0)
                        SelectedFailure = FailureList.Last();
                    else
                        SelectedFailure = FailureList.ElementAt(selectedCaptureIndex - 1);
                }
            }
        }

        /// <summary>
        /// Selectionne la première failure non verifiée. S'il n'en existe pas, le première capture vérifiée est sélectionnée.
        /// </summary>
        public void SetSelectedCaptureOnFirstUnverifiedCapture()
        {
            bool changed = false;
            foreach (var failureCapture in FailureList)
            {
                if (failureCapture.State == FailureState.UnVerified)
                {
                    SelectedFailure = failureCapture;
                    changed = true;
                    break;
                }
            }
            if (!changed)
                SelectedFailure = FailureList.First();
        }
        #endregion

        /// <summary>
        /// Supprime le FailureCaptureViewModel dans toute la run, supprime les tests, suite ou runs s'ils sont vides et actualise la SelectedCapture.
        /// </summary>
        /// <param name="failureCapture"></param>
        public void OverrideSpecCapture(FailureCaptureViewModel failureCapture)
        {
            // Echange de capture de spec
            if(SelectedFailure is FailureCaptureViewModel selectedCapture)
            {
                selectedCapture.SwitchSpecCapture();

                // Par suite de test.
                for (int testSuiteIndex = TestSuiteViewModels.Count - 1; testSuiteIndex >= 0; testSuiteIndex--)
                {
                    var currentTestSuite = TestSuiteViewModels[testSuiteIndex];
                    // Par test.
                    for (int testIndex = currentTestSuite.TestViewModels.Count - 1; testIndex >= 0; testIndex--)
                    {
                        var currentTest = currentTestSuite.TestViewModels[testIndex];
                        var deleted = false;

                        // Si capture trouvée, suppression capture dans le test.
                        if (failureCapture.TestName == currentTest.TestName)
                        {
                            currentTest.DeleteFailureCapture(failureCapture.Name);
                            if(SelectedFailure.State == FailureState.UnVerified)
                                UnVerifiedFailuresCount -= 1;
                            else if(SelectedFailure.State == FailureState.Recognized)
                                FailureCount -= 1;
                            deleted = true;
                        }

                        // Si plus aucune capture dans le test, supprime test de la suite.
                        if (currentTest.FailureViewModels.Count == 0)
                        {
                            currentTestSuite.DeleteTest(currentTest);
                            break; // Car tests en 1 exemplaire max par suite
                        }

                        // Si capture trouvée (donc supprimée) dans un test de la suite, on arrete de parcourir la suite.
                        // Car une occurence de test par suite et une occurence de capture par test.
                        if (deleted)
                            break;
                    }
                    // Si plus aucun test dans la suite, supprime suite de la run.
                    if (currentTestSuite.TestViewModels.Count == 0)
                    {
                        DeleteTestSuite(currentTestSuite);
                    }
                }
                // Suppression des captures dans la liste de navigation.
                RemoveFailureCapturesFromNavList(failureCapture.Name);

                // Actualisation des captures
                MainViewModel.RefreshSpecCaptureSources(selectedCapture.Name);

                // S'il reste des échecs dans la run, changement de l'échec sélectionné
                if (FailureList.Count > 0)
                {
                    SetSelectedCaptureOnFirstUnverifiedCapture();
                }

                else
                {
                    //MainViewModel.DeleteSelectedRun();
                    SelectedFailure = null;
                }
            }
        }

        /// <summary>
        /// Change l'état de la failure suivant le paramètre "newState".
        /// </summary>
        /// <param name="newState"></param>
        public void UpdateState(FailureState newState)
        {
            // État actuel 
            if (SelectedFailure.State == FailureState.UnVerified)
                UnVerifiedFailuresCount--;
            // Remise en Non vérifié
            else if (newState == FailureState.UnVerified)
                UnVerifiedFailuresCount++;

            if (newState == FailureState.Recognized)
                FailureCount++;
            else if((newState  == FailureState.FalsePositive || newState == FailureState.UnVerified) && SelectedFailure.State == FailureState.Recognized)
                FailureCount--;

            // Nouvel état
            SelectedFailure.State = newState;
        }

        #region Private Methods
        /// <summary>
        /// Supprime toutes les captures dans la liste de nagivation (FailureCaptureList) qui portent le nom passé en paramètre .
        /// </summary>
        /// <param name="failureName"></param>
        private void RemoveFailureCapturesFromNavList(string failureName)
        {
            for (int captureIndex = FailureList.Count - 1; captureIndex >= 0; captureIndex--)
            {
                var currentFailure = FailureList[captureIndex];
                if(currentFailure is FailureCaptureViewModel failureCaptureVM)
                {
                    if (failureCaptureVM.Name == failureName)
                        FailureList.Remove(currentFailure);
                }
                else if(currentFailure is FailedAssertViewModel failedAssertVM)
                {
                    if (failedAssertVM.Name == failureName)
                        FailureList.Remove(currentFailure);
                }
                
            }
        }

        /// <summary>
        /// Retourne la liste des captures d'échec.
        /// </summary>
        /// <returns></returns>
        private List<FailureBaseViewModel> GetFailureCapturesList()
        {
            var list = new List<FailureBaseViewModel>();
            foreach (var testSuite in TestSuiteViewModels)
            {
                foreach (var test in testSuite.TestViewModels)
                {
                    foreach (var capture in test.FailureViewModels)
                    {
                        list.Add(capture);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Retourne la première capture d'échec non vérifiée s'il en existe au moins une. Sinon, retourne la première capture vérifiée.
        /// </summary>
        /// <returns></returns>
        private FailureBaseViewModel GetFirstFailure()
        {
            foreach (var capture in FailureList)
            {
                if (capture.State == FailureState.UnVerified)
                    return capture;
            }
            if (FailureList.Count > 0)
                return FailureList.First();
            else
                return null;
        }

        /// <summary>
        /// Etant le test et la suite (dans le tree view) correspondant à la capture selectionnee.
        /// </summary>
        private void UpdateExpand()
        {
            foreach (var testSuite in TestSuiteViewModels)
            {
                foreach (var test in testSuite.TestViewModels)
                {
                    if (test.FailureViewModels.Contains(SelectedFailure))
                    {
                        test.IsExpanded = true;
                        testSuite.IsExpanded = true;
                    }
                }
            }
        }

        /// <summary>
        /// Supprime la suite de test passee en parametre.
        /// </summary>
        /// <param name="testSuiteVM"></param>
        private void DeleteTestSuite(TestSuiteViewModel testSuiteVM)
        {
            int index = -1;
            foreach (var testSuiteModel in model.TestSuiteResults)
            {
                index++;
                if (testSuiteModel.TestSuiteName == testSuiteVM.TestSuiteName)
                {
                    model.TestSuiteResults.Remove(testSuiteModel);
                    break;
                }
            }

            TestSuiteViewModels.RemoveAt(index);
        }
        #endregion
    }
}
