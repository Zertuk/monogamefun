Content Management
==========
Nez includes it's own content management system that builds on the MonoGame ContentManager class. All content management goes through the `NezContentManager` which is a subclass of ContentManager. The debug console has a 'assets' command that will log all scene or global assets so you will always know what is still in memory.

Nez provides containers for global and per-scene content. You can also create your own `NezContentManager` at any time if you need to manage some short-lived assets. You can also unload assets at any time via the `unloadAsset<T>` method. Note that Effects should be unloaded with `unloadEffect` since they are a special case.


## Global Content
There is a global NezContentManager available via `Core.contentManager`. You can use this to load up assets that will survive the life of your application. Things like your fonts, shared atlases, shared sound effects, etc.


## Scene Content
Each scene has it's own `NezContentManager` (scene.contentManager) that you can use to load per-scene assets. When a new scene is set all of the assets from the previous scene will automatically be unloaded for you.



## Loading Effects
There are several ways to load Effects with Nez that are not present in MonoGame. These were added to make Effect management easier, especially when dealing with Effects that are subclasses of Effect (such as AlphaTestEffect and BasicEffect). All of the built in Nez Effects can also be loaded easily. The available methods are:

- **loadMonoGameEffect<T>:** loads and manages any Effect that is built-in to MonoGame such as BasicEffect, AlphaTestEffect, etc
- **loadEffect/loadEffect<T>:** loads an ogl effect directly from file and handles disposing of it when the ContentManager is disposed
- **loadEffect<T>( string name, byte[] effectCode ):** loads an ogl effect directly from its bytes and handles disposing of it when the ContentManager is disposed
- **loadNezEffect:** loads an embedded Nez effect. These are any of the Effect subclasses in the Nez/Graphics/Effects folder


## Async Loading
`NezContentManager` provides asynchronous loading of assets as well. You can load a single asset or an array of assets via the `loadAsync<T>` method. It takes a callback Action that will be called once the assets are loaded.
	