using System.Numerics;
using LambdaEngine;
using LambdaEngine.InputSystem;
using LambdaEngine.RenderSystem;
using LambdaEngine.SceneModule;

namespace LambdaEngineTutorial;

public class GameManager : BehaviourComponent {
    private const Keys PLAY_AGAIN_KEY = Keys.RETURN;
    private const Keys EXIT_KEY = Keys.ESCAPE;
    
    private const int SCORE_TO_WIN = 10;
    
    private Random random;
    private TextRenderer scoreLblRenderer;

    private GameObject gameOverLblObj;
    private GameObject endMenuObj0;
    private GameObject endMenuObj1;

    private Player player;

    private List<GameObject> enemies;
    
    private float cooldown;

    private int score;
    
    private bool gameOver;
    
    [LifecycleStart]
    protected virtual void Start() {
        random = new Random();

        enemies = new List<GameObject>(16);
        
        GameObject scoreLblObj = Instantiate(new Vector2(-7.5f, 4.65f)); 
        scoreLblRenderer = scoreLblObj.CreateComponent<TextRenderer>();
        scoreLblRenderer.Layer = 3;
        scoreLblRenderer.Scale = new Vector2(2, 2);
        scoreLblRenderer.Color = Color.Black;
        
        InitGame();
    }

    private void InitGame() {
        GameObject player = Instantiate(new Vector2(0, -4));
        this.player =  player.CreateComponent<Player>();
        this.player.enable = true;

        score = 0;
        gameOver = false;
        
        scoreLblRenderer.Text = $"Score: {score, 3}";

        cooldown = RandomCooldown();
    }

    [LifecycleUpdate]
    protected virtual void Update() {
        if (!gameOver) {
            if (cooldown <= 0) {
                SpawnEnemy();
                cooldown = RandomCooldown();
            }
            else {
                cooldown -= Time.DeltaTime;
            }
        }
        else {
            if (Input.GetKeyDown(EXIT_KEY)) {
                Debug.Log("Exiting game...");
                Environment.Exit(0);
            }

            if (Input.GetKeyDown(PLAY_AGAIN_KEY)) {
                Debug.Log("Restarting game...");
                PlayAgain();
            }
        }
    }

    public void IncrementScore(int amount = 1) {
        if (gameOver) {
            return;
        }
        
        score += amount;
        scoreLblRenderer.Text = $"Score: {score,3}";

        if (score >= SCORE_TO_WIN) {
            EndGame(true);
        }
    }

    public void LooseGame() {
        if (gameOver) {
            return;
        }
        
        EndGame(false);
    }

    public void RemoveEnemy(GameObject enemyObj) {
        enemies.Remove(enemyObj);
    }

    private void EndGame(bool won) {
        Debug.Log("Game ended. Won: " + won);
        
        gameOver = true;
        player.enable = false;
        
        gameOverLblObj = Instantiate(new Vector2(0, 2.5f));
        TextRenderer gameOverLbl = gameOverLblObj.CreateComponent<TextRenderer>();
        gameOverLbl.Layer = 3;
        gameOverLbl.Scale = new Vector2(4, 4);
        gameOverLbl.Color = Color.Black;
        gameOverLbl.Text = won ? "You won!" : "You lost!";

        endMenuObj0 = Instantiate(new Vector2(0, 0.1f));
        endMenuObj1 = Instantiate(new Vector2(0, -0.55f));
        
        TextRenderer endOption0 = endMenuObj0.CreateComponent<TextRenderer>();
        TextRenderer endOption1 = endMenuObj1.CreateComponent<TextRenderer>();

        endOption0.Layer = 3;
        endOption0.Scale = new Vector2(3, 3);
        endOption0.Color = Color.Black;
        endOption0.Text = $"Play Again <{PLAY_AGAIN_KEY}>";

        endOption1.Layer = 3;
        endOption1.Scale = new Vector2(3, 3);
        endOption1.Color = Color.Black;
        endOption1.Text = $"Exit <{EXIT_KEY}>";
    }

    private void PlayAgain() {
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
        
        enemies.Clear();
        
        Destroy(player.gameObject);
        
        Destroy(gameOverLblObj);
        Destroy(endMenuObj0);
        Destroy(endMenuObj1);
        
        InitGame();
    }

    private void SpawnEnemy() {
        GameObject enemyObj = Instantiate(new Vector2(random.Next(-7, 7), Camera.Size));
        Enemy enemy = enemyObj.CreateComponent<Enemy>();
        enemy.gameManager = this;

        enemies.Add(enemyObj);
    }

    private float RandomCooldown() {
        return random.NextSingle() * 2 + 1;
    }
}