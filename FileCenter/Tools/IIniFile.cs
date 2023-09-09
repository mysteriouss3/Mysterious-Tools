using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousTools.FileCenter
{
    public interface IIniFile
    {
        string FileName { get; set; }

        void AddSection(IniSection section);
        void AddSection(string sectionName);
        void RemoveSection(string sectionName);

        void Parse();
        void Reload();
        bool SectionExists(string sectionName);

        bool KeyExists(string sectionName, string keyName);
        int GetIntValue(string section, string key, int defaultValue);
        void SetIntValue(string section, string key, int value);

        string GetStringValue(string section, string key, string defaultValue);
        void SetStringValue(string section, string key, string value);

        bool GetBooleanValue(string section, string key, bool defaultValue);
        void SetBooleanValue(string section, string key, bool value);


        double GetDoubleValue(string section, string key, double defaultValue);
        void SetDoubleValue(string section, string key, double value);
        

        void WriteIniFile();
        void WriteIniFile(string filePath);
        void WriteIniStream(Stream stream);

    }
}
