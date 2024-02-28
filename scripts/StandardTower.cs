using Godot;
using System;

public partial class StandardTower : Panel
{
	// Called when the node enters the scene tree for the first time.
    private bool isOnPanel = false;
    PackedScene standardTower = ResourceLoader.Load("res://entities/defence_tower.tscn") as PackedScene;
    PackedScene panelSlot = ResourceLoader.Load("res://UI/StandardTower.tscn") as PackedScene;
    private string newParent = "root/TowerSpawner"; 
    private Node2D node2D;
    private CanvasLayer canvasLayer;
    public override void _Ready()
    {
        canvasLayer = (CanvasLayer)GetParent()?.GetParent()?.GetParent();
        node2D = GetParent()?.GetParent()?.GetParent()?.GetParent()?.GetNode<Node2D>("TowerSpawner");
    }

    private void _on_gui_input(InputEvent @event)
    {
        Node2D node = (Node2D)standardTower.Instantiate();
        if (@event is InputEventMouseMotion mouseButton1 && ((int)mouseButton1.ButtonMask) == 1) //Left Mouse Click And Drag
        {
            if (GetChildCount() > 0)
            {
                node = (Node2D)GetChild(0); // Assuming you want to access the first child
                node.GlobalPosition = GetGlobalMousePosition();
                canvasLayer.Visible = false;
                this.Visible = true;
                node.Visible = true;
                GD.Print(this.Visible);
                
            }
            
        }
        else if (@event is InputEventMouseButton mouseButton2 && ((int)mouseButton2.ButtonMask) == 0) //Left Mouse Click Release
        {
            if (GetChildCount() > 0 && isOnPanel == false)
            {
                node = (Node2D)GetChild(0);
                RemoveChild(node);
                node.QueueFree();
                Panel standard1 = (Panel)panelSlot.Instantiate();
                GridContainer standard2 = (GridContainer)GetParent();
                standard2.AddChild(standard1);
                node = (Node2D)standardTower.Instantiate();
                Reparent(node2D);
                QueueFree();
                
                //Node node1 = new Node();
                //standard2.AddChild(node1);
                //this.Reparent(node1 as Node);
                //GD.Print(GetTree());
                //Reparent(node2D);
                //GD.Print("dfsfsdfsd");
                //canvasLayer.Visible = true;
                //this.Visible = true;
                //node.Visible = true;
                //GD.Print(this.Visible);
            }
            canvasLayer.Visible = true;
            this.Visible = true;
            node.Visible = true;
            GD.Print(this.Visible);
            Reparent(node2D);

        }
    }




    private void _on_panel_mouse_entered()
    {
        isOnPanel = true;
    }
    private void _on_panel_mouse_exited() {
        isOnPanel = false;
    } 
}

