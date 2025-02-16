using FarlandsCoreMod;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Variable de depuración
        bool debug = true;

        // NO TOCAR
        bool fcmLocal = true;
        bool copy = true;
        bool zip = true;

        // Obtener la ruta de la variable de entorno FARLANDS_PATH
        Console.WriteLine("Iniciando el proceso de copia de archivos...");
        string farlandsPath = Environment.GetEnvironmentVariable("FARLANDS_PATH");
        if (string.IsNullOrEmpty(farlandsPath))
        {
            Console.WriteLine("Error: La variable de entorno FARLANDS_PATH no está configurada.");
            return;
        }

        // Definir la ruta de destino
        string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "farlands");

        // Verificar si la carpeta de destino existe y comparar archivos globalgamemanagers
        if (Directory.Exists(destinationPath))
        {
            string sourceFile = Path.Combine(farlandsPath, "Farlands_Data", "globalgamemanagers");
            string destinationFile = Path.Combine(destinationPath, "Farlands_Data", "globalgamemanagers");

            if (File.Exists(sourceFile) && File.Exists(destinationFile) && FilesAreEqual(sourceFile, destinationFile))
            {
                Console.WriteLine("Los archivos globalgamemanagers en FARLANDS_PATH y la carpeta de destino son iguales. No se requiere copia.");
                copy = false;
            }
            else
            {
                Console.WriteLine("Los archivos globalgamemanagers son diferentes. Eliminando la carpeta de destino existente...");
                Directory.Delete(destinationPath, true);
            }
        }

        if (copy)
        {
            // Copiar la carpeta FARLANDS_PATH a ./farlands
            Console.WriteLine($"Copiando archivos desde {farlandsPath} a {destinationPath}...");
            CopyDirectory(farlandsPath, destinationPath);
        }

        string destinationPlugins = Path.Combine(destinationPath, "BepInEx", "plugins");
        DownloadBepInEx(destinationPath);

        if (fcmLocal) CopyFCM(destinationPath, destinationPlugins);
        else DownloadFCM(destinationPlugins);

        // Copiar mono-2.0-bdwgc.dll si debug es true
        if (debug)
        {
            string toolsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Tools");
            string dllSourcePath = Path.Combine(toolsPath, "mono-2.0-bdwgc.dll");
            string dllDestinationPath = Path.Combine(destinationPath, "MonoBleedingEdge", "EmbedRuntime", "mono-2.0-bdwgc.dll");

            if (File.Exists(dllSourcePath))
            {
                Console.WriteLine($"Copiando {dllSourcePath} a {dllDestinationPath}...");
                File.Copy(dllSourcePath, dllDestinationPath, true);
            }
            else
            {
                Console.WriteLine($"Error: No se encontró {dllSourcePath}.");
            }

            // Copiar la carpeta sinai-dev-UnityExplorer
            string sinaiSourcePath = Path.Combine(toolsPath, "sinai-dev-UnityExplorer");
            string sinaiDestinationPath = Path.Combine(destinationPlugins, "sinai-dev-UnityExplorer");

            if (Directory.Exists(sinaiSourcePath))
            {
                Console.WriteLine($"Copiando carpeta {sinaiSourcePath} a {sinaiDestinationPath}...");
                CopyDirectory(sinaiSourcePath, sinaiDestinationPath);
            }
            else
            {
                Console.WriteLine($"Error: No se encontró la carpeta {sinaiSourcePath}.");
            }
        }

        Console.WriteLine("Operación completada.");

        // Ejecutar Farlands.exe
        string farlandsExePath = Path.Combine(destinationPath, "Farlands.exe");
        if (File.Exists(farlandsExePath))
        {
            Console.WriteLine($"Ejecutando {farlandsExePath}...");
            Process.Start(farlandsExePath);
        }
        else
        {
            Console.WriteLine($"Error: No se encontró {farlandsExePath}.");
        }
    }
    static void DownloadBepInEx(string destinationPath)
    {
        string bepinexUrl = "https://github.com/BepInEx/BepInEx/releases/download/v6.0.0-pre.1/BepInEx_UnityMono_x64_6.0.0-pre.1.zip";
        string bepinexZipPath = Path.Combine(Path.GetTempPath(), "BepInEx.zip");
        string bepinexExtractPath = Path.Combine(destinationPath, "BepInEx", "core");

        if (!Directory.Exists(bepinexExtractPath))
        {
            Console.WriteLine($"Descargando BepInEx desde {bepinexUrl}...");
            using (var client = new WebClient())
            {
                client.DownloadFile(bepinexUrl, bepinexZipPath);
            }

            Console.WriteLine($"Extrayendo BepInEx a {destinationPath}...");
            ZipFile.ExtractToDirectory(bepinexZipPath, destinationPath, true);

            // Eliminar el archivo ZIP descargado
            File.Delete(bepinexZipPath);
        }
        else
        {
            Console.WriteLine("BepInEx ya está instalado.");
        }
    }
    static void CopyFCM(string destinationPath, string destinationPlugins)
    {
        // Buscar el archivo ZIP en el proyecto principal
        string sourceDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "FCM", "bin", "Debug");
        string zipPattern = "FCM_*.zip";
        string[] zipFiles = Directory.GetFiles(sourceDir, zipPattern);

        if (zipFiles.Length == 0)
        {
            Console.WriteLine($"Error: No se encontró ningún archivo que coincida con el patrón {zipPattern} en {sourceDir}.");
            return;
        }

        // Seleccionar el archivo ZIP con la mayor versión
        string sourceZipPath = zipFiles
            .Select(file => new { File = file, Version = GetVersionFromFileName(file) })
            .OrderByDescending(x => x.Version)
            .First().File;

        

        // Crear la carpeta de destino si no existe
        Directory.CreateDirectory(destinationPlugins);

        // Extraer el archivo ZIP a un directorio temporal
        string tempExtractPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempExtractPath);
        Console.WriteLine($"Extrayendo {sourceZipPath} a {tempExtractPath}...");
        ZipFile.ExtractToDirectory(sourceZipPath, tempExtractPath);

        // Copiar archivos del directorio temporal al destino, sobrescribiendo los existentes
        Console.WriteLine($"Copiando archivos desde {tempExtractPath} a {destinationPlugins}...");
        CopyDirectory(tempExtractPath, destinationPlugins);

        // Eliminar el directorio temporal
        Directory.Delete(tempExtractPath, true);
    }
    static void DownloadFCM(string destinationPlugins)
    {
        string fcmUrl = $"https://farlands-geturl.maginciangeneral.workers.dev?mod=magin.fcm&version={FCMInfo.Version}";
        Console.WriteLine($"Obteniendo {fcmUrl}...");

        string zipPath = Path.Combine(Path.GetTempPath(), "FCM.zip");

        using (var client = new WebClient())
        {
            var uri = client.DownloadString(fcmUrl);
            Console.WriteLine($"Uri obtenida: {uri}");
            client.DownloadFile(uri, zipPath);
        }
        Console.WriteLine($"Descargado FCM en {zipPath}");

        // Crear la carpeta de destino si no existe
        Directory.CreateDirectory(destinationPlugins);

        // Extraer el archivo ZIP a un directorio temporal
        string tempExtractPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempExtractPath);
        Console.WriteLine($"Extrayendo {zipPath} a {tempExtractPath}...");
        ZipFile.ExtractToDirectory(zipPath, tempExtractPath);

        // Copiar archivos del directorio temporal al destino, sobrescribiendo los existentes
        Console.WriteLine($"Copiando archivos desde {tempExtractPath} a {destinationPlugins}...");
        CopyDirectory(tempExtractPath, destinationPlugins);

        // Eliminar el directorio temporal
        Directory.Delete(tempExtractPath, true);
    }
    static void CopyDirectory(string sourceDir, string destinationDir)
    {
        Directory.CreateDirectory(destinationDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
            Console.WriteLine($"Copiando archivo {file} a {destFile}...");
            File.Copy(file, destFile, true);
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            string destDir = Path.Combine(destinationDir, Path.GetFileName(directory));
            Console.WriteLine($"Copiando directorio {directory} a {destDir}...");
            CopyDirectory(directory, destDir);
        }
    }

    static Version GetVersionFromFileName(string fileName)
    {
        string pattern = @"FCM_(\d+\.\d+\.\d+\.\d+)\.zip";
        Match match = Regex.Match(fileName, pattern);
        if (match.Success)
        {
            return Version.Parse(match.Groups[1].Value);
        }
        return new Version(0, 0, 0, 0);
    }

    static bool FilesAreEqual(string filePath1, string filePath2)
    {
        using (var hashAlgorithm = SHA256.Create())
        {
            byte[] hash1 = hashAlgorithm.ComputeHash(File.ReadAllBytes(filePath1));
            byte[] hash2 = hashAlgorithm.ComputeHash(File.ReadAllBytes(filePath2));
            return hash1.SequenceEqual(hash2);
        }
    }
}
