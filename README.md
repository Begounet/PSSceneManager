# PS Scene Manager 

## Presentation
Allow to quickly save/load (and auto-load) scenes in the editor and in build.

## How it works
* Create a scene
* Include another scene
* Go to Window/PS Scene Manager
* In this new window -> Save Scenes. It will generate a [YourSceneFirstInTheHierarchyView]_SceneData.asset in the same directory that your scene.
	* Also, it will generate a GameObject RuntimeSceneLoader, that will load all the required scenes at start from the asset.
* If you click "Restore scenes", it will load your last save.
* If you go to another scene and come back later, the scenes will be automatically loaded. 
	However, if you load the same scene again, the other scenes will not be loaded. Useful if you want to get your scene alone.