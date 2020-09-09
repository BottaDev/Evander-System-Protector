using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Drawing;

public class GetFileName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rand = new UnityEngine.Random();
        var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        string path = files[UnityEngine.Random.Range(0, files.Length)];
        print("File Path: " + path);

        // Full file name   
        FileInfo fi = new FileInfo(path);

        // Get File Name  
        string justFileName = fi.Name;

        // Get file name with full path   
        string fullFileName = fi.FullName;

        // Get file extension   
        string extn = fi.Extension;

        // Get directory name   
        string directoryName = fi.DirectoryName;

        // File Exists ?  
        bool exists = fi.Exists;

        if (fi.Exists)
        {
            // Get file size  
            long size = fi.Length;

            // File ReadOnly ?  
            bool IsReadOnly = fi.IsReadOnly;

            // Creation, last access, and last write time   
            DateTime creationTime = fi.CreationTime;
            DateTime accessTime = fi.LastAccessTime;
            DateTime updatedTime = fi.LastWriteTime;
        }
    }
}
