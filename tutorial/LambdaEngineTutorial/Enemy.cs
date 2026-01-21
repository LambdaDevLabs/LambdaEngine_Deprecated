using System.Numerics;
using LambdaEngine;
using LambdaEngine.InputSystem;
using LambdaEngine.SceneModule;

namespace LambdaEngineTutorial;

public class Enemy : BehaviourComponent, IDamageable {
    private static Sprite sprite;
    private static Random random;
    
    public GameManager gameManager;

    private float radius;

    private float speed;

    private float health = 10;

    [LifecycleAwake]
    protected virtual void Awake() {
        if (sprite == null) {
            sprite = Sprite.CreateWithTexture("Assets/enemy.bmp");
        }

        if (random == null) {
            random = new Random();
        }
    }
    
    [LifecycleStart]
    protected virtual void Start() {
        /*
         pixelsPerUnit = 100;
         spriteSize = 10x10 Pixels;
         defaultDiameter = 1 Unit = 100 Pixels;

         diameter = spriteSize / pixelsPerUnit = 10 / 100 = 0.1
         diameter = 0.1 Units
         radius = 0.05 Units
        */
        float scale = random.Next(10, 25) * 0.1f;
        radius = scale * 0.5f;
        
        SpriteRenderer spriteRenderer = CreateComponent<SpriteRenderer>();
        spriteRenderer.Sprite = sprite;
        spriteRenderer.Layer = 1;
        spriteRenderer.Scale = new Vector2(scale);
        spriteRenderer.Color = GenRandColor();
        
        CircleCollider circleCollider = CreateComponent<CircleCollider>();
        circleCollider.Radius = radius;

        speed = 3 / scale;
        health = 15 * scale;
    }

    [LifecycleUpdate]
    protected virtual void Update() {
        transform.Translate(-Vector2.UnitY * (speed * Time.DeltaTime));

        if (transform.Position.Y <= -4) {
            gameManager.LooseGame();
            gameManager.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }

    public void Damage(float amount) {
        health -= amount;

        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        gameManager.IncrementScore();
        
        gameManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    private Color GenRandColor() {
        return new Color(1, random.NextSingle(), random.NextSingle(), random.NextSingle());
    }
}