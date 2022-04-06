using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class BuildScript
{
    /**************************************************************************************************************************************/
    #region Final Build methods for Android and Windows
    /**************************************************************************************************************************************/

    public static void BuildAndroid()
    {
        AutoLightBaker autoLightBaker = new AutoLightBaker();
        autoLightBaker.BakeLightingForScenesInBuildSettings();


        string buildVersion = "1", buildVersionCode = "1", buildNameForQuestBase = "AutomatedBuildQuest_QA";
        string timeStamp = "_" + System.DateTime.Now.Month.ToString() + "M" + System.DateTime.Now.Day.ToString() + "D" + System.DateTime.Now.Hour.ToString() + "H" + System.DateTime.Now.Minute.ToString() + "m";

        List<string> scenePaths = new List<string>();
        foreach (EditorBuildSettingsScene scenePath in EditorBuildSettings.scenes)
        {
            if (scenePath.enabled)
                scenePaths.Add(scenePath.path);
        }
        string[] allBuildScenes = scenePaths.ToArray();

        buildVersion = PlayerSettings.bundleVersion;
        buildVersionCode = PlayerSettings.Android.bundleVersionCode.ToString();
        BuildPlayerOptions buildPlayerOptionsAndroid = new BuildPlayerOptions();
        buildPlayerOptionsAndroid.scenes = allBuildScenes;
        string fullBuildNameForQuest = buildNameForQuestBase + "_" + buildVersion + "_" + buildVersionCode + timeStamp;
        buildPlayerOptionsAndroid.locationPathName = "BuildsOutput/Quest/Build/" + fullBuildNameForQuest + ".apk";
        buildPlayerOptionsAndroid.target = BuildTarget.Android;
        buildPlayerOptionsAndroid.options = BuildOptions.None;

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Others/KeyStore/userAutoMatedBuild.keystore";
        PlayerSettings.Android.keystorePass = "8dsp?Q7mS87fHxRG";
        PlayerSettings.Android.keyaliasName = "district m key";
        PlayerSettings.Android.keyaliasPass = "8dsp?Q7mS87fHxRG";

        // Check and create the Directories if doesn't exist
        CreateBuildPaths();

        // Write the build name to file
        WriteBuildNameToFile(BuildTarget.Android, fullBuildNameForQuest);

        Debug.Log("BuildScript : Starting android build.");
        // Start the build
        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptionsAndroid);
        BuildSummary buildSummary = buildReport.summary;
    }

    public static void BuildPCVR()
    {
        string buildVersion = "1", baseBuildNameForPCVR = "AutomatedBuildPCVR";
        string timeStamp = "_" + System.DateTime.Now.Month.ToString() + "M" + System.DateTime.Now.Day.ToString() + "D" + System.DateTime.Now.Hour.ToString() + "H" + System.DateTime.Now.Minute.ToString() + "m";

        List<string> scenePaths = new List<string>();
        foreach (EditorBuildSettingsScene scenePath in EditorBuildSettings.scenes)
        {
            if (scenePath.enabled)
                scenePaths.Add(scenePath.path);
        }
        string[] allBuildScenes = scenePaths.ToArray();

        buildVersion = PlayerSettings.bundleVersion;
        BuildPlayerOptions buildPlayerOptionsWindows = new BuildPlayerOptions();
        buildPlayerOptionsWindows.scenes = allBuildScenes;
        string fullBuildNameForPCVR = baseBuildNameForPCVR + "_" + buildVersion + "_" + timeStamp;
        buildPlayerOptionsWindows.locationPathName = "BuildsOutput/PCVR/Build/" + fullBuildNameForPCVR + ".exe";
        buildPlayerOptionsWindows.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptionsWindows.options = BuildOptions.None;

        // Check and create the Directories if doesn't exist
        CreateBuildPaths();

        // Write the build name to file
        WriteBuildNameToFile(BuildTarget.StandaloneWindows64, fullBuildNameForPCVR);

        Debug.Log("BuildScript : Starting windows build.");
        // Start the build
        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptionsWindows);
        BuildSummary buildSummary = buildReport.summary;
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
    #region Create build paths if they don't exist
    /**************************************************************************************************************************************/

    private static void CreateBuildPaths()
    {
        // In case the build paths are not created then create them before building
        // Otherwise throws error
        string rootBuildDirectory = "/BuildsOutput";
        string questBuildDirectory = "BuildsOutput/Quest";
        string pcvrBuildDirectory = "BuildsOutput/PCVR";

        Debug.Log("BuildScript : Checking and creating build path.");
        if (Directory.Exists(rootBuildDirectory) == false)
            Directory.CreateDirectory(rootBuildDirectory);
        if (Directory.Exists(questBuildDirectory) == false)
            Directory.CreateDirectory(questBuildDirectory);
        if (Directory.Exists(pcvrBuildDirectory) == false)
            Directory.CreateDirectory(pcvrBuildDirectory);
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
    #region Write the build name to text file
    /**************************************************************************************************************************************/

    private static void WriteBuildNameToFile(BuildTarget buildTarget, string buildName)
    {
        // The build name is written to a text file
        // Teamcity powershell script reads the name from it and puts the same name for the compressed build file for uploading

        Debug.Log("BuildScript : Writing build name to file.");
        string path = "";
        if (buildTarget == BuildTarget.Android)
            path = "BuildsOutput/Quest/" + "BuildNameQuest.txt";
        else
            path = "BuildsOutput/PCVR/" + "BuildNamePCVR.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(buildName);
        writer.Close();
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
}


//https://github.com/Nordeus/UnityBuildPipeline/blob/master/CommandLineBuild.cs