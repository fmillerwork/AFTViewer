using AFTViewer.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TFAScriptTool.Models;
using static AFTViewer.ViewModel.FailureCaptureViewModel;

namespace AFTViewer.ViewModel
{
    public class RunViewModel : Observable
    {
        private readonly RunResultModel model;
        private readonly List<FailureCaptureViewModel> FailureCaptureList;
        public ObservableCollection<TestSuiteViewModel> TestSuiteViewModels { get; set; }
        public RunViewModel(RunResultModel runResultModel)
        {
            model = runResultModel;
            TestSuiteViewModels = new ObservableCollection<TestSuiteViewModel>();
            foreach (var testSuite in model.TestSuiteResults)
            {
                TestSuiteViewModels.Add(new TestSuiteViewModel(testSuite, this));
            }

            FailureCaptureList = GetFailureCapturesList();
            SelectedCapture = GetFirstFailureCapture();
        }

        public int UnVerifiedFailuresCount
        {
            get => model.UnVerifiedFailuresCount;
            set { model.UnVerifiedFailuresCount = value; OnPropertyChanged(); }
        }

        public int FailureCount
        {
            get => model.FailureCount;
            set { model.FailureCount = value; OnPropertyChanged(); }
        }

        public RunResultModel Model { get => model; }

        public string RunName
        {
            get => model.TimeStamp;
        }

        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get => mainViewModel;
            set{ SetProperty(ref mainViewModel, value); }
        }

        private FailureCaptureViewModel selectedCapture;
        public FailureCaptureViewModel SelectedCapture
        {
            get => selectedCapture;
            set
            {
                if (selectedCapture != null)
                {
                    selectedCapture.BackgroundColor = Brushes.Transparent;
                    //selectedCapture.IsSelected = false;
                }
                SetProperty(ref selectedCapture, value);
                UpdateExpand();
                //if (selectedCapture != null) selectedCapture.Background = Brushes.DodgerBlue; // bleu de selection
                //if (selectedCapture != null) selectedCapture.Background = new SolidColorBrush(Color.FromRgb(238, 111, 36)); // couleur n2f
                if (selectedCapture != null) 
                {
                    selectedCapture.BackgroundColor = new SolidColorBrush(Color.FromRgb(154, 154, 154)); // gris
                    //selectedCapture.IsSelected = true;
                }
                //if (selectedCapture != null) selectedCapture.Background = Brushes.Orange;
            }
        }

        public int SelectedCaptureIndex { get; set; }

        #region Methodes de navigation
        /// <summary>
        /// Passe à l'échec suivant.
        /// </summary>
        /// <param name="getNextUnVerified"></param>
        /// <param name="selectedCaptureIndex"></param>
        public void SetNextSelectedCapture(bool getNextUnVerified, int selectedCaptureIndex = -1)
        {
            if (SelectedCapture == null)
                return;

            if (FailureCaptureList.Count != 0)
            {
                if (selectedCaptureIndex == -1)
                    selectedCaptureIndex = FailureCaptureList.IndexOf(SelectedCapture);

                if (getNextUnVerified)
                {
                    int indexCount = 0;
                    do
                    {
                        indexCount++;
                        // Si la boucle à fait le tour des captures.
                        if (indexCount > FailureCaptureList.Count)
                        {
                            SelectedCapture = FailureCaptureList.First();
                            return; // VERIF
                        }

                        // Si dernier indice
                        if (selectedCaptureIndex == FailureCaptureList.Count - 1)
                            selectedCaptureIndex = 0;
                        else
                            selectedCaptureIndex++;
                    } while (FailureCaptureList.ElementAt(selectedCaptureIndex).State != FailureState.UnVerified);

                    SelectedCapture = FailureCaptureList.ElementAt(selectedCaptureIndex);
                }
                else
                {
                    if (selectedCaptureIndex == FailureCaptureList.Count - 1)
                        SelectedCapture = FailureCaptureList.First();
                    else
                        SelectedCapture = FailureCaptureList.ElementAt(selectedCaptureIndex + 1);
                }
            }
            else
                SelectedCapture = null;
        }

        /// <summary>
        /// Passe à l'échec précédent.
        /// </summary>
        /// <param name="getNextUnVerified"></param>
        public void SetPreviousSelectedCapture(bool getPrevUnVerified)
        {
            if (SelectedCapture == null)
                return;

            if (FailureCaptureList.Count != 0)
            {
                var selectedCaptureIndex = FailureCaptureList.IndexOf(SelectedCapture);
                if (getPrevUnVerified)
                {
                    int indexCount = 0;
                    do
                    {
                        // Si la boucle à fait le tour des captures, on renvoie la première capture.
                        indexCount++;
                        if (indexCount > FailureCaptureList.Count)
                        {
                            SelectedCapture = FailureCaptureList.First();
                            return;
                        }
                        if (selectedCaptureIndex == 0)
                            selectedCaptureIndex = FailureCaptureList.Count - 1;
                        else
                            selectedCaptureIndex--;
                    } while (FailureCaptureList.ElementAt(selectedCaptureIndex).State != FailureState.UnVerified);

                    SelectedCapture = FailureCaptureList.ElementAt(selectedCaptureIndex);
                }
                else
                {
                    if (selectedCaptureIndex == 0)
                        SelectedCapture = FailureCaptureList.Last();
                    else
                        SelectedCapture = FailureCaptureList.ElementAt(selectedCaptureIndex - 1);
                }
            }
            else
                SelectedCapture = null;
        }

