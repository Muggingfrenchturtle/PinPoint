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


		incSpeedEnterButton.Pressed += onIncEnterButtonClick; //connects the signal that emmits from the button to the function below.
		tickEveryEnterButton.Pressed += onTickEveryEnterButtonClick;
		stopTickAtEnterButton.Pressed += onStopTickAtEnterButtonClick;

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
		try
		{
			logicscript.incrementSpeed = incSpeedTextBox.Text.ToFloat(); //updates the logicscript value with the content of the textboxes
		}
		catch(Exception ex) //incase the user input is invalid 
		{
			GD.PushError(ex); //intellicode told me to use gd.pusherror rather than gd.print
		}
	}

	public void onTickEveryEnterButtonClick() 
	{
        try
        {
            logicscript.tickEvery = tickEveryTextBox.Text.ToFloat(); //updates the logicscript value with the content of the textboxes
        }
        catch (Exception ex) //incase the user input is invalid 
        {
            GD.PushError(ex); 
        }
    }

    public void onStopTickAtEnterButtonClick()
    {
        try
        {
            logicscript.stopTickAt = stopTickAtTextBox.Text.ToFloat(); //updates the logicscript value with the content of the textboxes
        }
        catch (Exception ex) //incase the user input is invalid 
        {
            GD.PushError(ex);
        }
    }
}
