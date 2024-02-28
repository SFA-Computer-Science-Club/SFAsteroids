using Godot;

namespace SpaceGame.Code;

public partial class FlakProjectile : RigidBody2D
{
    // Called when the node enters the scene tree for the first time.
    [Export] public int Damage = 20;

    public Ship FiredFrom;
	
    public override void _Ready()
    {
        VisibleOnScreenNotifier2D node = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        node.ScreenExited += removeProjectile;
    }

    private void removeProjectile()
    {
        QueueFree();
    }

    public int CalculateDamage(RigidBody2D node)
    {
        Vector2 nodePosition = node.Position;

        double distance = (Position - nodePosition).Length();
        
        return 0;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position += LinearVelocity * (float)delta;
    }
}