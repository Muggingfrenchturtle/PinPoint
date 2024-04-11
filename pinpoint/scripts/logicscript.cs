using Godot;
using System;
//using mrousavy; //mrousavyglobalhotkey package

//using GlobalHotKeys; // the other globalhotkey nuget package by 8
//using GlobalHotKeys.Native.Types;




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

	private float tickEvery = 5; //after every x gets incremented, play a sound
	private float stopTickAt = 150; //if value is at or beyond this value, stop making sounds.

	//private HotKeyManager hotKeyManager;

	//private HotKey leftButton;
	//private IRegistration leftButton;
	//private IRegistration NotPressed;

	//private IDisposable subscription;

	//private HotKey key = new HotKey; 


	private GlobalInputCSharp GlobalInput; //darnoman global input plugin



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        //var key = new HotKey(mod, Key.S, this);


        /*
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
		*/



        //GlobalInput = GetNode("/root/GlobalInput/GlobalInputCSharp") //example in the readme (dosent work)
        //GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp"); // exapmple in the "charp_example" folder in the "examples" folder in the github

        GlobalInput = GetNode<GlobalInputCSharp>("/root/logicnode/GlobalInputCSharp"); //HOLY SHIT IT WORKS.
																					   //i needed to add the "GlobalInputCSharp.tscn" file into the main scene from the "autoloads" folder.
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


		//-----------------INPUT AFFECTING THE BOOLEANS------------------------------

		
        if (GlobalInput.IsActionPressed("leftFlipperButton")) 
		{
			leftFlipperActive = true;
		}
		else
		{
			leftFlipperActive = false;
		}

		if (GlobalInput.IsActionPressed("rightFlipperButton"))
		{
			rightFlipperActive = true;
		}
		else
		{
			rightFlipperActive = false;
		}
		

		//--------------BORDER / BORDERLESS WINDOW TOGGLE----------------------------
		if (GlobalInput.IsActionPressed("settingsModifier") && GlobalInput.IsActionJustPressed("settingsBorderToggle") ) 
		{
			GD.Print("keys pressed");

            //GD.Print("bordervalue = " + DisplayServer.WindowFlags.Borderless.ToString()); //tf, the value is literally "borderless"?
																							//dosent seem like a value.

            GD.Print("bordervalue = " + DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless) ); //bruh. yeah. in order to modify windowflags. you need to use get() and set().
																											 //which, in retrospect, makes sense ig.
																											 //making these values freely directly acessible would be bad cuz.. idk but its bad.


            if (DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless) == false)
			{
				//DisplayServer.WindowFlags.Borderless = false; //window_flag_borderless
																//https://docs.godotengine.org/en/stable/classes/class_displayserver.html
																//it cant be set like this.

				DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true); //huh. just sifting through the intellicode menus for a few moments can sometimes lead to finding the answer.
                                                                                         //wierd way to set a value tho, but idk anything about proper code writing practices, so what do i know.
                GD.Print("keys pressed 2");

            }
			else if (/*DisplayServer.WindowFlags.Borderless.Equals(1)*/ DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless) == true)
            {
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false); //inefficient, but readable for me in the future.
                GD.Print("keys pressed 2");
            }

        }





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



        //----------MAIN VALUE LOGIC(?)------------

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

				if (IsDivisible(leftFlipperValue,tickEvery) == true && leftFlipperValue < stopTickAt) //if value is a multiple of tickevery, play a sound
																									  //the stoptick thing is a bit inconsistent, but it works.
				{
					GetNode<AudioStreamPlayer2D>("/root/logicnode/leftFlip/audioPlayer").Play();
				}
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

                if (IsDivisible(rightFlipperValue, tickEvery) == true && rightFlipperValue < stopTickAt) //if value is a multiple of tickevery, play a sound
                {
                    GetNode<AudioStreamPlayer2D>("/root/logicnode/rightFlip/audioPlayer").Play();
                }
            }

        }
        else if (rightFlipperActive == true) //if flipper is flipped (during cradle/after shooting ball)
        {
            rightFlipperHistory = rightFlipperValue; //assign current calue to flipper history
            rightFlipValIsReset = false; //make isreset false so that the value can reset once flipper is put back down.
        }




        //-------------------UPDATING GUI WITH THE VALUES-------------------------

        GetNode<Label>(@"/root/logicnode/leftFlip/value").Text = leftFlipperValue.ToString("0");
		GetNode<Label>(@"/root/logicnode/rightFlip/value").Text = rightFlipperValue.ToString("0");

		GetNode<Label>(@"/root/logicnode/leftFlip/value/history").Text = leftFlipperHistory.ToString("0");
        GetNode<Label>(@"/root/logicnode/rightFlip/value/history").Text = rightFlipperHistory.ToString("0");
    }

    /*
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

        
        if (!Input.IsActionPressed("leftFlipperButton"))
        {
            leftFlipperActive = false;
        }
		 //currently shooting at the dark. but it dosent work here. maybe it works on process..?
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
		else 

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
	  
	*/

    private bool IsDivisible(float valueToCheck, float tickEvery) //https://stackoverflow.com/a/3216515
    {
		bool boolToReturn;

		if (valueToCheck % tickEvery == 0) //if its a multiple / divisible
		{
			boolToReturn = true;
		}
        else //if it isnt 0 (not multiple / divisible)
		{
            boolToReturn = false;
        }

		return boolToReturn;
    }

}
