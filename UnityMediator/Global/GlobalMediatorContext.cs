using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pryanik.UnityMediator.SceneBinding;
using UnityEditor;
using UnityEngine;

namespace Pryanik.UnityMediator.Global
{
    [CreateAssetMenu(fileName = FileName, order = 0)]
    public class GlobalMediatorContext : ScriptableObject
    {
        [SerializeField] private MediatorMonoInstaller[] _installers;
        
        internal const string FileName = "GlobalMediatorContext";
        internal const string MenuName = "Mediator/GlobalContext";
        
        private static GlobalMediatorContext _instance;
        internal static IEnumerable<MediatorMonoInstaller> Installers
        {
            get
            {
                _instance ??= Resources.Load<GlobalMediatorContext>(FileName);    
                return _instance == null ? Array.Empty<MediatorMonoInstaller>() : _instance._installers;
            }
        }
    }

#if UNITY_EDITOR
    
    public class GlobalMediatorContextCreator
    {
        [MenuItem("Assets/Create/"+GlobalMediatorContext.MenuName)]
        public static void CreateAsset()
        {
            string dirPath = "Assets/Resources";
            string fileName = GlobalMediatorContext.FileName + ".asset";

            var fullFileName = Path.Combine(dirPath, fileName);
            
            if (Directory.Exists(dirPath) is false)
                Directory.CreateDirectory(dirPath);

            if (File.Exists(fullFileName))
            {
                Debug.LogWarning("File already exists.");
                return;
            }
            
            var asset = ScriptableObject.CreateInstance<GlobalMediatorContext>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(fullFileName);

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
    
    #endif
}