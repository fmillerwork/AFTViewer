using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Media.Imaging;
using TFAScriptTool.Models;

namespace AFTViewer.Helpers
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
                    if (File.Exists(dir + @"\_result.json"))
                    {
                        json = File.ReadAllText(dir + @"\_result.json");
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
        /// Enregistre les changements effectues dans le dossier de sauvegarde. 
        /// </summary>
        /// <param name="model"></param>
        public static void SaveChanges(RunResultModel model) // ASYNC ?
        {
            try
            {
                var runPath = string.Format(Globals.RUN_PATH, model.TimeStamp);

                var serializerOptions = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(model, serializerOptions);
                File.WriteAllText(runPath + @"\_result.json", json);
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