        #endregion

        /// <summary>
        /// Supprime le FailureCaptureViewModel dans toute la run et actualise la SelectedCapture.
        /// </summary>
        /// <param name="failureCapture"></param>
        public void DeleteFailureCapture(FailureCaptureViewModel failureCapture)
        {
            SelectedCaptureIndex = FailureCaptureList.IndexOf(SelectedCapture);
            var isFailureCaptureTestSuiteExamined = false;
            // Par suite de test.
            for (int testSuiteIndex = TestSuiteViewModels.Count - 1; testSuiteIndex >= 0; testSuiteIndex--)
            {
                var currentTestSuite = TestSuiteViewModels[testSuiteIndex];
                // Par test.
                for (int testIndex = currentTestSuite.TestViewModels.Count - 1; testIndex >= 0; testIndex--)
                {
                    var currentTest = currentTestSuite.TestViewModels[testIndex];
                    var deleted = false;
                    
                    // Si capture trouvée, suppression capture du test.
                    if (failureCapture.TestName == currentTest.TestName)
                    {
                        // Vérification si la suite de failureCapture est déjà examinée
                        if (isFailureCaptureTestSuiteExamined)
                            SelectedCaptureIndex--;
                        if (failureCapture.TestSuiteName == currentTestSuite.TestSuiteName)
                            isFailureCaptureTestSuiteExamined = true;

                        UnVerifiedFailuresCount -= currentTest.DeleteFailureCapture(failureCapture.CaptureName);
                        deleted = true;
                    }

                    // Si plus aucune capture dans le test, supprime test de la suite.
                    if (currentTest.FailureCaptureViewModels.Count == 0)
                    {
                        currentTestSuite.DeleteTest(currentTest);
                        break; // Car tests en 1 exemplaire max par suite
                    }

                    // Si capture trouvée (et supprimée) dans un test de la suite, on arrete de parcourir la suite.
                    // Car une occurence de test par suite. Et les captures sont propres à chaque test.
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
            for (int captureIndex = FailureCaptureList.Count - 1; captureIndex >= 0; captureIndex--)
            {
                var currentCapture = FailureCaptureList[captureIndex];
                if (currentCapture.CaptureName == failureCapture.CaptureName)
                    FailureCaptureList.Remove(currentCapture);
            }

            //Actualisation SelectedCapture
            while( SelectedCaptureIndex >= FailureCaptureList.Count)
            {
                SelectedCaptureIndex--;
            }
            // Si plus de captures dans la run.
            if (SelectedCaptureIndex < 0)
            {
                // Modifier si besoin
                SelectedCapture = null;
                MainViewModel.SetPrevRun();
                MainViewModel.DeleteRun(); // tester
            }
        }

        /// <summary>
        /// Change l'état de la failure suivant le paramètre "newState".
        /// </summary>
        /// <param name="newState"></param>
        public void UpdateState(FailureState newState)
        {
            // État actuel 
            if (SelectedCapture.State == FailureState.UnVerified)
                UnVerifiedFailuresCount--;
            // Remise en Non vérifié
            else if (newState == FailureState.UnVerified)
                UnVerifiedFailuresCount++;

            SelectedCapture.State = newState;

            // *** PAS UTILE POUR LE MOMENT
            // Nouvel état
            if (SelectedCapture.State == FailureState.FalsePositive)
                FailureCount--;
            else if( SelectedCapture.State == FailureState.Recognized)
                FailureCount++;
            // ***
        }

        #region Private Methods
        /// <summary>
        /// Retourne la liste des captures d'échec.
        /// </summary>
        /// <returns></returns>
        private List<FailureCaptureViewModel> GetFailureCapturesList()
        {
            var list = new List<FailureCaptureViewModel>();
            foreach (var testSuite in TestSuiteViewModels)
            {
                foreach (var test in testSuite.TestViewModels)
                {
                    foreach (var capture in test.FailureCaptureViewModels)
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
        private FailureCaptureViewModel GetFirstFailureCapture()
        {
            foreach (var capture in FailureCaptureList)
            {
                if (capture.State == FailureState.UnVerified)
                    return capture;
            }
            return FailureCaptureList.First();
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
                    if (test.FailureCaptureViewModels.Contains(SelectedCapture))
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
