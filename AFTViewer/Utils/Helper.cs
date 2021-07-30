using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Media.Imaging;
using AFTViewer.Model;

namespace AFTViewer.Utils
{
    public class Helper
    {
        /// <summary>
        /// Charge les runs depuis le dossier de sauvegarde.
        /// </summary>
        /// <returns></returns>
        public static List<RunResultModel> LoadRunResults()
        {
            try
            {
                var runResultModels = new List<RunResultModel>();
                var runDirs = Directory.GetDirectories(Globals.RUNS_PATH);
                string json;
                foreach (var dir in runDirs)
                {
                    var filePath = dir + @"\_result.json";
                    if (File.Exists(filePath))
                    {
                        json = File.ReadAllText(filePath);
                        runResultModels.Add((RunResultModel)JsonSerializer.Deserialize(json, typeof(RunResultModel)));
                    } 
                }
                return runResultModels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Charge les runs depuis le dossier de sauvegarde.
        /// </summary>
        /// <returns></returns>
        public static List<RunErrorModel> LoadRunErrors()
        {
            try
            {
                var runErrorModels = new List<RunErrorModel>();
                var runDirs = Directory.GetDirectories(Globals.RUNS_PATH);
                string json;
                foreach (var dir in runDirs)
                {
                    var errorFilePath = dir + @"\_error.json";
                    var resultFilePath = dir + @"\_result.json";
                    if (File.Exists(errorFilePath))
                    {
                        json = File.ReadAllText(errorFilePath);
                        runErrorModels.Add((RunErrorModel)JsonSerializer.Deserialize(json, typeof(RunErrorModel)));
                    }
                    else if (!File.Exists(resultFilePath))
                    {
                        runErrorModels.Add(new RunErrorModel()
                        {
                            ErrorMessage = "Fichier _error.json ou _result.json manquant !",
                            TimeStamp = dir.Split(@"\")[^1]
                        });
                    }
                }
                return runErrorModels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Enregistre les changements effectues dans le dossier de sauvegarde. 
        /// </summary>
        /// <param name="model"></param>
        public static void SaveChanges(RunResultModel model) // ASYNC ?
        {
            try
            {
                var filePath = string.Format(Globals.RUN_PATH, model.TimeStamp) + @"\_result.json";
                if (File.Exists(filePath))
                {
                    var serializerOptions = new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    };
                    var json = JsonSerializer.Serialize(model, serializerOptions);
                    File.WriteAllText(filePath, json);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Charge une image.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage LoadImage(string path)
        {
            try
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = new Uri(path);
                bmi.EndInit();
                return bmi;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
