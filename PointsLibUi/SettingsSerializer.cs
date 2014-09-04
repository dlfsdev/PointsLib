using System;
using System.IO;
using System.Xml.Serialization;

namespace PointsLibUi
{
    class SettingsSerializer
    {
        private readonly String _filename;

        public SettingsSerializer(String filename)
        {
            _filename = filename;
        }

        public ApplicationSettings SerializeIn()
        {
            try
            {
                using (var f = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var x = CreateXmlSerializer(typeof(ApplicationSettings));
                    return x.Deserialize(f) as ApplicationSettings;
                }
            }
            // between the FileStream and the XmlSerializer, there are about a dozen things that could go wrong
            // if any of these happen, just let the caller know the settings couldn't be loaded
            catch (Exception)
            {
                return null;
            }
        }

        public bool SerializeOut(ApplicationSettings settings)
        {
            String tempFilename = Path.GetTempFileName();

            bool success = false;
            bool tempCreated = false;

            try
            {
                using (var f = new FileStream(tempFilename, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    tempCreated = true;
                    var x = CreateXmlSerializer(typeof(ApplicationSettings));
                    x.Serialize(f, settings);
                }

                success = true;
            }
            // between the FileStream and the XmlSerializer, there are about a dozen things that could go wrong
            // if any of these happen, just let the caller know the settings couldn't be saved
            catch (Exception)
            {
            }
            finally
            {
                if (success)
                    success &= TryReplaceFileWith(_filename, tempFilename);

                if (tempCreated)
                    TryDelete(tempFilename);
            }

            return success;
        }

        private String GetTempFilename()
        {
            return _filename + ".temp";
        }

        private static bool TryReplaceFileWith(String dest, String src)
        {
            try
            {
                File.Copy(src, dest, true);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static bool TryDelete(String file)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static XmlSerializer CreateXmlSerializer(Type type)
        {
            // This odd approach bypasses the framework's attempt to load a non-existant XML
            // serialization assembly, which would waste time and generate false debug warnings.
            return XmlSerializer.FromTypes(new[] { type })[0];
        }
    }
}
