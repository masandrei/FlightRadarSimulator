using System;
using System.Collections.Generic;

namespace flyingApp
{
    public class FileDataLoader
    {
        public static List<IBaseObject> GetElementByType(string className, List<IBaseObject> data)
        {
            return data.Where(temp => temp.ObjectType == className).ToList();
        }

        public static List<IBaseObject> DownloadData(string filePath)
        {
            List<IBaseObject> temp = new List<IBaseObject>();
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] info = line.Split(',');
                        temp.Add(Factory.getElement(info));
                    }
                }
            }
            else
            {
                Console.WriteLine($"File {filePath} does't exist");
            }
            return temp;
        }

        public static void UploadData(List<IBaseObject> info, string filePath)
        {
            using (
                FileStream fstr = !File.Exists(filePath)
                    ? File.Create(filePath)
                    : File.Open(filePath, FileMode.Truncate, FileAccess.Write)
            )
            using (StreamWriter astr = new StreamWriter(fstr))
            {
                astr.WriteLine("[");
                int lines = 1;
                foreach (var str in info)
                {
                    string json = str.ToJson();
                    if (lines < info.Count)
                    {
                        astr.WriteLine(json + ",");
                    }
                    else
                    {
                        astr.WriteLine(json);
                    }
                    lines++;
                }
                astr.WriteLine("]");
            }
            Console.WriteLine("Successfully added lines");
        }
    }
}
