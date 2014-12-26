using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using clojure.lang;
using clojure.clr.api;




public class ClojureNewFile : EditorWindow {
  // TODO support nested folders, hyphen/underscores
  [MenuItem ("Assets/Create/Clojure Component", false, 90)]
  [MenuItem ("Arcadia/New Component", false, 90)]
  public static void NewComponent () {
	var paths = GetPaths();
    var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
    
    ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
      ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction,
      paths[0] + "/new-component.clj",
      null,
      paths[2] + "/new-component-template.clj.txt");
  }
  
  [MenuItem ("Arcadia/New File", false, 91)]
  public static void NewFile () {
	var paths = GetPaths();
    var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");
    
    ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
      ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction,
      paths[0] + "/new-file.clj",
      null,
      paths[2] + "/new-file-template.clj.txt");
  }



  private static String[] GetPaths() {
	RT.load("arcadia/config");
	// load config file and get the paths from it
	RT.var("arcadia.config", "update-from-default-location!").invoke();
	var val_in = RT.var("arcadia.config", "value-in");
	PersistentVector paths = (PersistentVector) val_in.invoke(Clojure.read("[:compiler :load-path]"));
	// TODO change config file to use a map so that the order doesn't matter here
	String scripts_path = (String) paths.nth(0);
	String lib_path = (String) paths.nth(1);
	String editor_path = (String) paths.nth(2);
	
	return new String[]{scripts_path, lib_path, editor_path};
  }
	

}