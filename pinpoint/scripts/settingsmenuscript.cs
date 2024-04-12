using Godot;
using System;

public partial class settingsmenuscript : Control
{
	private Label incSpeedLabel;
	private TextEdit incSpeedTextBox;
	private Button incSpeedEnterButton;

	private Label tickEveryLabel;
	private TextEdit tickEveryTextBox;
	private Button tickEveryEnterButton;


	private Label stopTickAtLabel;
	private TextEdit stopTickAtTextBox;
	private Button stopTickAtEnterButton;

	private logicscript logicscript;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		incSpeedLabel = GetNode<Label>("incSpeedLabel");
		incSpeedTextBox = GetNode<TextEdit>("incSpeedTextBox");
        incSpeedEnterButton = GetNode<Button>("incSpeedEnterButton");

		tickEveryLabel = GetNode<Label>("tickEveryLabel");
		tickEveryTextBox = GetNode<TextEdit>("tickEveryTextBox");
		tickEveryEnterButton = GetNode<Button>("tickEveryEnterButton");

        stopTickAtLabel = GetNode<Label>("stopTickAtLabel");
        stopTickAtTextBox = GetNode<TextEdit>("stopTickAtTextBox");
        stopTickAtEnterButton = GetNode<Button>("stopTickAtEnterButton");

		logicscript = GetNode<logicscript>("/root/logicnode");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//-------------VISIBILITY DEPENDING ON WHETHER THE WINDOW IS BORDERLESS OR NOT------------------------
		if (logicscript.isBordered == true) 
		{
			Visible = true; //be visible if window is bordered
		}
		else if (logicscript.isBordered == false) 
		{
			Visible = false; //become invisible if window is borderless
			//deactivate buttons and textboxes too i guess.
		}


		//-------------UPDATING OF LABELS WITH LOGICSCRIPT VALUES----------------------------
		incSpeedLabel.Text = "Increment Speed : " + logicscript.incrementSpeed;
		tickEveryLabel.Text = "play tick every : " + logicscript.tickEvery;
		stopTickAtLabel.Text = "stop ticking at : " + logicscript.stopTickAt;

		
		//-------------REASSIGNMENT OF VALUES ON BUTTON CLICK---------------------------
		//if button click, reassign the approprite logicscript values with value in the text boxes.



	}

	public void onIncEnterButtonClick() 
	{
	
	}
}
