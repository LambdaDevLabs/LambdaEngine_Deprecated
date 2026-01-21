using System.Numerics;
using LambdaEngine;
using LambdaEngine.DebugSystem;
using LambdaEngine.InputSystem;
using LambdaEngine.PhysicsSystem;
using LambdaEngine.PlatformSystem;
using LambdaEngine.RenderSystem;
using LambdaEngine.SceneModule;
using LambdaEngine.TimeSystem;

namespace LambdaEngineTutorial;

internal class Program {
    private static void Main(string[] args) {
        DefaultRenderSystem renderSystem = new();
        DefaultInputSystem inputSystem = new();
        
        DefaultPlatformSystem platformSystem = new(renderSystem, inputSystem, null!);

        DefaultDebugSystem debugSystem = new();
        
        DefaultTimeSystem timeSystem = new();
        
        DefaultSceneModule sceneModule = new();
        
        DefaultPhysicsSystem physicsSystem = new();

        LambdaEngine.LambdaEngine engine = new(debugSystem, platformSystem, timeSystem, physicsSystem, sceneModule);

        platformSystem.SetWindowSize(1280, 720);

        platformSystem.WindowTitle = "Circle Shooter";

        platformSystem.AppName = "CircleShooter";
        platformSystem.AppVersion = "1.0";
        platformSystem.AppIdentifier = "io.github.blabliblubpaul.circle-shooter";

        renderSystem.vSyncMode = VSyncMode.NORMAL;

        Scene scene = new(sceneModule);

        engine.Initialize(true, scene);

        GameObject mainMenu = scene.Instantiate();
        mainMenu.CreateComponent<MainMenu>();
        
        engine.Run();
    }
}