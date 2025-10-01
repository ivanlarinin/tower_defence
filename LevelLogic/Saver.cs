using UnityEngine;
using System.IO;
using System;

namespace TowerDefence
{
    [Serializable]
    public class Saver<T>
    {
        public T data;

        public static bool TryLoad(string filename, out T result)
        {
            var path = FileHandler.GetFullPath(filename);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"[Saver] No file found at {path}");
                result = default;
                return false;
            }

            try
            {
                var json = File.ReadAllText(path);
                var wrapper = JsonUtility.FromJson<Saver<T>>(json);
                result = wrapper.data;
                Debug.Log($"[Saver] Successfully loaded from {path}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Saver] Failed to load {path} — {e}");
                result = default;
                return false;
            }
        }

        public static bool Save(string filename, T data)
        {
            var path = FileHandler.GetFullPath(filename);

            try
            {
                var wrapper = new Saver<T> { data = data };
                var json = JsonUtility.ToJson(wrapper, prettyPrint: true);

                // Ensure folder exists
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                File.WriteAllText(path, json);
                Debug.Log($"[Saver] Saved to {path}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Saver] Failed to save {path} — {e}");
                return false;
            }
        }
    }

    public static class FileHandler
    {
        public static string GetFullPath(string filename)
        {
            if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                filename += ".json";

            return Path.Combine(Application.persistentDataPath, filename);
        }

        public static void DeleteFile(string filename)
        {
            var path = GetFullPath(filename);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[FileHandler] Deleted file at {path}");
            }
            else
            {
                Debug.LogWarning($"[FileHandler] No file to delete at {path}");
            }
        }

        internal static bool FileExists(string filename)
        {
            return File.Exists(GetFullPath(filename));
        }
    }
}
