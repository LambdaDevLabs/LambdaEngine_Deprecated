using System.Numerics;
using LambdaEngine;
using LambdaEngine.InputSystem;
using LambdaEngine.SceneModule;

namespace LambdaEngineTutorial;

public class MainMenu : BehaviourComponent {
    private TextRenderer textRenderer;
    private readonly Keys startKey = Keys.SPACE;

    private bool started;
    private float opacity = 1;
    
    [LifecycleStart]
    protected virtual void Start() {
        textRenderer = CreateComponent<TextRenderer>();

        textRenderer.Scale = new Vector2(3.5f, 3.5f);
        textRenderer.Color = Color.Black;
        textRenderer.Text = $"Press <{startKey}> to start.";
    }

    [LifecycleUpdate]
    protected virtual void Update() {
        if (!started && Input.GetKeyDown(startKey)) {
            started = true;
        }

        if (started) {
            if (opacity > 0) {
                opacity -= Time.DeltaTime;
                if (opacity < 0) {
                    opacity = 0;
                }
                
                textRenderer.Color = new Color(opacity, 0, 0, 0);
            }
            else {
                StartGame();
            }
        }
    }

    private void StartGame() {
        Debug.Log("Start Game");
        
        GameObject gameManager = Instantiate();
        gameManager.CreateComponent<GameManager>();
        
        Destroy(gameObject);
    }
}