using System.Collections;
using Godot;
using Godot.Collections;

namespace SpaceGame.Code;

public partial class FlakProjectile : RigidBody2D
{
    // Called when the node enters the scene tree for the first time.
    [Export] public int Damage = 20;

    public Ship FiredFrom;
	
    public override void _Ready()
    {
        VisibleOnScreenNotifier2D node = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        node.ScreenExited += RemoveProjectile;
    }

    private void RemoveProjectile()
    {
        QueueFree();
    }

    private void Collision(Node2D node)
    {
        Area2D explosionRadius = GetNode<Area2D>("DamageRadius");

        Array<Node2D> bodies = explosionRadius.GetOverlappingBodies();
        
        GD.Print("Colliding Bodies: " + bodies.Count);
        
        Hide();
        AudioStreamPlayer2D audio = GetNode<AudioStreamPlayer2D>("FlakBurst");
        audio.Play();

        foreach (Node2D body in bodies)
        {
            if (body is RigidBody2D)
            {
                RigidBody2D rNode = (RigidBody2D)body;
                int damage = CalculateDamage(rNode);
                GD.Print("Body");

                if (rNode.IsInGroup("large_asteroid"))
                {
                    LargeAsteroid asteroid = (LargeAsteroid)rNode;
                    asteroid.Damage(damage, FiredFrom);
                }
            }
        }

        audio.Finished += Clear;
    }

    private void Clear()
    {
        QueueFree();
    }

    public int CalculateDamage(RigidBody2D node)
    {
        Vector2 nodePosition = node.Position;

        CircleShape2D circle = (CircleShape2D) GetNode<CollisionShape2D>("DamageRadius/CollisionShape2D").Shape;

        double radius = circle.Radius;
        double distance = (Position - nodePosition).Length();

        if (distance > radius)
        {
            return 0;
        }

        double dg = (1 - (distance / radius)) * 30;
        
        return (int) dg;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position += LinearVelocity * (float)delta;
    }
}