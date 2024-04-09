using Godot;
using System;
//using mrousavy; //mrousavyglobalhotkey package

using GlobalHotKeys; // the other globalhotkey pavkage 
using GlobalHotKeys.Native.Types;




public partial class logicscript : Node2D
{
	private float leftFlipperValue;
	private float rightFlipperValue;

	private float incrementSpeed = 60; //number per second
									   //10 is default.

	public bool leftFlipperActive; //making this public allows hotkeypressed functions to change this value while being void
	public bool rightFlipperActive;

	private bool leftFlipValIsReset;
	private bool rightFlipValIsReset;

	private float leftFlipperHistory;
	private float rightFlipperHistory;

	private HotKeyManager hotKeyManager;

	//private HotKey leftButton;
	private IRegistration leftButton;
	private IRegistration NotPressed;

	private IDisposable subscription;

	//private HotKey key = new HotKey; 

	// Called when the node enters the scene tree for the first time.




	public override void _Ready()
	{

		//var key = new HotKey(mod, Key.S, this);



		hotKeyManager = new HotKeyManager();
		subscription = hotKeyManager.HotKeyPressed.Subscribe(HotKeyPressed); //i dont fukin know what 'subscribing' does atm
																			 //oh god, it finally sorta worked after making (private IDisposable subscription;).
																			 //now, hotkeypressed() runs after pressing a button.

		leftButton = hotKeyManager.Register(VirtualKeyCode.KEY_S, 0); //idk, modifier is required to register a hotckey even tho i dont wanna modifier. however, intellicode seems to indicate that "modifiers" is an int of some kind.
																	  //with most of the options seemigly being readable shorthand for simple numbers (e.g. control button is = 2)
																	  //so maybe putting zero here can make it so there is no modifyer.
																	  //nvm. it seems that options for modifyers really seem to be limited to the keys given bu intellicode.
																	  //nvm nvm. tried 0 modifyier with the example code in a seperate test project and it works 

		NotPressed = hotKeyManager.Register(0, 0);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


		//-----------------INPUT AFFECTING THE BOOLEANS------------------------------

		/*
        if (Input.IsActionPressed("leftFlipperButton") || hotKeyManager.HotKeyPressed.Equals(leftButton) ) //idk im just shooting at the dark here
		{
			leftFlipperActive = true;
		}
		else
		{
			leftFlipperActive = false;
		}

		if (Input.IsActionPressed("rightFlipperButton"))
		{
			rightFlipperActive = true;
		}
		else
		{
			rightFlipperActive = false;
		}
		*/

		/*
        if (Input.IsActionPressed("leftFlipperButton") == false)
        {
            leftFlipperActive = false;
        }
		*/ //nope. dosent work.

		/*
        if (leftButton.)
        {
			GD.Print("hi");
            leftFlipperActive = false;
        }
		*/

		//NotPressed.Equals(true); //huh, no syntax error.

		 

        //----------------------
        if (leftFlipperActive == false) //flipper is down (about to aim)
		{

			if (leftFlipValIsReset == false)
			{

				leftFlipperValue = 0; //reset value
				leftFlipValIsReset = true;

			}
			else if (leftFlipValIsReset == true)
			{
				leftFlipperValue += incrementSpeed * (float)delta; //start incrementing once the value is properly reset.
			}

		}
		else if (leftFlipperActive == true) //if flipper is flipped (during cradle/after shooting ball)
		{
			leftFlipperHistory = leftFlipperValue; //assign current calue to flipper history
			leftFlipValIsReset = false; //make isreset false so that the value can reset once flipper is put back down.
		}



        if (rightFlipperActive == false) //flipper is down (about to aim)
        {

            if (rightFlipValIsReset == false)
            {

                rightFlipperValue = 0; //reset value
                rightFlipValIsReset = true;

            }
            else if (rightFlipValIsReset == true)
            {
                rightFlipperValue += incrementSpeed * (float)delta; //start incrementing once the value is properly reset.
            }

        }
        else if (rightFlipperActive == true) //if flipper is flipped (during cradle/after shooting ball)
        {
            rightFlipperHistory = leftFlipperValue; //assign current calue to flipper history
            rightFlipValIsReset = false; //make isreset false so that the value can reset once flipper is put back down.
        }




        //-------------------UPDATING GUI WITH THE VALUES-------------------------

        GetNode<Label>(@"/root/logicnode/leftFlip/value").Text = leftFlipperValue.ToString();
		GetNode<Label>(@"/root/logicnode/rightFlip/value").Text = rightFlipperValue.ToString();

		GetNode<Label>(@"/root/logicnode/leftFlip/value/history").Text = leftFlipperHistory.ToString();
	}


	private void HotKeyPressed(HotKey hotkeyparam) //this needs to be void
	{
		GD.Print("hotkeypressed");
		leftFlipperActive = true;

		if (hotkeyparam.Key == VirtualKeyCode.KEY_S)
		{
			leftflipperAction(); //runs if the globahotkey detected is the left button
		}

        if (hotkeyparam.Key == VirtualKeyCode.KEY_K)
        {
            rightflipperAction(); //runs if the globahotkey detected is the left button
        }

        /*
        if (!Input.IsActionPressed("leftFlipperButton"))
        {
            leftFlipperActive = false;
        }
		*/ //currently shooting at the dark. but it dosent work here. maybe it works on process..?
           //nope. dosent work.
           //maybe a "leftbuttonnotpressed" hotkey object that has 0 in both parameter values?

        
		if (hotkeyparam.Key == 0) 
		{
            leftFlipperActive = false;
			GD.Print("nothing pressed");
        }
		 //nope. this dosent work either. maybe an else statement here, and when nothing is pressed in process, call this function???
		   //maybe just put a timer that makes it false. no the thing should start incrementing precisely as the flipper goes down. idk anymore.
    }

    private void leftflipperAction()
	{
		if (Input.IsActionPressed("leftFlipperButton")) //leftflipper button (godoteditor) should be set the same as the globalhotkey button.
														//basically, the godot action button is what triggers the actual in-app numbers. the globalhotkey basically acts like a "gate 1" of sorts.
														//after passing thorugh the gate, it fuckin.. godot input time.
		{
			leftFlipperActive = true; //these values are made public cuz the hotkeypressed() function where these funcions are called in NEEDS to be void.
									  //also idk how to give it parameters with how the nuget package is used.
		}
		else /*if (Input.IsActionJustReleased("leftFlipperButton"))*/

        {
			//leftFlipperActive = false; // leaving this in will make it false the entire time. leaving it out only allows it to be active once. (tldr leftflipperactive dosent become false again when flipper gpes back down)
			//maybe make a if(buttonreleased) in process to make it false again, although i dont think it would work, since that method requires the window to be focused..
			//the hotkey package has no thingy fo retecting keyup soo
			//
			//then again, in retrospect, i wonder how the heck this managed to work, as apparently the if() statement above runs no problem when unfocused.
			//shoulod it *not* run? since the window is unfocused and cannot accept these types of input?
		}
	}

    private void rightflipperAction()
    {
        if (Input.IsActionPressed("rightFlipperButton")) //leftflipper button (godoteditor) should be set the same as the globalhotkey button.
                                                        //basically, the godot action button is what triggers the actual in-app numbers. the globalhotkey basically acts like a "gate 1" of sorts.
                                                        //after passing thorugh the gate, it fuckin.. godot input time.
        {
            rightFlipperActive = true;
        }
        else 
        {
            //rightFlipperActive = false;
        }
    }

}
